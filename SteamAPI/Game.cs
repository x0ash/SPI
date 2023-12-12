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
        public int appid;
        public string title;
        public int playtime_2weeks;
        public int playtime_forever;
        public bool appinfo;

        // Constructors
        public Game()
        {
            appid = 0;
            playtime_forever = 0;
            title = "";
            playtime_2weeks = 0;
            appinfo = false;
        }

        // As it goes at the moment, these constructors are unused.
        public Game(int _appid, int _playtime_forever)
        {
            appid = _appid;
            playtime_forever = _playtime_forever;
            title = "";
            playtime_2weeks = 0;
            appinfo = false;
        }

        public Game(int _appid, int _playtime_forever, string _title, int _playtime_2weeks)
        {
            appid = _appid;
            playtime_forever = _playtime_forever;
            title = _title;
            playtime_2weeks = _playtime_2weeks;
            appinfo = true;
        }

        public override string ToString()
        {
            if (!appinfo)
            {
                return $"App ID: {appid}\n\tTotal Playtime: {playtime_forever}\n\n";
            }

            else
            {
                // Playtime is always delivered in minutes, hours is how Steam displays it and probably a bit better for our usage?
                return $"App ID: {appid}\n\tTitle: {title}\n\tTotal Hours: {Math.Round((float)playtime_forever / 60, 2)}\n\t\tLast Two Weeks: {Math.Round((float)playtime_2weeks / 60, 2)}\n\n";
            }
        }
    }
}

