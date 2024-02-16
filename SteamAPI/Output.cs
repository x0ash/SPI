/*
 *      -- Output.cs --
 *      By: Ash Duck
 *      Date: 05/02/2024
 *      Description: This file provides a nice output for some information like loading.
 */

namespace SteamAPI
{
	public class Output
	{
		public static void LogProgress(string progress)
		{
			//
			// This method outputs to the console a percentage (amateurly calculated) for how 'ready' the data is.
			// If verbose, it will instead output how many operations it's on and how long it's taken.
			// Verbosity is hardcoded (bool: verbose)
			// Requires: verbose = true/false, progress string
			//

			if (verbose)
			{
				current = ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds();
				Console.Write(" (Took {2}ms)\n{0} -- {1}", _LoadingProgress, progress, current - last);
				last = current;
			}

			else
			{
				// This is broken so I'm just going to disable it for now.
				//Console.WriteLine("Loading: {0}%", Math.Round((float)_LoadingProgress * 100 / operationCount));
			}

			_LoadingProgress++;
		}

        public static void Error(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Error] {errorMessage}");
			Console.ForegroundColor = ConsoleColor.White;
        }

        private static int _LoadingProgress;
		private static long current;
		private static long last = ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds();
		private static int operationCount = 28;             // This is obtained via testing on just my personal account, probably not too accurate
		private static bool verbose = false;

    }
}

