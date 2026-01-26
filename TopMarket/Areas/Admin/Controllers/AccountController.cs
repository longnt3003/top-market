using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TopMarket.Models;

namespace TopMarket.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AccountController : Controller
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		private readonly ApplicationDbContext _db = new ApplicationDbContext();

		public AccountController()
		{
		}

		public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

		// GET: Admin/Account
		public ActionResult Index()
		{
			var users = _db.Users.ToList();
			return View(users);
		}

		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		// POST: /Account/Login
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
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError(string.Empty, "Invalid login attempt");
					return View(model);
			}
		}

		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Index", "Home");
		}

		// GET: /Account/Create
		[AllowAnonymous]
		public ActionResult Create()
		{
			ViewBag.Role = new SelectList(_db.Roles.ToList(), "Name", "Name");
			return View();
		}

		// POST: /Account/Create
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(CreateAccountViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					UserName = model.UserName,
					Email = model.Email,
					FullName = model.FullName,
					Phone = model.Phone
				};

				var result = await UserManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					if (model.Roles != null)
					{
						foreach (var role in model.Roles)
						{
							UserManager.AddToRole(user.Id, role);
						}
					}

					return RedirectToAction("Index");
				}

				AddErrors(result);
			}

			ViewBag.Role = new SelectList(_db.Roles.ToList(), "Name", "Name");
			return View(model);
		}

		// GET: /Account/Edit
		public ActionResult Edit(string id)
		{
			var user = UserManager.FindById(id);
			if (user == null) return HttpNotFound();

			var rolesForUser = UserManager.GetRoles(id) ?? new List<string>();
			var model = new EditAccountViewModel
			{
				FullName = user.FullName,
				Email = user.Email,
				Phone = user.Phone,
				UserName = user.UserName,
				Roles = rolesForUser.ToList()
			};

			ViewBag.Role = new SelectList(_db.Roles, "Name", "Name");
			return View(model);
		}

		// POST: /Account/Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(EditAccountViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = UserManager.FindByName(model.UserName);
				if (user == null) return HttpNotFound();

				user.FullName = model.FullName;
				user.Phone = model.Phone;
				user.Email = model.Email;

				var result = await UserManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					var rolesForUser = UserManager.GetRoles(user.Id);

					// Remove roles not in model
					foreach (var role in rolesForUser)
					{
						if (model.Roles == null || model.Roles.Contains(role) == false)
						{
							await UserManager.RemoveFromRoleAsync(user.Id, role);
						}
					}

					// Add new roles
					if (model.Roles != null)
					{
						foreach (var role in model.Roles)
						{
							if (rolesForUser.Contains(role) == false)
							{
								UserManager.AddToRole(user.Id, role);
							}
						}
					}

					return RedirectToAction("Index");
				}

				AddErrors(result);
			}

			ViewBag.Role = new SelectList(_db.Roles.ToList(), "Name", "Name");
			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> DeleteAccount(string id)
		{
			var response = new { Success = false };
			var user = UserManager.FindById(id);
			if (user != null)
			{
				var rolesForUser = UserManager.GetRoles(id);
				if (rolesForUser != null)
				{
					foreach (var role in rolesForUser)
					{
						await UserManager.RemoveFromRoleAsync(id, role);
					}
				}

				var result = await UserManager.DeleteAsync(user);
				response = new { Success = result.Succeeded };
			}

			return Json(response);
		}

		private IAuthenticationManager AuthenticationManager
		{
			get { return HttpContext.GetOwinContext().Authentication; }
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

			return RedirectToAction("Index", "Home");
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing) _db.Dispose();
			base.Dispose(disposing);
		}
	}
}
