using System.Collections.Generic;
using System.Text;

namespace TopMarket.Models.Common
{
	public static class Filter
	{
		private static readonly Dictionary<char, char> VietnameseMap = CreateVietnameseMap();

		private static Dictionary<char, char> CreateVietnameseMap()
		{
			var map = new Dictionary<char, char>();

			// a ă â
			AddMapping(map, "áàạảãâấầậẩẫăắằặẳẵ", 'a');
			AddMapping(map, "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ", 'a');

			// e ê
			AddMapping(map, "éèẹẻẽêếềệểễ", 'e');
			AddMapping(map, "ÉÈẸẺẼÊẾỀỆỂỄ", 'e');

			// i
			AddMapping(map, "íìịỉĩ", 'i');
			AddMapping(map, "ÍÌỊỈĨ", 'i');

			// o ô ơ
			AddMapping(map, "óòọỏõôốồộổỗơớờợởỡ", 'o');
			AddMapping(map, "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", 'o');

			// u ư
			AddMapping(map, "úùụủũưứừựửữ", 'u');
			AddMapping(map, "ÚÙỤỦŨƯỨỪỰỬỮ", 'u');

			// đ
			map['đ'] = 'd';
			map['Đ'] = 'd';

			// y
			AddMapping(map, "ýỳỵỷỹ", 'y');
			AddMapping(map, "ÝỲỴỶỸ", 'y');

			return map;
		}

		private static void AddMapping(Dictionary<char, char> map, string source, char target)
		{
			foreach (char c in source)
			{
				map[c] = target;
			}
		}

		public static string ToSlug(string input, bool replaceSpaceWithDash = true)
		{
			if (string.IsNullOrWhiteSpace(input)) return string.Empty;

			var sb = new StringBuilder(input.Length);
			foreach (char c in input)
			{
				var processed = c;
				if (VietnameseMap.TryGetValue(c, out char replacement))
				{
					processed = replacement;
				}

				if (char.IsLetterOrDigit(processed)
					|| processed == ' '
					|| processed == '-')
				{
					sb.Append(processed);
				}
				else
				{
					sb.Append(' ');
				}
			}

			var result = sb.ToString().Trim().Replace("  ", " ");
			if (replaceSpaceWithDash)
			{
				result = result.Replace(' ', '-');
			}

			while (result.Contains("--"))
			{
				result = result.Replace("--", "-");
			}

			result = result.Trim('-');

			return result.ToLower();
		}

		public static string FilterChar(string str) { return ToSlug(str, replaceSpaceWithDash: true); }
		public static string PunctuationConverter(string str) { return ToSlug(str, replaceSpaceWithDash: false); }
	}
}