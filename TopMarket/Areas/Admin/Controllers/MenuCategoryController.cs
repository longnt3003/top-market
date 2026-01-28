using System;
using System.Web.Mvc;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;

namespace TopMarket.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	public class MenuCategoryController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();

		// GET: Admin/Category
		public ActionResult Index()
		{
			var items = db.Categories;
			return View(items);
		}

		public ActionResult Add() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(MenuCategory model)
		{
			if (!ModelState.IsValid) return View(model);

			model.DateCreated = DateTime.Now;
			model.DateModified = DateTime.Now;
			model.Alias = TopMarket.Models.Common.Filter.FilterChar(model.Title);

			db.Categories.Add(model);
			db.SaveChanges();

			return RedirectToAction("Index");
		}

		public ActionResult Edit(int id)
		{
			var item = db.Categories.Find(id);
			return View(item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(MenuCategory model)
		{
			if (!ModelState.IsValid) return View(model);

			db.Categories.Attach(model);
			model.DateModified = DateTime.Now;
			model.Alias = TopMarket.Models.Common.Filter.FilterChar(model.Title);

			db.Entry(model).Property(x => x.Title).IsModified = true;
			db.Entry(model).Property(x => x.Alias).IsModified = true;
			db.Entry(model).Property(x => x.Description).IsModified = true;
			db.Entry(model).Property(x => x.Link).IsModified = true;
			db.Entry(model).Property(x => x.Position).IsModified = true;
			db.Entry(model).Property(x => x.DateModified).IsModified = true;

			db.SaveChanges();
			return RedirectToAction("Index");
		}

		public ActionResult Delete(int id)
		{
			var item = db.Categories.Find(id);
			if (item == null) return Json(new { success = false });

			db.Categories.Remove(item);
			db.SaveChanges();

			return Json(new { success = true });
		}
	}
}
