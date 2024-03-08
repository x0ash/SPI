/*
 *      -- User.cs --
 *      By: Ash Duck
 *      Date: 12/12/2023
 *      Description: This file defines a User class -- this represents Steam users using the API & XML
 */

using System.ComponentModel;

namespace SteamAPI
{
    public class User
    {
        // Obtainable via XML
        ulong steamID64;
        string steamID;
        bool vacBanned;
        DateTime memberSince;
        int userLevel;
        
        // Obtainable via Web API
        List<Game> gamesList;

        public int IsSmurf; // Might not really belong here

        // Constructor
        public User()
        {
            steamID64 = 0;
            steamID = "";
            vacBanned = false;
            memberSince = new DateTime();
            userLevel = -1;

            IsSmurf = 0;

            gamesList = new List<Game>();
        }

        // -- Getter/Setters --
        #region GettersSetters

        // SteamID64
        public ulong GetSteamID64()
        {
            return steamID64;
        }

        public void SetSteamID64(ulong _steamID64)
        {
            steamID64 = _steamID64;
        }

        // SteamID
        public string GetSteamID()
        {
            return steamID;
        }

        public void SetSteamID(string _steamID)
        {
            steamID = _steamID;
        }

        // VAC Status
        public bool GetVacStatus()
        {
            return vacBanned;
        }

        public void SetVacStatus(bool _vacBanned)
        {
            vacBanned = _vacBanned;
        }

        // Join Date
        public DateTime GetJoinDate()
        {
            return memberSince;
        }

        public void SetJoinDate(DateTime _memberSince)
        {
            memberSince = _memberSince;
        }

        public void SetJoinDate(string _memberSince)
        {
            memberSince = DateTime.Parse(_memberSince);             // XML pages return as string, so we can parse
        }

        // Obtaining the user details via Web API gives date created as a unix time, so this has to be supported.
        public void SetJoinDateUnix(string timecreated)
        {
            memberSince = DateTimeOffset.FromUnixTimeSeconds(long.Parse(timecreated)).DateTime;
        }

        // User Level
        public int GetUserLevel()
        {
            return userLevel;
        }

        public void SetUserLevel(int _userLevel)
        {
            userLevel = _userLevel;
        }

        // Games List
        public Game[] GetGamesList()
        {
            return gamesList.ToArray();
        }

        public void SetGamesList(Game[] _gamesList)
        {
            gamesList = new List<Game>(_gamesList);
        }

        #endregion

        public void AddGamesList(Game game)
        {
            gamesList.Add(game);
        }

        // Methods
        public override string ToString()
        {
            // -- Sorting by total playtime --

            List<Game> sortedPlaytime = gamesList.OrderBy(o => o.GetTotalPlaytime()).Reverse().ToList();

            string playtimeString = "";

            for (int i = 0; i < 5; i++)
            {
                SteamStorePage.GetCommunityTags(sortedPlaytime[i]);
                playtimeString += sortedPlaytime[i].ToString();
            }

            // -- Sorting by recent playtime --
            // This is actually obtainable via the XML page, but this unlimits our reach on it
            // Obtaining via the XML page would also require a game parser from XML results to be created.

            sortedPlaytime = gamesList.OrderBy(o => o.GetRecentPlaytime()).Reverse().ToList();

            string recentPlaytime = "";

            for (int i = 0; i < 5; i++)
            {
                if (sortedPlaytime[i].GetRecentPlaytime() != 0)
                {
                    SteamStorePage.GetCommunityTags(sortedPlaytime[i]);
                    recentPlaytime += sortedPlaytime[i].ToString();
                }
            }

            return $"Steam ID64: {steamID64}\n\tSteam ID: {steamID}\n\tUser Level: {userLevel}\n\tVAC Banned: {vacBanned}\n\tMember Since: {memberSince}\n\nTop 5 Most Played Games:\n{playtimeString}\nTop 5 Recently Played Games:\n{recentPlaytime}\n";
        }

        public float TotalPlaytimeInHours()
        {
            ulong totalPlaytime = 0;
            foreach (Game game in gamesList)
            {
                totalPlaytime += game.GetTotalPlaytime();
            }
            return MathF.Round((float)totalPlaytime / 60, 2);
        }

        public float RecentPlaytimeInHours()
        {
            ulong recentPlaytime = 0;
            foreach (Game game in gamesList)
            {
                recentPlaytime += game.GetRecentPlaytime();
            }
            return MathF.Round((float)recentPlaytime / 60, 2);
        }

        public int AccountLifeTimeInDays()
        {
            DateTime now = DateTime.Now;
            TimeSpan lifeTime = now.Subtract(memberSince);

            return lifeTime.Days;
        }
    }
}