using System;
using System.Data.Entity.Migrations;
using System.Linq;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;

namespace TopMarket.DataSeed
{
	public static class BlogSeeder
	{
		public static void Seed(ApplicationDbContext context)
		{
			if (!context.Blogs.Any())
			{
				context.Blogs.AddOrUpdate(
					b => b.Title,
					new Blog
					{
						Title = "What Is Organic Food and Why It Matters?",
						Alias = "what-is-organic-food-and-why-it-matters",
						Description = "Organic food is becoming more popular thanks to its safety, natural taste, and health benefits.",
						Detail = @"<p>Organic food is produced using natural farming methods without synthetic pesticides, chemical fertilizers, or growth stimulants.</p>
								   <p>This approach helps protect the environment, maintain soil quality, and reduce harmful chemical residues in food.</p>
								   <p>At TopMarket, we carefully select organic products to ensure freshness, safety, and high nutritional value for customers.</p>",
						Image = "/Uploads/files/Blog/blog-thumb-1.png",
						IsActive = true,
						CreatedBy = "system",
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now
					},
					new Blog
					{
						Title = "5 Benefits of Eating Organic Food Every Day",
						Alias = "5-benefits-of-eating-organic-food-every-day",
						Description = "Eating organic food daily can improve your health while supporting a more sustainable lifestyle.",
						Detail = @"<p>Organic food offers many advantages for both your body and the environment.</p>
								   <ul>
									   <li>Lower exposure to harmful chemicals</li>
									   <li>Higher nutritional value</li>
									   <li>Better digestion</li>
									   <li>Environmentally friendly farming</li>
									   <li>Safer for children and the elderly</li>
								   </ul>
								   <p><br />Choosing organic food is a long-term investment in your health and well-being.</p>",
						Image = "/Uploads/files/Blog/blog-thumb-2.png",
						IsActive = true,
						CreatedBy = "system",
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now
					},
					new Blog
					{
						Title = "How to Store Vegetables Properly to Keep Them Fresh",
						Alias = "how-to-store-vegetables-properly-to-keep-them-fresh",
						Description = "Proper vegetable storage helps maintain freshness, nutrition, and reduces food waste.",
						Detail = @"<p>Many vegetables lose freshness quickly if they are not stored correctly after purchase.</p>
								   <p>Here are some simple storage tips:</p>
								   <ul>
									   <li>Do not wash vegetables before refrigerating</li>
									   <li>Use breathable bags or paper towels</li>
									   <li>Separate vegetables by type</li>
									   <li>Store at the correct refrigerator temperature</li>
								   </ul>
								   <p><br />Following these tips will help your vegetables stay fresh longer and taste better.</p>",
						Image = "/Uploads/files/Blog/blog-thumb-3.png",
						IsActive = true,
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
