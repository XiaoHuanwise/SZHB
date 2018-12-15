using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SZHB
{
	/*internal*/ static class Zb
	{
		public static void Main(string[] args)
		{
			string[] keyword = {"CA/CC(0)：", "XC/CA(1)：", "CWV/XC(2)：", "CWV/USDT(3)：", "CC/USDT(4)："};
			
			//HEAD0:
			//获取用户自定义数据
			for (int i = 0; i < 5; i++)
			{
				Console.WriteLine(keyword[i]);
			}//引导信息输出
			Console.WriteLine("请输入循环开始的地方(数子编号)：");
			int s = Convert.ToInt32(Console.ReadLine());
			Console.Write("请输入开始的货币数：");
			double r = Convert.ToDouble(Console.ReadLine());
			Console.WriteLine("请问是买还是卖？（卖请输入0，否则输入非0数）");
			bool fg = "0" == Console.ReadLine() ? false : true;
			
			//HEAD1:
			while (true)
			{
				var ret = HttpPost("https://webapi.ccash.com/common/rate", "32");//从CCASH获取更新数据包
				if (ret == "读取数据包失败")
				{
					//Console.WriteLine("读取数据包错误！！！");
					//goto Print;
					//goto HEAD1;
					continue;
				}//检查是否出错(过快或超时)
				//截取数据包的zh_CN部分-----------------
				ret = ret.Remove(0, ret.Length - 445);
				ret = ret.Remove(ret.Length - 4, 3);
				//------------------------------------
			
				//Print:
				//Console.WriteLine(ret);//输出检查
				double[] crate = Calculation(Division(ret));//计算
			
				//输出比例
				for (int i = 0; i < 5; i++)
				{
					Console.WriteLine(keyword[i] + crate[i]);
				}
			
				//开始计算
				for(short i=0; i<5; i++)
				{
					if (fg)//判断卖买
					{
						if(s == 3)
						{
							r *= crate[s];
						}
						else
						{
							r /= crate[s];
						}
						if(s == 4)
						{
							s = 0;
						}
						else
						{
							s++;
						}
					}
					else
					{
						if(s == 3)
						{
							r /= crate[s];
						}
						else
						{
							r *= crate[s];
						}
						if(s == 0)
						{
							s = 4;
						}
						else
						{
							s--;
						}
					}
				}
				r = Math.Round(r * 0.99, 6);
			
				//输出结果
				Console.WriteLine("循环后的货币数量为：{0}", r);
				Console.WriteLine("------------------------------");
				//Delay(35);
				//Thread.Sleep(35000);
			}
			//goto HEAD1;
			
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
				request.AllowAutoRedirect = true;
				request.Timeout = 30000;
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
		
		//分割数据部分--------------------------------------
		private static string[] Division(string rate)
		{
			string[] rerate = new string[5], keyword = {"\"CC\":", "\"CA\":", "\"CWV\":", "\"XC\":", "\"USDT\":"};
			
			for (int i = 0; i < 5; i++)
			{
				rerate[i] = rate.Substring(rate.IndexOf(keyword[i]) + keyword[i].Length, 18);
				//Console.WriteLine(keyword[i] + rerate[i]);
			}
			
			return rerate;
		}
		//------------------------------------------------
		
		//计算汇率部分-------------------------------------------
		private static double[] Calculation(string[] rate)
		{
			double[] crate = new double[5];
			int[,] division = {{1, 0}, {3, 1}, {2, 3}, {2, 4}, {0, 4}};
			for (int i = 0; i < 5; i++)
			{
				//相除，并保留6位小数
				crate[i] = Math.Round(Convert.ToDouble(rate[division[i, 0]]) / Convert.ToDouble(rate[division[i, 1]]), 6);
			}
			return crate;
		}
		//-----------------------------------------------------
		
		//延时函数-----------------------------------
		public static void Delay(int delayTime)
		{
			DateTime now = DateTime.Now;
			int s;
			do
			{
				TimeSpan spand = DateTime.Now - now; 
				s = spand.Seconds;
			}
			while (s < delayTime);
			return;
		}
		//------------------------------------------
	}
}