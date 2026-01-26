using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;

namespace TopMarket.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	public class SettingSystemController : Controller
	{
		private readonly ApplicationDbContext db = new ApplicationDbContext();

		// GET: Admin/SettingSystem
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Partial_Setting()
		{
			var settings = db.SystemSettings.ToList();
			return PartialView(settings);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddSetting(SettingSystemViewModel req)
		{
			UpdateOrCreateSetting("SettingTitle", req.SettingTitle);
			UpdateOrCreateSetting("SettingLogo", req.SettingLogo);
			UpdateOrCreateSetting("SettingEmail", req.SettingEmail);
			UpdateOrCreateSetting("SettingHotline", req.SettingHotline);

			db.SaveChanges();
			return View("Partial_Setting", db.SystemSettings.ToList());
		}

		private void UpdateOrCreateSetting(string key, string value)
		{
			var setting = db.SystemSettings.FirstOrDefault(x => x.SettingKey == key);
			if (setting == null)
			{
				var newSetting = new SystemSetting
				{
					SettingKey = key,
					SettingValue = value
				};

				db.SystemSettings.Add(newSetting);
			}
			else
			{
				setting.SettingValue = value;
				db.Entry(setting).State = EntityState.Modified;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing) db.Dispose();
			base.Dispose(disposing);
		}
	}
}
