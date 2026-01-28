using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using TopMarket.Models;
using TopMarket.Models.EntityFramework;

namespace TopMarket.DataSeed
{
	public static class ProductSeeder
	{
		public static void Seed(ApplicationDbContext context)
		{
			if (context.Products.Any() == false)
			{
				context.Products.AddOrUpdate(
					p => p.Title,
					new Product
					{
						Title = "Spinach",
						Alias = "spinach",
						ProductCode = "100",
						Description = "Fresh organic spinach rich in vitamins and minerals.",
						Detail = @"<p>Freshly harvested organic spinach, grown without chemical pesticides. Ideal for salads, soups, and healthy meals.</p>",
						Image = "/Uploads/files/Product/spinach.jpg",
						Price = 20000M,
						PromotionPrice = 15000M,
						Quantity = 100,
						IsHome = false,
						IsSale = true,
						IsFeature = true,
						IsHot = true,
						IsActive = true,
						ProductCategoryId = 1,
						CreatedBy = "system",
						DateCreated = DateTime.Now,
						DateModified = DateTime.Now,
						ProductImages = new List<ProductImage>
					{
						new ProductImage { Image = "/Uploads/files/Product/spinach.jpg", IsDefault = true }
					}
					},
				new Product
				{
					Title = "Carrot",
					Alias = "carrot",
					ProductCode = "101",
					Description = "Crisp and sweet carrots for everyday cooking.",
					Detail = @"<p>Natural fresh carrots with a crunchy texture and mild sweetness. Perfect for soups, stir-fry dishes, or juicing.</p>",
					Image = "/Uploads/files/Product/carrot.jpg",
					Price = 15000M,
					PromotionPrice = 13000M,
					Quantity = 200,
					IsHome = true,
					IsSale = true,
					IsFeature = false,
					IsHot = false,
					IsActive = true,
					ProductCategoryId = 1,
					CreatedBy = "system",
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now,
					ProductImages = new List<ProductImage>
					{
						new ProductImage { Image = "/Uploads/files/Product/carrot.jpg", IsDefault = true }
					}
				},
				new Product
				{
					Title = "Banana",
					Alias = "banana",
					ProductCode = "102",
					Description = "Naturally sweet bananas, rich in potassium.",
					Detail = @"<p>Ripe bananas with a smooth texture and natural sweetness. A great choice for breakfast, smoothies, or snacks.</p>",
					Image = "/Uploads/files/Product/banana.jpg",
					Price = 18000M,
					PromotionPrice = null,
					Quantity = 150,
					IsHome = true,
					IsSale = false,
					IsFeature = true,
					IsHot = false,
					IsActive = true,
					ProductCategoryId = 2,
					CreatedBy = "system",
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now,
					ProductImages = new List<ProductImage>
					{
						new ProductImage { Image = "/Uploads/files/Product/banana.jpg", IsDefault = true }
					}
				},
				new Product
				{
					Title = "Red Apple",
					Alias = "red-apple",
					ProductCode = "103",
					Description = "Juicy red apples with a refreshing taste.",
					Detail = @"<p>Fresh red apples harvested at peak ripeness. Suitable for eating raw, making desserts, or fresh juice.</p>",
					Image = "/Uploads/files/Product/red-apple.jpg",
					Price = 25000M,
					PromotionPrice = null,
					Quantity = 120,
					IsHome = true,
					IsSale = false,
					IsFeature = false,
					IsHot = true,
					IsActive = true,
					ProductCategoryId = 2,
					CreatedBy = "system",
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now,
					ProductImages = new List<ProductImage>
					{
						new ProductImage { Image = "/Uploads/files/Product/red-apple.jpg", IsDefault = true }
					}
				},
				new Product
				{
					Title = "Chicken Breast",
					Alias = "chicken-breast",
					ProductCode = "104",
					Description = "High-quality fresh chicken breast, lean and nutritious.",
					Detail = @"<p>Carefully selected fresh chicken breast, rich in protein and low in fat. Ideal for healthy diets and daily meals.</p>",
					Image = "/Uploads/files/Product/chicken-breast.jpg",
					Price = 60000M,
					PromotionPrice = 50000M,
					Quantity = 80,
					IsHome = true,
					IsSale = true,
					IsFeature = true,
					IsHot = false,
					IsActive = true,
					ProductCategoryId = 3,
					CreatedBy = "system",
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now,
					ProductImages = new List<ProductImage>
					{
						new ProductImage { Image = "/Uploads/files/Product/chicken-breast.jpg", IsDefault = true }
					}
				},
				new Product
				{
					Title = "Salmon Fillet",
					Alias = "salmon-fillet",
					ProductCode = "105",
					Description = "Premium fresh salmon fillet rich in omega-3.",
					Detail = @"<p>Fresh salmon fillet with tender texture and natural flavor. Perfect for grilling, baking, or pan-searing.</p>",
					Image = "/Uploads/files/Product/salmon-fillet.jpg",
					Price = 120000M,
					PromotionPrice = null,
					Quantity = 60,
					IsHome = true,
					IsSale = true,
					IsFeature = true,
					IsHot = true,
					IsActive = true,
					ProductCategoryId = 3,
					CreatedBy = "system",
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now,
					ProductImages = new List<ProductImage>
					{
						new ProductImage { Image = "/Uploads/files/Product/salmon-fillet.jpg", IsDefault = true }
					}
				},
				new Product
				{
					Title = "Brown Rice",
					Alias = "brown-rice",
					ProductCode = "106",
					Description = "Healthy organic brown rice with natural nutrients.",
					Detail = @"<p>Organic brown rice with high fiber content, suitable for a balanced and healthy diet.</p>",
					Image = "/Uploads/files/Product/brown-rice.jpg",
					Price = 40000M,
					PromotionPrice = null,
					Quantity = 90,
					IsHome = false,
					IsSale = false,
					IsFeature = false,
					IsHot = false,
					IsActive = true,
					ProductCategoryId = 4,
					CreatedBy = "system",
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now,
					ProductImages = new List<ProductImage>
					{
						new ProductImage { Image = "/Uploads/files/Product/brown-rice.jpg", IsDefault = true }
					}
				},
				new Product
				{
					Title = "Natural Honey",
					Alias = "natural-honey",
					ProductCode = "107",
					Description = "Pure natural honey without additives.",
					Detail = @"<p>100% natural honey harvested from trusted farms. Ideal for daily use, drinks, or desserts.</p>",
					Image = "/Uploads/files/Product/honey.jpg",
					Price = 70000M,
					PromotionPrice = null,
					Quantity = 50,
					IsHome = false,
					IsSale = true,
					IsFeature = false,
					IsHot = true,
					IsActive = true,
					ProductCategoryId = 4,
					CreatedBy = "system",
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now,
					ProductImages = new List<ProductImage>
					{
						new ProductImage { Image = "/Uploads/files/Product/honey.jpg", IsDefault = true }
					}
				}
				);

				context.SaveChanges();
			}
		}
	}
}
