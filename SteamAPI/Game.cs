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
        uint _appid;
        string _title;
        uint _playtime2WeeksInMinutes;
        uint _playtimeForeverInMinutes;
        bool _appinfo;
        List<string> _tags;

        // Constructors
        public Game()
        {
            _appid = 0;
            _playtimeForeverInMinutes = 0;
            _title = "";
            _playtime2WeeksInMinutes = 0;
            _appinfo = false;
            _tags = new List<string>();
        }

        // As it goes at the moment, these constructors are unused.
        public Game(uint _appid, uint _playtime_forever)
        {
            this._appid = _appid;
            _playtimeForeverInMinutes = _playtime_forever;
            _title = "";
            _playtime2WeeksInMinutes = 0;
            _appinfo = false;
            _tags = new List<string>();
        }

        public Game(uint _appid, uint _playtime_forever, string _title, uint _playtime_2weeks)
        {
            this._appid = _appid;
            _playtimeForeverInMinutes = _playtime_forever;
            this._title = _title;
            _playtime2WeeksInMinutes = _playtime_2weeks;
            _appinfo = true;
            _tags = new List<string>();
        }

        // Getters/Setters

        // App ID
        public uint GetAppID()
        {
            return _appid;
        }

        public void SetAppID(uint _appid)
        {
            this._appid = _appid;
        }

        // Game Title
        public string GetTitle()
        {
            return _title;
        }

        public void SetTitle(string _title)
        {
            this._title = _title;
        }

        // Playtime across two weeks
        public uint GetRecentPlaytimeInMinutes()
        {
            return _playtime2WeeksInMinutes;
        }

        public void SetRecentPlaytimeInMinutes(uint playtime2WeeksInMinutes)
        {
            _playtime2WeeksInMinutes = playtime2WeeksInMinutes;
        }

        // Playtime across all time
        public uint GetTotalPlaytimeInMinutes()
        {
            return _playtimeForeverInMinutes;
        }

        public void SetTotalPlaytimeInMinutes(uint playtimeForeverInMinutes)
        {
            _playtimeForeverInMinutes = playtimeForeverInMinutes;
        }

        // Is App Info available?
        public bool GetAppInfo()
        {
            return _appinfo;
        }

        public void SetAppInfo(bool appinfo)
        {
            _appinfo = appinfo;
        }

        // Store Tags
        public string[] GetStoreTags()
        {
            return _tags.ToArray();
        }

        public void SetStoreTags(string[] tags)
        {
            _tags = new List<string>(tags);
        }

        public void AddStoreTag(string tag)
        {
            _tags.Add(tag);
        }

        // Methods
        public override string ToString()
        {
            string tagsString = "";
            if (_tags.Count > 0)
            {
                tagsString = "\n\tTags: " + string.Join(", ", _tags.GetRange(0,Math.Min(_tags.Count, 5)));
            }

            if (!_appinfo)
            {
                return $"App ID: {_appid}\n\tTotal Playtime: {_playtimeForeverInMinutes}\n\n";
            }

            else
            {
                // Playtime is always delivered in minutes, hours is how Steam displays it and probably a bit better for our usage?
                return $"App ID: {_appid}\n\tTitle: {_title}{tagsString}\n\tTotal Hours: {Math.Round((float)_playtimeForeverInMinutes / 60, 2)}\n\t\tLast Two Weeks: {Math.Round((float)_playtime2WeeksInMinutes / 60, 2)}\n\n";
            }
        }
    }
}

