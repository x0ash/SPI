namespace SteamAPI
{
    public class HTMLRequest
    {
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
                HttpClient httpClient = new HttpClient();
                return await httpClient.GetStringAsync(url);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "-1";
            }
        }

        // Most interaction should be performed using this, however.
        public static string GetHTMLPage(string url)
        {
            return RequestHTMLPage(url).Result;
        }
    }
}