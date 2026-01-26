using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using TopMarket.Models.EntityFramework;

namespace TopMarket.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; }
		public string Phone { get; set; }
		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
		{
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			return userIdentity;
		}
	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
		{
		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Blog> Blogs { get; set; }
		public DbSet<SystemSetting> SystemSettings { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<Contact> Contacts { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<ReviewProduct> ReviewProducts { get; set; }
		public DbSet<Wishlist> Wishlists { get; set; }

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
	}
}