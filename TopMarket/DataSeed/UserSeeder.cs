using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TopMarket.Models;

namespace TopMarket.DataSeed
{
	public static class UserSeeder
	{
		public static void Seed(ApplicationDbContext context)
		{
			var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

			var adminEmail = "admin@gmail.com";
			var adminUser = userManager.FindByEmail(adminEmail);
			if (adminUser == null)
			{
				adminUser = new ApplicationUser
				{
					UserName = adminEmail,
					Email = adminEmail,
					EmailConfirmed = true,
					FullName = "System Administrator",
					Phone = "1234567890"
				};
				userManager.Create(adminUser, "admin123");
			}

			if (userManager.IsInRole(adminUser.Id, "Admin") == false)
			{
				userManager.AddToRole(adminUser.Id, "Admin");
			}
		}
	}
}
