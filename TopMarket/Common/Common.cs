using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace TopMarket.Common
{
	public class Common
	{
		private static readonly string password = ConfigurationManager.AppSettings["PasswordEmail"];
		private static readonly string email = ConfigurationManager.AppSettings["Email"];

		public static bool SendMail(string name, string subject, string content, string toMail)
		{
			try
			{
				var smtp = new SmtpClient("smtp.gmail.com", 587)
				{
					EnableSsl = true,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					UseDefaultCredentials = false,
					Credentials = new NetworkCredential(email, password)
				};

				var message = new MailMessage(new MailAddress(email, name), new MailAddress(toMail))
				{
					Subject = subject,
					Body = content,
					IsBodyHtml = true
				};

				smtp.Send(message);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public static string FormatNumber(object value, int numberAfterComma = 2)
		{
			if (!IsNumeric(value)) return string.Empty;

			var gt = Convert.ToDecimal(value);
			var decimalNumber = new string('#', numberAfterComma);
			if (decimalNumber.Length > 0) decimalNumber = "." + decimalNumber;

			var format = $"0:#,##0{decimalNumber}";
			return string.Format("{0:" + format + "}", gt);
		}

		private static bool IsNumeric(object value)
		{
			var result = value is sbyte
				|| value is byte
				|| value is short
				|| value is ushort
				|| value is int
				|| value is uint
				|| value is long
				|| value is ulong
				|| value is float
				|| value is double
				|| value is decimal;

			return result;
		}

		public static string HtmlRate(int rate)
		{
			if ((rate < 1)
				|| (rate > 5)) return string.Empty;

			var result = string.Join("\n", Enumerable.Range(1, 5)
				.Select(i => $"<li><i class=\"fa {(i <= rate ? "fa-star" : "fa-star-o")}\" aria-hidden=\"true\"></i></li>"));

			return result;
		}
	}
}
