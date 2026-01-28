using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopMarket.Models.EntityFramework
{
	[Table("tb_ProductImage")]
	public class ProductImage
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string Image { get; set; }
		public bool IsDefault { get; set; }

		public virtual Product Product { get; set; }
	}
}