using System;
namespace IO
{
	public static class Input
	{
		public static string Read()
		{
			string msg = Console.ReadLine();
			if (msg == null)
			{
				msg = "";
			}
			return msg;
		}
	}
}

