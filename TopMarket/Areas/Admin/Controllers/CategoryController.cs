using System;
using System.Linq;
using System.Web.Mvc;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;
using CommonFilter = TopMarket.Models.Common.Filter;

namespace TopMarket.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();

		// GET: Admin/Category
		public ActionResult Index()
		{
			var categories = db.Categories.ToList();
			return View(categories);
		}

		public ActionResult Add()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(Category model)
		{
			if (ModelState.IsValid)
			{
				model.DateCreated = DateTime.UtcNow;
				model.DateModified = DateTime.UtcNow;
				model.Alias = CommonFilter.FilterChar(model.Title);

				db.Categories.Add(model);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(model);
		}

		public ActionResult Edit(int id)
		{
			var category = db.Categories.Find(id);
			if (category == null) return HttpNotFound();

			return View(category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Category model)
		{
			if (ModelState.IsValid)
			{
				var category = db.Categories.Find(model.Id);
				if (category == null) return HttpNotFound();

				category.Title = model.Title;
				category.Description = model.Description;
				category.Link = model.Link;
				category.Position = model.Position;
				category.DateModified = DateTime.UtcNow;
				category.Alias = CommonFilter.FilterChar(model.Title);

				db.Entry(category).State = System.Data.Entity.EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(model);
		}

		public ActionResult Delete(int id)
		{
			var category = db.Categories.Find(id);
			if (category != null)
			{
				db.Categories.Remove(category);
				db.SaveChanges();
				return Json(new { success = true });
			}

			return Json(new { success = false });
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing) db.Dispose();
			base.Dispose(disposing);
		}
	}
}
