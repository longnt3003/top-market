using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace TopMarket.Models.EntityFramework
{
	[Table("tb_Blog")]
	public class Blog : CommonAbstract
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required(ErrorMessage = "Blog Title cannot be blank!")]
		[StringLength(150)]
		public string Title { get; set; }
		[StringLength(150)]
		public string Alias { get; set; }
		[AllowHtml]
		public string Description { get; set; }
		[AllowHtml]
		public string Detail { get; set; }
		[StringLength(250)]
		public string Image { get; set; }
		public bool IsActive { get; set; }
	}
}
