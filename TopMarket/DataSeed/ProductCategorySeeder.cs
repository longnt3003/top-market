using System;
using System.Data.Entity.Migrations;
using System.Linq;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;

namespace TopMarket.DataSeed
{
	public static class ProductCategorySeeder
	{
		public static void Seed(ApplicationDbContext context)
		{
			if (context.ProductCategories.Any() == false)
			{
				context.ProductCategories.AddOrUpdate(
					pc => pc.Title,
					new ProductCategory
					{
						Title = "Vegetables",
						Alias = "vegetables",
						Description = "Fresh organic vegetables",
						Icon = "/Uploads/files/thumb/vegetables-category.png",
						CreatedBy = "system",
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now
					},
					new ProductCategory
					{
						Title = "Fruit",
						Alias = "fruit",
						Description = "Fresh organic fruit",
						Icon = "/Uploads/files/thumb/fruit-category.png",
						CreatedBy = "system",
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now
					},
					new ProductCategory
					{
						Title = "Fresh food",
						Alias = "fresh-food",
						Description = "Fresh organic food",
						Icon = "/Uploads/files/thumb/fresh-food-category.png",
						CreatedBy = "system",
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now
					},
					new ProductCategory
					{
						Title = "Other",
						Alias = "other",
						Description = "Other fresh organic product",
						Icon = "/Uploads/files/thumb/dry-food-category.png",
						CreatedBy = "system",
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now
					}
				);

				context.SaveChanges();
			}
		}
	}
}
