using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace TopMarket.Common
{
	public class Common
	{
		private static string password = ConfigurationManager.AppSettings["PasswordEmail"];
		private static string Email = ConfigurationManager.AppSettings["Email"];

		public static bool SendMail(
			string name,
			string subject,
			string content,
			string toMail)
		{
			bool rs = false;
			try
			{
				MailMessage message = new MailMessage();
				var smtp = new SmtpClient();
				{
					// Host name
					smtp.Host = "smtp.gmail.com";
					// Port number
					smtp.Port = 587;
					// Whether your smtp server requires SSL
					smtp.EnableSsl = true;
					smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
					smtp.UseDefaultCredentials = false;
					smtp.Credentials = new NetworkCredential()
					{
						UserName = Email,
						Password = password
					};
				}
				MailAddress fromAddress = new MailAddress(Email, name);
				message.From = fromAddress;
				message.To.Add(toMail);
				message.Subject = subject;
				message.IsBodyHtml = true;
				message.Body = content;
				smtp.Send(message);
				rs = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				rs = false;
			}

			return rs;
		}

		public static string FormatNumber(object value, int NumberAfterComma = 2)
		{
			bool isNumber = IsNumeric(value);
			decimal GT = 0;
			if (isNumber) GT = Convert.ToDecimal(value);

			string str = string.Empty;
			string decimalNumber = string.Empty;
			for (int i = 0; i < NumberAfterComma; i++)
			{
				decimalNumber += "#";
			}

			if (decimalNumber.Length > 0) decimalNumber = "." + decimalNumber;

			string snumformat = string.Format("0:#,##0{0}", decimalNumber);
			str = GT.ToString(snumformat);
			return str;
		}

		private static bool IsNumeric(object value)
		{
			return value is sbyte
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
		}

		public static string HtmlRate(int rate)
		{
			var sb = new System.Text.StringBuilder();
			for (int i = 0; i < rate; i++)
			{
				sb.Append(@"<li><i class=""fa fa-star"" aria-hidden=""true""></i></li>");
			}

			for (int i = rate; i < 5; i++)
			{
				sb.Append(@"<li><i class=""fa fa-star-o"" aria-hidden=""true""></i></li>");
			}

			return sb.ToString();
		}
	}
}
