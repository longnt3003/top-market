using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TopMarket.Models.Payments
{
	public class VnPayLibrary
	{
		public const string VERSION = "2.1.0";

		private readonly SortedList<string, string> requestData = new SortedList<string, string>(new VnPayCompare());
		private readonly SortedList<string, string> responseData = new SortedList<string, string>(new VnPayCompare());

		public void AddRequestData(string key, string value)
		{
			if (!string.IsNullOrEmpty(value)) requestData.Add(key, value);
		}

		public void AddResponseData(string key, string value)
		{
			if (!string.IsNullOrEmpty(value)) responseData.Add(key, value);
		}

		public string GetResponseData(string key)
		{
			return responseData.TryGetValue(key, out var retValue)
				? retValue
				: string.Empty;
		}

		#region Request
		public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
		{
			var data = new StringBuilder();
			foreach (var kv in requestData)
			{
				if (!string.IsNullOrEmpty(kv.Value))
				{
					data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
				}
			}

			var queryString = data.ToString();
			baseUrl += "?" + queryString;

			var signData = queryString;
			if (signData.Length > 0) signData = signData.Remove(data.Length - 1, 1);

			var vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);
			baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

			return baseUrl;
		}
		#endregion

		#region Response process
		public bool ValidateSignature(string inputHash, string secretKey)
		{
			var rspRaw = GetResponseData();
			var myChecksum = Utils.HmacSHA512(secretKey, rspRaw);
			return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
		}

		private string GetResponseData()
		{
			var data = new StringBuilder();
			if (responseData.ContainsKey("vnp_SecureHashType")) responseData.Remove("vnp_SecureHashType");
			if (responseData.ContainsKey("vnp_SecureHash")) responseData.Remove("vnp_SecureHash");

			foreach (var kv in responseData)
			{
				if (!string.IsNullOrEmpty(kv.Value))
				{
					data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
				}
			}

			// remove last '&'
			if (data.Length > 0) data.Remove(data.Length - 1, 1);

			return data.ToString();
		}
		#endregion
	}

	public class Utils
	{
		public static string HmacSHA512(string key, string inputData)
		{
			var hash = new StringBuilder();
			var keyBytes = Encoding.UTF8.GetBytes(key);
			var inputBytes = Encoding.UTF8.GetBytes(inputData);

			using (var hmac = new HMACSHA512(keyBytes))
			{
				var hashValue = hmac.ComputeHash(inputBytes);
				foreach (var theByte in hashValue)
				{
					hash.Append(theByte.ToString("x2"));
				}
			}

			return hash.ToString();
		}

		public static string GetIpAddress()
		{
			string ipAddress;
			try
			{
				ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				if (string.IsNullOrEmpty(ipAddress)
					|| ipAddress.ToLower() == "unknown"
					|| ipAddress.Length > 45)
				{
					ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
				}
			}
			catch (Exception ex)
			{
				ipAddress = "Invalid IP:" + ex.Message;
			}

			return ipAddress;
		}
	}

	public class VnPayCompare : IComparer<string>
	{
		public int Compare(string x, string y)
		{
			if (x == y) return 0;
			if (x == null) return -1;
			if (y == null) return 1;

			var vnpCompare = CompareInfo.GetCompareInfo("en-US");
			return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
		}
	}
}
