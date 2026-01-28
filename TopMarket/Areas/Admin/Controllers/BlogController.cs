using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;

namespace TopMarket.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin, Employee")]
	public class BlogController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();

		// GET: Admin/Blog
		public ActionResult Index(string Searchtext, int? page)
		{
			var pageSize = 8;
			if (page == null) page = 1;

			IEnumerable<Blog> items = db.Blogs.OrderByDescending(x => x.Id);

			if (!string.IsNullOrEmpty(Searchtext))
			{
				items = items.Where(x =>
					x.Alias.Contains(Searchtext)
					&& x.Title.Contains(Searchtext));
			}

			var pageIndex = page.HasValue
				? Convert.ToInt32(page)
				: 1;
			items = items.ToPagedList(pageIndex, pageSize);

			ViewBag.PageSize = pageSize;
			ViewBag.Page = page;

			return View(items);
		}

		public ActionResult Add() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(Blog model)
		{
			if (!ModelState.IsValid) return View(model);

			model.DateCreated = DateTime.Now;
			model.DateModified = DateTime.Now;
			model.Alias = TopMarket.Models.Common.Filter.FilterChar(model.Title);

			db.Blogs.Add(model);
			db.SaveChanges();

			return RedirectToAction("Index");
		}

		public ActionResult Edit(int id)
		{
			var item = db.Blogs.Find(id);
			return View(item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Blog model)
		{
			if (!ModelState.IsValid) return View(model);

			model.DateCreated = DateTime.Now;
			model.DateModified = DateTime.Now;
			model.Alias = TopMarket.Models.Common.Filter.FilterChar(model.Title);

			db.Blogs.Attach(model);
			db.Entry(model).State = System.Data.Entity.EntityState.Modified;
			db.SaveChanges();

			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			var item = db.Blogs.Find(id);
			if (item == null) return Json(new { success = false });

			db.Blogs.Remove(item);
			db.SaveChanges();

			return Json(new { success = true });
		}

		[HttpPost]
		public ActionResult IsActive(int id)
		{
			var item = db.Blogs.Find(id);
			if (item == null) return Json(new { success = false });

			item.IsActive = !item.IsActive;
			db.Entry(item).State = System.Data.Entity.EntityState.Modified;
			db.SaveChanges();

			return Json(new { success = true, isAct = item.IsActive });
		}

		[HttpPost]
		public ActionResult DeleteAll(string ids)
		{
			if (string.IsNullOrEmpty(ids)) return Json(new { success = false });

			var items = ids.Split(',');
			if (items != null && items.Any())
			{
				foreach (var item in items)
				{
					var obj = db.Blogs.Find(Convert.ToInt32(item));
					if (obj != null)
					{
						db.Blogs.Remove(obj);
						db.SaveChanges();
					}
				}
			}

			return Json(new { success = true });
		}
	}
}
