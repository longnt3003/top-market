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
	public class ManageController : Controller
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;

		public ManageController()
		{
		}

		public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
		{
			UserManager = userManager;
			SignInManager = signInManager;
		}

		public ApplicationSignInManager SignInManager
		{
			get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
			private set { _signInManager = value; }
		}

		public ApplicationUserManager UserManager
		{
			get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
			private set { _userManager = value; }
		}

		public async Task<ActionResult> Index(ManageMessageId? message)
		{
			switch (message)
			{
				case ManageMessageId.ChangePasswordSuccess:
					ViewBag.StatusMessage = "Your password has been changed";
					break;
				case ManageMessageId.SetPasswordSuccess:
					ViewBag.StatusMessage = "Your password has been set";
					break;
				case ManageMessageId.SetTwoFactorSuccess:
					ViewBag.StatusMessage = "Your two-factor authentication provider has been set";
					break;
				case ManageMessageId.Error:
					ViewBag.StatusMessage = "An error has occurred";
					break;
				case ManageMessageId.AddPhoneSuccess:
					ViewBag.StatusMessage = "Your phone number was added";
					break;
				case ManageMessageId.RemovePhoneSuccess:
					ViewBag.StatusMessage = "Your phone number was removed";
					break;
				default:
					ViewBag.StatusMessage = string.Empty;
					break;
			}

			var userId = User.Identity.GetUserId();
			var model = new IndexViewModel
			{
				HasPassword = HasPassword(),
				PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
				TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
				Logins = await UserManager.GetLoginsAsync(userId),
				BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
		{
			var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
			ManageMessageId? message = result.Succeeded
				? ManageMessageId.RemoveLoginSuccess
				: ManageMessageId.Error;

			if (result.Succeeded)
			{
				var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await SignInManager.SignInAsync(
						user,
						isPersistent: false,
						rememberBrowser: false);
				}
			}

			return RedirectToAction("ManageLogins", new { Message = message });
		}

		public ActionResult AddPhoneNumber()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
		{
			if (ModelState.IsValid == false) return View(model);

			var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
			if (UserManager.SmsService != null)
			{
				var message = new IdentityMessage
				{
					Destination = model.Number,
					Body = "Your security code is: " + code
				};
				await UserManager.SmsService.SendAsync(message);
			}

			return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EnableTwoFactorAuthentication()
		{
			await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user != null)
			{
				await SignInManager.SignInAsync(
					user,
					isPersistent: false,
					rememberBrowser: false);
			}

			return RedirectToAction("Index", "Manage");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DisableTwoFactorAuthentication()
		{
			await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user != null)
			{
				await SignInManager.SignInAsync(
					user,
					isPersistent: false,
					rememberBrowser: false);
			}

			return RedirectToAction("Index", "Manage");
		}

		public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
		{
			if (string.IsNullOrEmpty(phoneNumber)) return View("Error");

			await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
			return View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
		{
			if (ModelState.IsValid == false) return View(model);

			var result = await UserManager.ChangePhoneNumberAsync(
				User.Identity.GetUserId(),
				model.PhoneNumber,
				model.Code);
			if (result.Succeeded)
			{
				var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await SignInManager.SignInAsync(
						user,
						isPersistent: false,
						rememberBrowser: false);
				}

				return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
			}

			ModelState.AddModelError(string.Empty, "Failed to verify phone");
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> RemovePhoneNumber()
		{
			var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
			if (result.Succeeded == false) return RedirectToAction("Index", new { Message = ManageMessageId.Error });

			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user != null)
			{
				await SignInManager.SignInAsync(
					user,
					isPersistent: false,
					rememberBrowser: false);
			}

			return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
		}

		public ActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (ModelState.IsValid == false) return View(model);

			var result = await UserManager.ChangePasswordAsync(
				User.Identity.GetUserId(),
				model.OldPassword,
				model.NewPassword);
			if (result.Succeeded)
			{
				var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await SignInManager.SignInAsync(
						user,
						isPersistent: false,
						rememberBrowser: false);
				}

				return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
			}

			AddErrors(result);
			return View(model);
		}

		public ActionResult SetPassword()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
		{
			if (ModelState.IsValid == false) return View(model);

			var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
			if (result.Succeeded)
			{
				var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await SignInManager.SignInAsync(
						user,
						isPersistent: false,
						rememberBrowser: false);
				}

				return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
			}

			AddErrors(result);
			return View(model);
		}

		public async Task<ActionResult> ManageLogins(ManageMessageId? message)
		{
			switch (message)
			{
				case ManageMessageId.RemoveLoginSuccess:
					ViewBag.StatusMessage = "The external login was removed";
					break;
				case ManageMessageId.Error:
					ViewBag.StatusMessage = "An error has occurred";
					break;
				default:
					ViewBag.StatusMessage = string.Empty;
					break;
			}

			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user == null) return View("Error");

			var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
			var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes()
				.Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider))
				.ToList();

			ViewBag.ShowRemoveButton = (user.PasswordHash != null)
				|| (userLogins.Count > 1);

			return View(new ManageLoginsViewModel
			{
				CurrentLogins = userLogins,
				OtherLogins = otherLogins
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LinkLogin(string provider)
		{
			return new AccountController.ChallengeResult(
				provider,
				Url.Action("LinkLoginCallback", "Manage"),
				User.Identity.GetUserId()
			);
		}

		public async Task<ActionResult> LinkLoginCallback()
		{
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
			if (loginInfo == null) return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });

			var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
			return result.Succeeded
				? RedirectToAction("ManageLogins")
				: RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing
				&& (_userManager != null))
			{
				_userManager.Dispose();
				_userManager = null;
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

		private bool HasPassword()
		{
			var user = UserManager.FindById(User.Identity.GetUserId());
			if (user != null) return user.PasswordHash != null;
			return false;
		}

		private bool HasPhoneNumber()
		{
			var user = UserManager.FindById(User.Identity.GetUserId());
			if (user != null) return user.PhoneNumber != null;
			return false;
		}

		public enum ManageMessageId
		{
			AddPhoneSuccess,
			ChangePasswordSuccess,
			SetTwoFactorSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
			RemovePhoneSuccess,
			Error
		}
		#endregion
	}
}