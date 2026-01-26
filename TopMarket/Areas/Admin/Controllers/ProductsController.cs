using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;
using CommonFilter = TopMarket.Models.Common.Filter;

namespace TopMarket.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin, Employee")]
	public class ProductsController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();
		private const int PageSize = 8;

		// GET: Admin/Product
		public ActionResult Index(int? page)
		{
			var pageIndex = page ?? 1;
			var products = db.Products.OrderByDescending(x => x.Id);
			ViewBag.PageSize = PageSize;
			ViewBag.Page = pageIndex;
			return View(products.ToPagedList(pageIndex, PageSize));
		}

		public ActionResult Add()
		{
			ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(Product model, List<string> images, List<int> rDefault)
		{
			if (ModelState.IsValid)
			{
				if ((images != null)
					&& (images.Count > 0)
					&& (rDefault != null)
					&& (rDefault.Count > 0))
				{
					for (int i = 0; i < images.Count; i++)
					{
						bool isDefault = (i + 1 == rDefault[0]);
						if (isDefault) model.Image = images[i];

						model.ProductImages.Add(new ProductImage
						{
							Image = images[i],
							IsDefault = isDefault
						});
					}
				}

				model.DateCreated = DateTime.UtcNow;
				model.DateModified = DateTime.UtcNow;
				if (string.IsNullOrEmpty(model.Alias)) model.Alias = CommonFilter.FilterChar(model.Title);

				db.Products.Add(model);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
			return View(model);
		}

		public ActionResult Edit(int id)
		{
			ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
			var product = db.Products.Find(id);
			if (product == null) return HttpNotFound();

			return View(product);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Product model)
		{
			if (ModelState.IsValid)
			{
				var product = db.Products.Find(model.Id);
				if (product == null) return HttpNotFound();

				product.Title = model.Title;
				product.Description = model.Description;
				product.Price = model.Price;
				product.Alias = CommonFilter.FilterChar(model.Title);

				db.Entry(product).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(model);
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			var product = db.Products.Find(id);
			if (product != null)
			{
				var images = product.ProductImages.ToList();
				foreach (var img in images)
				{
					db.ProductImages.Remove(img);
				}

				db.Products.Remove(product);
				db.SaveChanges();
				return Json(new { success = true });
			}

			return Json(new { success = false });
		}

		[HttpPost]
		public ActionResult IsActive(int id)
		{
			var product = db.Products.Find(id);
			if (product != null)
			{
				product.IsActive = !product.IsActive;
				db.Entry(product).State = EntityState.Modified;
				db.SaveChanges();
				return Json(new { success = true, isActive = product.IsActive });
			}

			return Json(new { success = false });
		}

		[HttpPost]
		public ActionResult IsHome(int id)
		{
			var product = db.Products.Find(id);
			if (product != null)
			{
				product.IsHome = !product.IsHome;
				db.Entry(product).State = EntityState.Modified;
				db.SaveChanges();
				return Json(new { success = true, isHome = product.IsHome });
			}

			return Json(new { success = false });
		}

		[HttpPost]
		public ActionResult IsSale(int id)
		{
			var product = db.Products.Find(id);
			if (product != null)
			{
				product.IsSale = product.IsSale == false;
				db.Entry(product).State = EntityState.Modified;
				db.SaveChanges();
				return Json(new { success = true, isSale = product.IsSale });
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
