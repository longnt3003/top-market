namespace TopMarket.Models.Common
{
	public class Filter
	{
		private static readonly string[] VietnameseChar = new string[]
		{
			"aAeEoOuUiIdDyY",
			"áàạảãâấầậẩẫăắằặẳẵ",
			"ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
			"éèẹẻẽêếềệểễ",
			"ÉÈẸẺẼÊẾỀỆỂỄ",
			"óòọỏõôốồộổỗơớờợởỡ",
			"ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
			"úùụủũưứừựửữ",
			"ÚÙỤỦŨƯỨỪỰỬỮ",
			"íìịỉĩ",
			"ÍÌỊỈĨ",
			"đ",
			"Đ",
			"ýỳỵỷỹ",
			"ÝỲỴỶỸ"
		};

		private static readonly char[] Specials =
		{
			'?','&',',',':','!','\'','"','%','#','$','*','`','~','@','^','.','/','>','<','[',']',';','+'
		};

		public static string FilterChar(string str)
		{
			str = str.Trim();
			for (int i = 1; i < VietnameseChar.Length; i++)
			{
				for (int j = 0; j < VietnameseChar[i].Length; j++)
				{
					str = str.Replace(VietnameseChar[i][j], VietnameseChar[0][i - 1]);
				}
			}

			str = str.Replace(" ", "-").Replace("--", "-");
			foreach (var c in Specials)
			{
				str = str.Replace(c.ToString(), string.Empty);
			}

			return str.ToLower();
		}

		public static string PunctuationConverter(string str)
		{
			str = str.Trim();
			for (int i = 1; i < VietnameseChar.Length; i++)
			{
				for (int j = 0; j < VietnameseChar[i].Length; j++)
				{
					str = str.Replace(VietnameseChar[i][j], VietnameseChar[0][i - 1]);
				}
			}

			str = str.Replace("--", "-");
			foreach (var c in Specials)
			{
				str = str.Replace(c.ToString(), string.Empty);
			}

			return str.ToLower();
		}
	}
}
