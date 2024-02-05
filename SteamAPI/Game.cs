/*
 *      -- Game.cs --
 *      By: Ash Duck
 *      Date: 12/12/2023
 *      Description: This file defines a Game class -- this is used to represent Steam games using the API
 */

namespace SteamAPI
{
    public class Game
    {
        public uint appid;
        public string title;
        public uint playtime_2weeks;
        public uint playtime_forever;
        public bool appinfo;
        public List<string> tags;

        // Constructors
        public Game()
        {
            appid = 0;
            playtime_forever = 0;
            title = "";
            playtime_2weeks = 0;
            appinfo = false;
            tags = new List<string>();
        }

        // As it goes at the moment, these constructors are unused.
        public Game(uint _appid, uint _playtime_forever)
        {
            appid = _appid;
            playtime_forever = _playtime_forever;
            title = "";
            playtime_2weeks = 0;
            appinfo = false;
            tags = new List<string>();
        }

        public Game(uint _appid, uint _playtime_forever, string _title, uint _playtime_2weeks)
        {
            appid = _appid;
            playtime_forever = _playtime_forever;
            title = _title;
            playtime_2weeks = _playtime_2weeks;
            appinfo = true;
            tags = new List<string>();
        }

        public override string ToString()
        {
            string tagsString = "";
            if (tags.Count > 0)
            {
                tagsString = "\n\tTags: " + string.Join(", ", tags);
            }

            if (!appinfo)
            {
                return $"App ID: {appid}\n\tTotal Playtime: {playtime_forever}\n\n";
            }

            else
            {
                // Playtime is always delivered in minutes, hours is how Steam displays it and probably a bit better for our usage?
                return $"App ID: {appid}\n\tTitle: {title}{tagsString}\n\tTotal Hours: {Math.Round((float)playtime_forever / 60, 2)}\n\t\tLast Two Weeks: {Math.Round((float)playtime_2weeks / 60, 2)}\n\n";
            }
        }
    }
}

