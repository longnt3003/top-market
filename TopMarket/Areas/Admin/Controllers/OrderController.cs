using PagedList;
using System.Linq;
using System.Web.Mvc;
using TopMarket.Models;

namespace TopMarket.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	public class OrderController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();
		private const int PageSize = 8;

		// GET: Admin/Order
		public ActionResult Index(int? page)
		{
			var pageNumber = page ?? 1;
			var orders = db.Orders.OrderByDescending(x => x.DateCreated);
			ViewBag.PageSize = PageSize;
			ViewBag.Page = pageNumber;
			return View(orders.ToPagedList(pageNumber, PageSize));
		}

		// GET: Admin/Order/Details/5
		public ActionResult Details(int id)
		{
			var order = db.Orders.Find(id);
			if (order == null) return HttpNotFound();

			return View(order);
		}

		public ActionResult Partial_Product(int id)
		{
			var items = db.OrderDetails.Where(x => x.OrderId == id).ToList();
			return PartialView(items);
		}

		[HttpPost]
		public ActionResult UpdateStatus(int id, int status)
		{
			var order = db.Orders.Find(id);
			if (order != null)
			{
				order.PaymentMethods = status;
				db.Entry(order).Property(x => x.PaymentMethods).IsModified = true;
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
