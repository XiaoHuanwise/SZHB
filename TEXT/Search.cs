namespace TEXT
{
	class Search
	{
		public static void Web(string keyword)
		{
			System.Diagnostics.Process.Start("https://www.baidu.com/s?ie=UTF-8&wd="+keyword);
			return;
		}

		public static void Main(string[] args)
		{
			Web("比利王");
			return;
		}
	}
}