using System.Linq;
using System.Web.Mvc;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;

namespace TopMarket.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	public class ProductImageController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();

		// GET: Admin/ProductImage
		public ActionResult Index(int id)
		{
			ViewBag.ProductId = id;
			var images = db.ProductImages.Where(x => x.ProductId == id).ToList();
			return View(images);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddImage(int productId, string url)
		{
			if (string.IsNullOrEmpty(url)) return Json(new { success = false, message = "Image URL is required" });

			var image = new ProductImage
			{
				ProductId = productId,
				Image = url,
				IsDefault = false,
			};

			db.ProductImages.Add(image);
			db.SaveChanges();
			return Json(new { success = true });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id)
		{
			var image = db.ProductImages.Find(id);
			if (image != null)
			{
				db.ProductImages.Remove(image);
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
