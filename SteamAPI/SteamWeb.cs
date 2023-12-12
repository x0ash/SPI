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

        private static async Task<string> RequestOwnedGames(ulong steamid, bool appinfo=false)
        {
            //
            // An async task that returns the JSON response from the Steam Web API
            // Requires: SteamID64, API key
            // Returns: JSON response (as string)
            //

            string uri = "http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=" + API_Key + "&steamid=" + steamid.ToString() + "&format=json";

            // appinfo=true provides extra details (such as the title of the game and the playtime in the last two weeks) at the cost of being slower.
            if (appinfo)
            {
                uri += "&include_appinfo=true";
            }

            try
            {
                HttpClient httpClient = new HttpClient();
                return await httpClient.GetStringAsync(uri);

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "-1";
            }
        }

        public static void GetOwnedGames(User user)
        {
            //
            // A void that populates the gamesList variable for a given user
            // Requires: user.steamID64 != null
            //

            string result = RequestOwnedGames(user.steamID64,true).Result;      // This is a JSON string

            JsonElement response = JsonSerializer.Deserialize<JsonElement>(result).GetProperty("response");     // This is the bit we care about
            foreach (var game in response.GetProperty("games").EnumerateArray())                                // Games are (understandably) stored in an array as JSON objects
            {
                JsonElement gameInfo = JsonSerializer.Deserialize<JsonElement>(game);                           // To work with them, we have to deserialize them
                Game newGame = new Game();                                                                      // While games can be created with one line, it makes sense here to separate it as the results may not include name and/or playtime_2weeks
                {
                    newGame.appid = gameInfo.GetProperty("appid").GetUInt32();                                  // App IDs are stored as integers (on our side & Valve's)
                    newGame.playtime_forever = gameInfo.GetProperty("playtime_forever").GetUInt32();            // The same applies to playtime_forever (records in minutes)

                    JsonElement title;                                                                          // name & playtime_2weeks may not be present in the response so they must be checked specially.
                    JsonElement playtime_2weeks;

                    if (gameInfo.TryGetProperty("name", out title))
                    {
                        newGame.title = title.GetString();
                        newGame.appinfo = true;                                                                 // If any of these extra details are present, appinfo must be set to true (this is useful for outputting without missing titles & playtime_2weeks)
                    }

                    if (gameInfo.TryGetProperty("playtime_2weeks", out playtime_2weeks))
                    {
                        newGame.playtime_2weeks = playtime_2weeks.GetUInt32();
                        newGame.appinfo = true;
                    }


                }
                user.gamesList.Add(newGame);                                                                    // Once the new game has been created fully, add it to the user object.
            }
        }
    }
}