using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SZHB
{
	/*internal*/ static class Zb
	{
		public static void Main(string[] args)
		{
			string ret = HttpPost("https://webapi.ccash.com/common/rate", "32");
			//string[] sArray=ret.Split(new char[2] {'z','}'});
			//foreach (string i in sArray) Console.WriteLine(i.ToString() + "<br>");
			ret = ret.Remove(0, ret.Length - 445);
			ret = ret.Remove(ret.Length - 4, 3);
			Console.WriteLine(ret);
		}
		
		private static CookieContainer cookie = new CookieContainer();
		
		private static string HttpPost(string url, string postDataStr)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "POST";
			request.ContentType = "application/json;charset=UTF-8";
			request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
			request.CookieContainer = cookie;
			Stream myRequestStream = request.GetRequestStream();
			StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
			myStreamWriter.Write(postDataStr);
			myStreamWriter.Close();
 
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
 
			response.Cookies = cookie.GetCookies(response.ResponseUri);
			Stream myResponseStream = response.GetResponseStream();
			StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
			string retString = myStreamReader.ReadToEnd();
			myStreamReader.Close();
			myResponseStream.Close();
 
			return retString;
		}
	}
}