using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopMarket.Models.EntityFramework
{
	[Table("tb_Wishlist")]
	public class Wishlist
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[ForeignKey("Product")]
		public int ProductId { get; set; }
		public string UserName { get; set; }
		public DateTime DateCreated { get; set; }

		public virtual Product Product { get; set; }
	}
}