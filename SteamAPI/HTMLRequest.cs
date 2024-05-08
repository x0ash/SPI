using System.Net;
using System.IO.Pipes;
using System.Text;

namespace SteamAPI
{
    public class HTMLRequest
    {
        private static HttpClient httpClient = new HttpClient();
        static NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "spi", PipeDirection.InOut, PipeOptions.None);
        static StreamWriter sw;
        static StreamReader sr;

        // I'm keeping this public for now because it might be useful at some point?
        public async static Task<string> RequestHTMLPage(string url)
        {
            //
            // An async task that returns an HTML response
            // Requires: Web URL
            // Returns: Web Request (as string)
            //

            try
            {
                return await httpClient.GetStringAsync(url);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "-1";
            }
        }

        // This is mainly for bypassing the age verification check on store pages
        public async static Task<string> RequestPOST(string url, HttpContent content)
        {
            return await httpClient.PostAsync(url, content).Result.Content.ReadAsStringAsync();
        }

        /*
        // Most interaction should be performed using this, however.
        public static string GetHTMLPage(string url)
        {
            return RequestHTMLPage(url).Result;
        }
        */

        public static string GetHTMLPage(string url)
        {
            if (!pipeClient.IsConnected)
            {
                pipeClient.Connect();
            }
            sw = new StreamWriter(pipeClient);
            sr = new StreamReader(pipeClient);

            sw.AutoFlush = true;
            string page = "";

            sw.WriteLine(url);
            if (sr.Peek() > 0)
            {
                page = sr.ReadToEnd();
            }

            return page;
        }



        public static string GetPOST(string url, HttpContent content)
        {
            return RequestPOST(url, content).Result;
        }
    }
}