using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopMarket.Models.EntityFramework
{
	[Table("tb_Category")]
	public class Category : CommonAbstract
	{
		public Category() 
		{ 
			this.Blog = new HashSet<Blog>();
		}

		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required(ErrorMessage = "Category Name cannot be blank!")]
		[StringLength(150)]
		public string Title { get; set; } 
		public string Alias { get; set; }
		public string Link { get; set; }
		public string Description { get; set; }
		public bool IsActive { get; set; }
		public int Position { get; set; }

		public ICollection<Blog> Blog { get; set; }
	}
}