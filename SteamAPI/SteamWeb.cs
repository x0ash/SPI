/*
 *      -- SteamWeb.cs --
 *      By: Ash Duck
 *      Date: 12/12/2023
 *      Description: This file retrieves information from the Steam Web API and deserializes it.
 */

using System.Text.Json;

namespace SteamAPI
{ 
    public class SteamWeb
    {
        public static string API_Key = "";
        // An API key is required to use the Steam Web API.

        private static string URL = "http://api.steampowered.com/{0}={1}&key={2}&format=json{3}";
        // This is the standard API request template string

        private static JsonElement MakeWebAPIRequest(string reqPath, ulong steamID64, string extra = "")
        {
            //
            // This just wraps the request for API calls into a method thats a little bit nicer
            //
            string jsonResult = HTMLRequest.GetHTMLPage(string.Format(URL, reqPath, steamID64, API_Key, extra));
            return JsonSerializer.Deserialize<JsonElement>(jsonResult).GetProperty("response");
        }

        public static int GetOwnedGames(User user)
        {
            //
            // A void that populates the gamesList variable for a given user
            // Requires: user.steamID64 != null
            // Returns: 0 (if successful), 1 (if unsuccessful)
            //

            const string reqPath = "IPlayerService/GetOwnedGames/v0001/?steamid";

            Output.LogProgress("Requesting user summary from Web API");
            JsonElement response = MakeWebAPIRequest(reqPath, user.GetSteamID64(), "&include_appinfo=true");    // This is the bit we care about
            Output.LogProgress("Adding games to gamesList");

            try
            {
                foreach (var game in response.GetProperty("games").EnumerateArray())                                // Games are (understandably) stored in an array as JSON objects
                {
                    JsonElement gameInfo = JsonSerializer.Deserialize<JsonElement>(game);                           // To work with them, we have to deserialize them
                    Game newGame = new Game();                                                                      // While games can be created with one line, it makes sense here to separate it as the results may not include name and/or playtime_2weeks
                    {
                        newGame.SetAppID(gameInfo.GetProperty("appid").GetUInt32());                                // App IDs are stored as integers (on our side & Valve's)
                        newGame.SetTotalPlaytime(gameInfo.GetProperty("playtime_forever").GetUInt32());             // The same applies to playtime_forever (records in minutes)

                        JsonElement title;                                                                          // name & playtime_2weeks may not be present in the response so they must be checked specially.
                        JsonElement playtime_2weeks;

                        if (gameInfo.TryGetProperty("name", out title))
                        {
                            newGame.SetTitle(title.GetString());
                            newGame.SetAppInfo(true);                                                               // If any of these extra details are present, appinfo must be set to true (this is useful for outputting without missing titles & playtime_2weeks)
                        }

                        if (gameInfo.TryGetProperty("playtime_2weeks", out playtime_2weeks))
                        {
                            newGame.SetRecentPlaytime(playtime_2weeks.GetUInt32());
                        }


                    }
                    user.AddGamesList(newGame);                                                                     // Once the new game has been created fully, add it to the user object.
                }
                return 0;
            }

            catch
            {
                Output.Error($"Cannot obtain games list (SteamID: {user.GetSteamID()})", "SteamAPI/SteamWeb:43");
                return 1;
            }
        }

        // Using this is NOT recommended compared to SteamXML.GetUserDetails for the following reasons:
        //      1. It uses a Web API request -- there's a limit to those while XML requests are unlimited
        //      2. Most users won't be interacting with a SteamID64 -- they'll have a profile URL
        //
        // However, it would be useful as a fallback option in the event that we can't get the data from a URL
        // Additionally, it can allow us to request information on multiple accounts at once.

        public static void GetUserDetails(User[] users, ulong steamIDs64)
        {
            const string reqPath = "ISteamUser/GetPlayerSummaries/v0002/?steamids";

            JsonElement response = MakeWebAPIRequest(reqPath, steamIDs64, "&include_appinfo=true");
            int count = 0;          // This really doesn't feel like the best way to handle this.

            foreach (var player in response.GetProperty("players").EnumerateArray())
            {
                JsonElement playerJson = JsonSerializer.Deserialize<JsonElement>(player);
                users[count].SetSteamID(playerJson.GetProperty("personaname").ToString());
                users[count].SetSteamID64(playerJson.GetProperty("steamid").GetUInt64());
                users[count].SetJoinDateUnix(playerJson.GetProperty("timecreated").ToString());
                count++;
            }
        }
    }
}