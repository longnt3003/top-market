using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TopMarket.Models;

namespace TopMarket.DataSeed
{
	public static class RoleSeeder
	{
		public static void Seed(ApplicationDbContext context)
		{
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

			if (roleManager.RoleExists("Admin") == false)
			{
				roleManager.Create(new IdentityRole("Admin"));
			}
		}
	}
}
