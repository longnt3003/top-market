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
		private readonly ApplicationDbContext db = new ApplicationDbContext();

		// GET: Review
		public ActionResult Index() => View();

		[AllowAnonymous]
		public ActionResult _Review(int productId)
		{
			ViewBag.ProductId = productId;
			var item = new ReviewProduct();

			if (User.Identity.IsAuthenticated)
			{
				var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
				var userManager = new UserManager<ApplicationUser>(userStore);
				var user = userManager.FindByName(User.Identity.Name);

				if (user != null)
				{
					item.Email = user.Email;
					item.FullName = user.FullName;
					item.UserName = user.UserName;
				}

				return PartialView(item);
			}

			return PartialView();
		}

		[AllowAnonymous]
		public ActionResult _Load_Review(int productId)
		{
			var item = db.ReviewProducts
				.Where(x => x.ProductId == productId)
				.OrderByDescending(x => x.Id)
				.ToList();

			ViewBag.Count = item.Count;
			return PartialView(item);
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult PostReview(ReviewProduct req)
		{
			if (ModelState.IsValid)
			{
				req.DateCreated = DateTime.Now;
				db.ReviewProducts.Add(req);
				db.SaveChanges();
				return Json(new { Success = true });
			}

			return Json(new { Success = false });
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}
