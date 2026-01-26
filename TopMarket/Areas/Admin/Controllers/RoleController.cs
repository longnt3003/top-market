using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Mvc;
using TopMarket.Models;

namespace TopMarket.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	public class RoleController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();

		// GET: Admin/Role
		public ActionResult Index()
		{
			var roles = db.Roles.ToList();
			return View(roles);
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IdentityRole model)
		{
			if (ModelState.IsValid)
			{
				var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
				if (roleManager.RoleExists(model.Name) == false)
				{
					roleManager.Create(model);
					return RedirectToAction("Index");
				}

				ModelState.AddModelError(string.Empty, "Role already exists");
			}

			return View(model);
		}

		public ActionResult Edit(string id)
		{
			var role = db.Roles.FirstOrDefault(r => r.Id == id);
			if (role == null) return HttpNotFound();

			return View(role);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(IdentityRole model)
		{
			if (ModelState.IsValid)
			{
				var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
				var existingRole = roleManager.FindById(model.Id);
				if (existingRole != null)
				{
					existingRole.Name = model.Name;
					roleManager.Update(existingRole);
					return RedirectToAction("Index");
				}

				ModelState.AddModelError(string.Empty, "Role not found");
			}

			return View(model);
		}

		public ActionResult Delete(string id)
		{
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
			var role = roleManager.FindById(id);
			if (role != null)
			{
				roleManager.Delete(role);
				return RedirectToAction("Index");
			}

			return HttpNotFound();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing) db.Dispose();
			base.Dispose(disposing);
		}
	}
}
