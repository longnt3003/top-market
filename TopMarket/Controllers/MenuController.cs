using System.Linq;
using System.Web.Mvc;
using TopMarket.Models;

namespace TopMarket.Controllers
{
	public class MenuController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult MenuTop()
		{
			var items = db.Categories.OrderBy(x => x.Position).ToList();
			return PartialView("_MenuTop", items);
		}

		public ActionResult MenuProductCategory()
		{
			var items = db.ProductCategories.ToList();
			return PartialView("_MenuProductCategory", items);
		}

		public ActionResult MenuLeft(int? id)
		{
			if (id.HasValue) ViewBag.CateId = id.Value;

			var items = db.ProductCategories.ToList();
			return PartialView("_MenuLeft", items);
		}

		public ActionResult MenuArrivals()
		{
			var items = db.ProductCategories.ToList();
			return PartialView("_MenuArrivals", items);
		}
	}
}
