using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TopMarket.Models.Payments
{
	public class VnPayLibrary
	{
		public const string VERSION = "2.1.0";

		private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
		private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

		public void AddRequestData(string key, string value)
		{
			if (string.IsNullOrWhiteSpace(value) == false) _requestData[key] = value.Trim();
		}

		public void AddResponseData(string key, string value)
		{
			if (string.IsNullOrWhiteSpace(value) == false) _responseData[key] = value.Trim();
		}

		public string GetResponseData(string key)
		{
			return _responseData.TryGetValue(key, out var retValue)
				? retValue
				: string.Empty;
		}

		#region Request
		public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
		{
			var data = new StringBuilder();
			foreach (var kv in _requestData)
			{
				if (string.IsNullOrEmpty(kv.Value) == false)
				{
					data.Append(HttpUtility.UrlEncode(kv.Key) + "=" + HttpUtility.UrlEncode(kv.Value) + "&");
				}
			}

			var queryString = data.ToString();
			if (queryString.EndsWith("&"))
			{
				queryString = queryString.Substring(0, queryString.Length - 1);
			}

			var vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, queryString);
			return $"{baseUrl}?{queryString}&vnp_SecureHash={vnp_SecureHash}";
		}
		#endregion

		#region ResponseProcess
		public bool ValidateSignature(string inputHash, string secretKey)
		{
			var rspRaw = BuildResponseData();
			var myChecksum = Utils.HmacSHA512(secretKey, rspRaw);
			return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
		}

		private string BuildResponseData()
		{
			var data = new StringBuilder();

			_responseData.Remove("vnp_SecureHashType");
			_responseData.Remove("vnp_SecureHash");

			foreach (var kv in _responseData)
			{
				if (string.IsNullOrEmpty(kv.Value) == false)
				{
					data.Append(HttpUtility.UrlEncode(kv.Key) + "=" + HttpUtility.UrlEncode(kv.Value) + "&");
				}
			}

			if (data.Length > 0)
			{
				data.Remove(data.Length - 1, 1);
			}

			return data.ToString();
		}
		#endregion
	}

	public static class Utils
	{
		public static string HmacSHA512(string key, string inputData)
		{
			var hash = new StringBuilder();
			var keyBytes = Encoding.UTF8.GetBytes(key);
			var inputBytes = Encoding.UTF8.GetBytes(inputData);

			using (var hmac = new HMACSHA512(keyBytes))
			{
				var hashValue = hmac.ComputeHash(inputBytes);
				foreach (var b in hashValue)
				{
					hash.Append(b.ToString("x2"));
				}
			}

			return hash.ToString();
		}

		public static string GetIpAddress()
		{
			try
			{
				var ipAddress = HttpContext.Current?.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

				if ((string.IsNullOrEmpty(ipAddress) == false)
					&& (ipAddress.ToLower() != "unknown")
					&& (ipAddress.Length <= 45))
				{
					return ipAddress.Split(',')[0].Trim();
				}

				return HttpContext.Current?.Request.ServerVariables["REMOTE_ADDR"] ?? "Unknown";
			}
			catch (Exception ex)
			{
				return "Invalid IP: " + ex.Message;
			}
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
