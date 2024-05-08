using System;
namespace IO
{
	public static class Output
	{
		public static void Print(string message)
		{
			Console.WriteLine(message);
		}

        public static void Error(string errorMessage, string origin = "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{(origin == "" ? "" : $"[{origin}] ")}[Error] {errorMessage}");             // I'm so sorry
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

