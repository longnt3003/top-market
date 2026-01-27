using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;

namespace TopMarket.Controllers
{
	public class BlogController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();

		public ActionResult Index(int? page, string searchText)
		{
			int pageSize = 8;
			int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

			IEnumerable<Blog> items = db.Blogs.OrderByDescending(x => x.DateCreated);

			if (string.IsNullOrEmpty(searchText) == false)
			{
				items = items.Where(
					x => x.Alias.Contains(searchText)
					|| x.Title.Contains(searchText));
			}

			items = items.ToPagedList(pageIndex, pageSize);

			ViewBag.PageSize = pageSize;
			ViewBag.Page = pageIndex;

			return View(items);
		}

		public ActionResult Detail(int id)
		{
			var item = db.Blogs.Find(id);
			if (item == null) return HttpNotFound();

			return View(item);
		}

		public ActionResult Partial_Blog_Home()
		{
			var items = db.Blogs
				.OrderByDescending(x => x.DateCreated)
				.Take(3)
				.ToList();
			return PartialView(items);
		}
	}
}
