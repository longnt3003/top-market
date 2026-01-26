using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;
using System.Data.Entity;
using CommonFilter = TopMarket.Models.Common.Filter;

namespace TopMarket.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin, Employee")]
	public class BlogController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();
		private const int PageSize = 8;

		// GET: Admin/Blog
		public ActionResult Index(string searchText, int? page)
		{
			var pageIndex = page ?? 1;
			IEnumerable<Blog> items = db.Blogs.OrderByDescending(x => x.Id);

			if (string.IsNullOrEmpty(searchText) == false)
			{
				items = items.Where(x =>
					(x.Alias != null && x.Alias.Contains(searchText))
					|| (x.Title != null && x.Title.Contains(searchText)));
			}

			items = items.ToPagedList(pageIndex, PageSize);
			ViewBag.PageSize = PageSize;
			ViewBag.Page = pageIndex;
			return View(items);
		}

		public ActionResult Add()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(Blog model)
		{
			if (ModelState.IsValid)
			{
				model.DateCreated = DateTime.UtcNow;
				model.DateModified = DateTime.UtcNow;
				model.CategoryId = 1;
				model.Alias = CommonFilter.FilterChar(model.Title);

				db.Blogs.Add(model);
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(model);
		}

		public ActionResult Edit(int id)
		{
			var item = db.Blogs.Find(id);
			if (item == null) return HttpNotFound();
			return View(item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Blog model)
		{
			if (ModelState.IsValid)
			{
				model.DateModified = DateTime.UtcNow;
				model.Alias = CommonFilter.FilterChar(model.Title);

				db.Blogs.Attach(model);
				db.Entry(model).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(model);
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			var item = db.Blogs.Find(id);
			if (item != null)
			{
				db.Blogs.Remove(item);
				db.SaveChanges();
				return Json(new { success = true });
			}

			return Json(new { success = false });
		}

		[HttpPost]
		public ActionResult IsActive(int id)
		{
			var item = db.Blogs.Find(id);
			if (item != null)
			{
				item.IsActive = !item.IsActive;
				db.Entry(item).State = EntityState.Modified;
				db.SaveChanges();
				return Json(new { success = true, isAct = item.IsActive });
			}

			return Json(new { success = false });
		}

		[HttpPost]
		public ActionResult DeleteAll(string ids)
		{
			if (!string.IsNullOrEmpty(ids))
			{
				var idList = ids.Split(',').Select(int.Parse).ToList();
				var blogs = db.Blogs.Where(x => idList.Contains(x.Id)).ToList();

				foreach (var blog in blogs)
				{
					db.Blogs.Remove(blog);
				}

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
