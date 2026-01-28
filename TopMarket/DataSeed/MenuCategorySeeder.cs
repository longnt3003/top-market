using System;
using System.Data.Entity.Migrations;
using System.Linq;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;

namespace TopMarket.DataSeed
{
	public static class MenuCategorySeeder
	{
		public static void Seed(ApplicationDbContext context)
		{
			if (context.Categories.Any() == false)
			{
				context.Categories.AddOrUpdate(
					c => c.Title,
					new MenuCategory
					{
						Title = "Home",
						Alias = "",
						Link = "/",
						Description = "Homepage",
						IsActive = true,
						Position = 1,
						CreatedBy = "system",
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now
					},
					new MenuCategory
					{
						Title = "Products",
						Alias = "products",
						Link = "/product",
						Description = "Product list",
						IsActive = true,
						Position = 2,
						CreatedBy = "system",
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now
					},
					new MenuCategory
					{
						Title = "Blog",
						Alias = "blog",
						Link = "/blog",
						Description = "News and articles",
						IsActive = true,
						Position = 3,
						CreatedBy = "system",
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now
					},
					new MenuCategory
					{
						Title = "Contact",
						Alias = "contact",
						Link = "/contact",
						Description = "Contact page",
						IsActive = true,
						Position = 4,
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
