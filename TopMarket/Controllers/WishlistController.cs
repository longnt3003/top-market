using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;

namespace TopMarket.Controllers
{
	[Authorize]
	public class WishlistController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();

		public ActionResult Index(int? page)
		{
			int pageSize = 10;
			int pageIndex = page.HasValue
				? Convert.ToInt32(page)
				: 1;

			IEnumerable<Wishlist> items = db.Wishlists
				.Where(x => x.UserName == User.Identity.Name)
				.OrderByDescending(x => x.DateCreated)
				.ToPagedList(pageIndex, pageSize);

			ViewBag.PageSize = pageSize;
			ViewBag.Page = pageIndex;

			return View(items);
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult PostWishlist(int productId)
		{
			if (Request.IsAuthenticated == false) return Json(new { Success = false, Message = "You need to login first" });

			var checkItem = db.Wishlists.FirstOrDefault(
				x => x.ProductId == productId
				&& (x.UserName == User.Identity.Name));
			if (checkItem != null) return Json(new { Success = false, Message = "This product is already in your favorites list" });

			var item = new Wishlist
			{
				ProductId = productId,
				UserName = User.Identity.Name,
				DateCreated = DateTime.Now
			};

			db.Wishlists.Add(item);
			db.SaveChanges();

			return Json(new { Success = true });
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult PostDeleteWishlist(int productId)
		{
			var checkItem = db.Wishlists.FirstOrDefault(
				x => x.ProductId == productId
				&& (x.UserName == User.Identity.Name));
			if (checkItem != null)
			{
				db.Wishlists.Remove(checkItem);
				db.SaveChanges();
				return Json(new { Success = true, Message = "Delete successfully" });
			}

			return Json(new { Success = false, Message = "Delete failed" });
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing) db.Dispose();
			base.Dispose(disposing);
		}
	}
}
