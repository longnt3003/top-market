using System.Linq;
using System.Web.Mvc;
using TopMarket.Models;

namespace TopMarket.Controllers
{
	public class ProductsController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();

		// GET: Products
		public ActionResult Index(string Searchtext)
		{
			var items = db.Products.ToList();

			if (!string.IsNullOrEmpty(Searchtext))
			{
				var item = db.Products.Where(x => x.Title.Contains(Searchtext));
				return View(item.ToList());
			}

			return View(items);
		}

		public ActionResult Detail(string alias, int id)
		{
			var item = db.Products.Find(id);
			var countReview = db.ReviewProducts.Where(x => x.ProductId == id).Count();
			ViewBag.CountReview = countReview;

			return View(item);
		}

		public ActionResult ProductCategory(string alias, int id)
		{
			var items = db.Products.ToList();

			if (id > 0)
			{
				items = items.Where(x => x.ProductCategoryId == id).ToList();
			}

			var cate = db.ProductCategories.Find(id);
			if (cate != null) ViewBag.CateName = cate.Title;

			ViewBag.CateId = id;
			return View(items);
		}

		public ActionResult Partial_ItemsByCateId()
		{
			var items = db.Products
				.Where(x => x.IsHome && x.IsActive)
				.Take(12)
				.ToList();

			return PartialView(items);
		}

		public ActionResult Partial_ProductSales()
		{
			var items = db.Products
				.Where(x => x.IsSale && x.IsActive)
				.Take(12)
				.ToList();

			return PartialView(items);
		}
	}
}
