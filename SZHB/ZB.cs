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
			var ret = HttpPost("https://webapi.ccash.com/common/rate", "32");//从CCASH获取更新数据包
			if (ret == "读取数据包失败")
			{
				Console.WriteLine("读取数据包错误！！！");
			}//检查是否出错
			//截取数据包的zh_CN部分-----------------
			ret = ret.Remove(0, ret.Length - 445);
			ret = ret.Remove(ret.Length - 4, 3);
			//------------------------------------
			
			Console.WriteLine(ret);//输出检查
			return;
		}
		
		//POST请求部分----------------------------------------------------------------------------
		private static CookieContainer cookie = new CookieContainer();//新建一个cookie
		private static string HttpPost(string url, string postDataStr)//http模拟请求函数(XRH)
		{
			try//尝试，如果出错就处理
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				//从head匹配相关设置
				request.Method = "POST";//请求类型为POST
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
			catch (Exception e)
			{
				//Console.WriteLine(e);
			}

			return "读取数据包失败";//如果在try中没有返回，则返回错误信息
		}
		//---------------------------------------------------------------------------------------
		
		
	}
}