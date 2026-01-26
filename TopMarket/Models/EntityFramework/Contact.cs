using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopMarket.Models.EntityFramework
{
	[Table("tb_Contact")]
	public class Contact : CommonAbstract
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required(ErrorMessage ="Name cannot be blank!")]
		[StringLength(150,ErrorMessage = "Cannot exceed 150 characters")]
		public string Name { get; set; }
		[EmailAddress]
		[StringLength(150, ErrorMessage = "Cannot exceed 150 characters")]
		public string Email { get; set; }
		public string Message { get; set; }
		public bool IsRead { get; set; }
	}
}