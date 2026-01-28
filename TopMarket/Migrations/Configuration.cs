namespace TopMarket.Migrations
{
	using System.Data.Entity.Migrations;
	using TopMarket.DataSeed;
	using TopMarket.Models;

	internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(ApplicationDbContext context)
		{
			MenuCategorySeeder.Seed(context);
			ProductCategorySeeder.Seed(context);
			ProductSeeder.Seed(context);
			BlogSeeder.Seed(context);
			RoleSeeder.Seed(context);
			UserSeeder.Seed(context);
		}
	}
}
