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

		// GET: Admin/Order
		public ActionResult Index(int? page)
		{
			var items = db.Orders.OrderByDescending(x => x.DateCreated).ToList();

			if (page == null) page = 1;

			var pageNumber = page ?? 1;
			var pageSize = 8;

			ViewBag.PageSize = pageSize;
			ViewBag.Page = pageNumber;

			return View(items.ToPagedList(pageNumber, pageSize));
		}

		public ActionResult View(int id)
		{
			var item = db.Orders.Find(id);
			return View(item);
		}

		public ActionResult Partial_Product(int id)
		{
			var items = db.OrderDetails.Where(x => x.OrderId == id).ToList();
			return PartialView(items);
		}

		[HttpPost]
		public ActionResult UpdateStatus(int id, int status)
		{
			var item = db.Orders.Find(id);
			if (item != null)
			{
				db.Orders.Attach(item);
				item.PaymentMethods = status;
				db.Entry(item).Property(x => x.PaymentMethods).IsModified = true;
				db.SaveChanges();
				return Json(new { message = "Success", Success = true });
			}
			return Json(new { message = "Fail", Success = false });
		}
	}
}
