using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopMarket.Models.EntityFramework
{
	[Table("tb_SystemSetting")]
	public class SystemSetting
	{
		[Key]
		[StringLength(50)]
		public string SettingKey { get; set; }
		[StringLength(3000)]
		public string SettingValue { get; set; }
		[StringLength(3000)]

		public string SettingDescription { get; set; }
	}
}