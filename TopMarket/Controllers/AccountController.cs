using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TopMarket.Models;

namespace TopMarket.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;

		public AccountController()
		{
		}

		public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
		{
			SignInManager = signInManager;
			UserManager = userManager;
		}

		public ApplicationSignInManager SignInManager
		{
			get
			{
				if (_signInManager == null) _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
				return _signInManager;
			}
			private set { _signInManager = value; }
		}

		public ApplicationUserManager UserManager
		{
			get
			{
				if (_userManager == null) _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
				return _userManager;
			}
			private set { _userManager = value; }
		}

		public async Task<ActionResult> UserProfile()
		{
			var user = await UserManager.FindByNameAsync(User.Identity.Name);
			if (user == null) return HttpNotFound();

			var item = new CreateAccountViewModel
			{
				Email = user.Email,
				FullName = user.FullName,
				Phone = user.Phone,
				UserName = user.UserName
			};

			return View(item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> PostProfile(ApplicationUser req)
		{
			var user = await UserManager.FindByEmailAsync(req.Email);
			if (user == null) return HttpNotFound();

			user.FullName = req.FullName;
			user.Phone = req.Phone;

			var result = await UserManager.UpdateAsync(user);
			if (result.Succeeded) return RedirectToAction("Profile");

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error);
			}

			return View(req);
		}

		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (ModelState.IsValid == false) return View(model);

			var result = await SignInManager.PasswordSignInAsync(
				model.UserName,
				model.Password,
				model.RememberMe,
				shouldLockout: false);

			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(returnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
				default:
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					return View(model);
			}
		}

		[AllowAnonymous]
		public async Task<ActionResult> VerifyCode(
			string provider,
			string returnUrl,
			bool rememberMe)
		{
			if (await SignInManager.HasBeenVerifiedAsync() == false) return View("Error");

			return View(new VerifyCodeViewModel
			{
				Provider = provider,
				ReturnUrl = returnUrl,
				RememberMe = rememberMe
			});
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
		{
			if (ModelState.IsValid == false) return View(model);

			var result = await SignInManager.TwoFactorSignInAsync(
				model.Provider,
				model.Code,
				isPersistent: model.RememberMe,
				rememberBrowser: model.RememberBrowser);

			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(model.ReturnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				default:
					ModelState.AddModelError(string.Empty, "Invalid code.");
					return View(model);
			}
		}

		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
			if ((model == null)
				|| (ModelState.IsValid) == false) return View(model);

			var user = new ApplicationUser
			{
				UserName = model.Email,
				Email = model.Email
			};

			var result = await UserManager.CreateAsync(user, model.Password);
			if (result.Succeeded)
			{
				await SignInManager.SignInAsync(
					user,
					isPersistent: false,
					rememberBrowser: false);

				var roleResult = await UserManager.AddToRoleAsync(user.Id, "Customer");
				if (roleResult.Succeeded == false)
				{
					foreach (var error in roleResult.Errors)
					{
						ModelState.AddModelError(string.Empty, error);
					}

					return View(model);
				}

				return RedirectToAction("Index", "Home");
			}

			AddErrors(result);
			return View(model);
		}

		[AllowAnonymous]
		public async Task<ActionResult> ConfirmEmail(string userId, string code)
		{
			if (string.IsNullOrEmpty(userId)
				|| string.IsNullOrEmpty(code)) return View("Error");

			var result = await UserManager.ConfirmEmailAsync(userId, code);
			return View(result.Succeeded
				? "ConfirmEmail"
				: "Error");
		}

		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (ModelState.IsValid == false) return View(model);

			var user = await UserManager.FindByNameAsync(model.Email);
			if ((user == null)
				|| (await UserManager.IsEmailConfirmedAsync(user.Id) == false)) return View("ForgotPasswordConfirmation");

			return View("ForgotPasswordConfirmation");
		}

		[AllowAnonymous]
		public ActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult ResetPassword(string code)
		{
			return string.IsNullOrEmpty(code)
				? View("Error")
				: View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if ((model == null)
				|| (ModelState.IsValid) == false) return View(model);

			var user = await UserManager.FindByNameAsync(model.Email);
			if (user == null) return RedirectToAction("ResetPasswordConfirmation", "Account");

			var result = await UserManager.ResetPasswordAsync(
				user.Id,
				model.Code,
				model.Password);
			if (result.Succeeded) return RedirectToAction("ResetPasswordConfirmation", "Account");

			AddErrors(result);
			return View(model);
		}

		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			return new ChallengeResult(
				provider,
				Url.Action(
					"ExternalLoginCallback",
					"Account",
					new { ReturnUrl = returnUrl }));
		}

		[AllowAnonymous]
		public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
		{
			var userId = await SignInManager.GetVerifiedUserIdAsync();
			if (userId == null) return View("Error");

			var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
			var factorOptions = userFactors
				.Select(purpose => new SelectListItem { Text = purpose, Value = purpose })
				.ToList();

			return View(new SendCodeViewModel
			{
				Providers = factorOptions,
				ReturnUrl = returnUrl,
				RememberMe = rememberMe
			});
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SendCode(SendCodeViewModel model)
		{
			if ((model == null)
				|| (ModelState.IsValid) == false) return View(model);

			var success = await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider);
			if (success == false)
			{
				ModelState.AddModelError(string.Empty, "Unable to send verification code.");
				return View("Error");
			}

			return RedirectToAction("VerifyCode", new
			{
				Provider = model.SelectedProvider,
				ReturnUrl = model.ReturnUrl,
				RememberMe = model.RememberMe
			});
		}

		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
		{
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null) return RedirectToAction("Login");

			var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(returnUrl ?? Url.Action("Index", "Home"));
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
				default:
					ViewBag.ReturnUrl = returnUrl;
					ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
					return View("ExternalLoginConfirmation",
						new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
			}
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
		{
			if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Manage");

			if ((model == null)
				|| (ModelState.IsValid) == false)
			{
				ViewBag.ReturnUrl = returnUrl;
				return View(model);
			}

			var info = await AuthenticationManager.GetExternalLoginInfoAsync();
			if (info == null) return View("ExternalLoginFailure");

			var user = new ApplicationUser
			{
				UserName = model.Email,
				Email = model.Email
			};

			var result = await UserManager.CreateAsync(user);
			if (result.Succeeded)
			{
				var loginResult = await UserManager.AddLoginAsync(user.Id, info.Login);
				if (loginResult.Succeeded)
				{
					await SignInManager.SignInAsync(
						user,
						isPersistent: false,
						rememberBrowser: false);
					return RedirectToLocal(returnUrl);
				}

				AddErrors(loginResult);
			}
			else
			{
				AddErrors(result);
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Index", "Home");
		}

		[AllowAnonymous]
		public ActionResult ExternalLoginFailure()
		{
			return View();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_userManager?.Dispose();
				_userManager = null;

				_signInManager?.Dispose();
				_signInManager = null;
			}

			base.Dispose(disposing);
		}

		#region Helpers
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get { return HttpContext.GetOwinContext().Authentication; }
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error);
			}
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
			return RedirectToAction("Index", "Home");
		}

		internal class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(
				string provider,
				string redirectUri,
				string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; }
			public string RedirectUri { get; }
			public string UserId { get; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
				if (UserId != null) properties.Dictionary[XsrfKey] = UserId;
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion
	}
}