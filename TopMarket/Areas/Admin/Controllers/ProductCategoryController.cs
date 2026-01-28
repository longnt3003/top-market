using System;
using System.Web.Mvc;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;

namespace TopMarket.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	public class ProductCategoryController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();

		// GET: Admin/ProductCategory
		public ActionResult Index()
		{
			var items = db.ProductCategories;
			return View(items);
		}

		public ActionResult Add() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(ProductCategory model)
		{
			if (!ModelState.IsValid) return View();

			model.DateCreated = DateTime.Now;
			model.DateModified = DateTime.Now;
			model.Alias = TopMarket.Models.Common.Filter.FilterChar(model.Title);

			db.ProductCategories.Add(model);
			db.SaveChanges();

			return RedirectToAction("Index");
		}

		public ActionResult Edit(int id)
		{
			var item = db.ProductCategories.Find(id);
			return View(item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ProductCategory model)
		{
			if (!ModelState.IsValid) return View(model);

			model.DateModified = DateTime.Now;
			model.Alias = TopMarket.Models.Common.Filter.FilterChar(model.Title);

			db.ProductCategories.Attach(model);
			db.Entry(model).State = System.Data.Entity.EntityState.Modified;
			db.SaveChanges();

			return RedirectToAction("Index");
		}

		public ActionResult Delete(int id)
		{
			var item = db.ProductCategories.Find(id);
			if (item == null) return Json(new { success = false });

			db.ProductCategories.Remove(item);
			db.SaveChanges();

			return Json(new { success = true });
		}
	}
}
