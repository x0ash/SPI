/*
 *      -- Program.cs --
 *      By: Ash Duck
 *      Date: 08/03/2024
 *      Description: This file defines an account crawler that will let us obtain data quickly.
 */

// Warning! This is a very hastily put together chunk of code.
// You might have to wait for it to look nice.

using SteamAPI;
using AccountCrawler;
using System.Text.Json;
using System.Diagnostics;

const int max_accs = 10;
List<string> urls_to_check = new List<string>();
List<(string, int)> output = new List<(string, int)>();

Console.WriteLine("Loading API key from Data Collection config");
// This will not have any sort of error correction sorry
// its crunch time :)
const string config_path = "../../../../DataCollection/config.json";
string _apiKey;

using (StreamReader sr = new StreamReader(config_path))
{
    string file = sr.ReadToEnd();
    Config config = JsonSerializer.Deserialize<Config>(file);
    _apiKey = config.key;
    SteamWeb.API_Key = _apiKey;
}

User[] users = new User[] { new User() };
// I know I say don't do this but in this case its fine because we're dealing with SteamID64 internally all the way through
Console.Write("Enter starting SteamID64 here >> ");
ulong start_id = Convert.ToUInt64(Console.ReadLine());
List<ulong> steamids = new List<ulong>() { start_id };
User user;

while (urls_to_check.Count() < max_accs && steamids.Count > 0)
{
    try
    {
        SteamWeb.GetUserDetails(users, steamids[0]);

        user = users[0];
        urls_to_check.Add(user.GetURL());
        try
        {
            foreach (ulong id in SteamWeb.GetFriendList(user))
            {
                if (!steamids.Contains(id))
                {
                    steamids.Add(id);
                }
            }
        }
        catch
        {
            SteamAPI.Output.Error("Friends list is private!");
        }
    }
    catch
    {
        SteamAPI.Output.Error("Can't obtain some part of the information");
    }
    steamids.RemoveAt(0);
}

Console.WriteLine("done");
foreach (string url in urls_to_check)
{
    var ps = new ProcessStartInfo(url)
    {
        UseShellExecute = true,
        Verb = "open"
    };
    Process.Start(ps);
    Console.Write("Real or nah? (0/1) >> ");
    Console.ReadLine();
}