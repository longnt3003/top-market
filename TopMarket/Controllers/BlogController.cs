using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;

namespace TopMarket.Controllers
{
    public class BlogController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Blog
        public ActionResult Index(int? page, string Searchtext)
        {
            var pageSize = 8;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Blog> items = db.Blogs.OrderByDescending(x => x.DateCreated);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Detail(int id)
        {
            var item = db.Blogs.Find(id);
            return View(item);
        }
        public ActionResult Partial_Blog_Home()
        {
            var items = db.Blogs.Take(3).ToList();
            return PartialView(items);
        }
    }
}