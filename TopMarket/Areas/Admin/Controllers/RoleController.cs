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
			var items = db.Roles.ToList();
			return View(items);
		}

		public ActionResult Create() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IdentityRole model)
		{
			if (!ModelState.IsValid) return View(model);

			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
			roleManager.Create(model);

			return RedirectToAction("Index");
		}

		public ActionResult Edit(int id)
		{
			var item = db.Roles.Find(id);
			return View(item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(IdentityRole model)
		{
			if (!ModelState.IsValid) return View(model);

			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
			roleManager.Update(model);

			return RedirectToAction("Index");
		}
	}
}
