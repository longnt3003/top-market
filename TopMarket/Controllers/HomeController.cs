using System.Web.Mvc;
using TopMarket.Models;

namespace TopMarket.Controllers
{
	public class HomeController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult About()
		{
			ViewBag.Message = "Your application description page";
			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";
			return View();
		}
	}
}