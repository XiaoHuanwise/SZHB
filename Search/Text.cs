using System;

namespace Search
{
	static class Text
	{
		public static void Web(string keyword)
		{
			System.Diagnostics.Process.Start("https://www.baidu.com/s?ie=UTF-8&wd="+keyword);
			return;
		}

		public static void Main(string[] args)
		{
			Console.WriteLine("请输入要搜索的内容：");
			Web(Console.ReadLine());
			return;
		}
	}
}