using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Web.Mvc;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;

namespace TopMarket.Controllers
{
	[Authorize]
	public class ReviewController : Controller
	{
		private readonly ApplicationDbContext _db = new ApplicationDbContext();

		public ActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult _Review(int productId)
		{
			ViewBag.ProductId = productId;
			var item = new ReviewProduct();

			if (User.Identity.IsAuthenticated)
			{
				using (var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext()))
				{
					var userManager = new UserManager<ApplicationUser>(userStore);
					var user = userManager.FindByName(User.Identity.Name);

					if (user != null)
					{
						item.Email = user.Email;
						item.FullName = user.FullName;
						item.UserName = user.UserName;
					}
				}

				return PartialView(item);
			}

			return PartialView();
		}

		[AllowAnonymous]
		public ActionResult _Load_Review(int productId)
		{
			var items = _db.ReviewProducts
				.Where(x => x.ProductId == productId)
				.OrderByDescending(x => x.Id)
				.ToList();

			ViewBag.Count = items.Count;
			return PartialView(items);
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult PostReview(ReviewProduct req)
		{
			if (ModelState.IsValid == false) return Json(new { Success = false });

			req.DateCreated = DateTime.Now;
			_db.ReviewProducts.Add(req);
			_db.SaveChanges();

			return Json(new { Success = true });
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing) _db.Dispose();
			base.Dispose(disposing);
		}
	}
}
