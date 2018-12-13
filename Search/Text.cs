using System;

namespace Search
{
	static class Text
	{
		public static void Web(string keyword, short num)
		{
			string[] url = 
				{
					"https://www.baidu.com/s?ie=UTF-8&wd=", //百度
					"http://dict.cnki.net/dict_result.aspx?searchword=", //术语翻译
					"http://define.cnki.net/WebForms/WebDefines.aspx?searchword=" //学术定义
				};
			System.Diagnostics.Process.Start(url[num] + keyword);
			return;
		}

		public static void Main(string[] args)
		{
			Console.WriteLine("请输入要搜索的内容：");
			string KW = Console.ReadLine();
			Web(KW, 0);
			Web(KW, 1);
			return;
		}
	}
}