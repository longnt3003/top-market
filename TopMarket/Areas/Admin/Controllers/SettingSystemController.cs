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
		public ActionResult Index() => View();

		public ActionResult Partial_Setting() => PartialView();

		[HttpPost]
		public ActionResult AddSetting(SettingSystemViewModel req)
		{
			UpsertSetting("SettingTitle", req.SettingTitle);
			UpsertSetting("SettingLogo", req.SettingLogo);
			UpsertSetting("SettingEmail", req.SettingEmail);
			UpsertSetting("SettingHotline", req.SettingHotline);

			db.SaveChanges();

			return View("Partial_Setting");
		}

		private void UpsertSetting(string key, string value)
		{
			var setting = db.SystemSettings
				.FirstOrDefault(x => x.SettingKey == key);

			if (setting == null)
			{
				db.SystemSettings.Add(new SystemSetting
				{
					SettingKey = key,
					SettingValue = value
				});
			}
			else
			{
				setting.SettingValue = value;
				db.Entry(setting).State = EntityState.Modified;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db?.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}