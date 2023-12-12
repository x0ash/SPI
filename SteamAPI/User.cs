/*
 *      -- User.cs --
 *      By: Ash Duck
 *      Date: 12/12/2023
 *      Description: This file defines a User class -- this represents Steam users using the API & XML
 */

namespace SteamAPI
{
    public class User
    {
        // Obtainable via XML
        public ulong steamID64;
        public string steamID;
        public bool vacBanned;
        public string memberSince;      // At the minute this is a string -- this is probably better as a datetime and then we can do calculations with it?

        // Obtainable via Web API
        public List<Game> gamesList;

        // Constructor
        public User()
        {
            steamID64 = 0;
            steamID = "";
            vacBanned = false;
            memberSince = "";

            gamesList = new List<Game>();
        }

        // Methods
        public override string ToString()
        {
            // -- Sorting by total playtime --

            List<Game> sortedPlaytime = gamesList.OrderBy(o => o.playtime_forever).Reverse().ToList();

            string playtimeString = "";

            for (int i = 0; i < 5; i++)
            {
                playtimeString += sortedPlaytime[i].ToString();
            }

            // -- Sorting by recent playtime --
            // This is actually obtainable via the XML page, but this unlimits our reach on it
            // Obtaining via the XML page would also require a game parser from XML results to be created.

            sortedPlaytime = gamesList.OrderBy(o => o.playtime_2weeks).Reverse().ToList();

            string recentPlaytime = "";

            for (int i = 0; i < 5; i++)
            {
                if (sortedPlaytime[i].playtime_2weeks != 0)
                {
                    recentPlaytime += sortedPlaytime[i].ToString();
                }
            }

            return $"Steam ID64: {steamID64}\n\tSteam ID: {steamID}\n\tVAC Banned: {vacBanned}\n\tMember Since: {memberSince}\n\nTop 5 Most Played Games:\n{playtimeString}\nTop 5 Recently Played Games:\n{recentPlaytime}";
        }
    }
}