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
        uint appid;
        string title;
        uint playtime_2weeks;
        uint playtime_forever;
        bool appinfo;
        List<string> tags;

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

        // Getters/Setters

        // App ID
        public uint GetAppID()
        {
            return appid;
        }

        public void SetAppID(uint _appid)
        {
            appid = _appid;
        }

        // Game Title
        public string GetTitle()
        {
            return title;
        }

        public void SetTitle(string _title)
        {
            title = _title;
        }

        // Playtime across two weeks
        public uint GetRecentPlaytime()
        {
            return playtime_2weeks;
        }

        public void SetRecentPlaytime(uint _playtime_2weeks)
        {
            playtime_2weeks = _playtime_2weeks;
        }

        // Playtime across all time
        public uint GetTotalPlaytime()
        {
            return playtime_forever;
        }

        public void SetTotalPlaytime(uint _playtime_forever)
        {
            playtime_forever = _playtime_forever;
        }

        // Is App Info available?
        public bool GetAppInfo()
        {
            return appinfo;
        }

        public void SetAppInfo(bool _appinfo)
        {
            appinfo = _appinfo;
        }

        // Store Tags
        public string[] GetStoreTags()
        {
            return tags.ToArray();
        }

        public void SetStoreTags(string[] _tags)
        {
            tags = new List<string>(_tags);
        }

        public void AddStoreTag(string tag)
        {
            tags.Add(tag);
        }

        // Methods
        public override string ToString()
        {
            string tagsString = "";
            if (tags.Count > 0)
            {
                tagsString = "\n\tTags: " + string.Join(", ", tags.GetRange(0,Math.Min(tags.Count, 5)));
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

