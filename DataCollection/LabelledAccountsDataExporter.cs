/*
    Description: Takes a spreadsheet of labelled accounts, uses the api to get information about the account
                 Outputs the information into another spreadsheet, to be used for training. 
*/


using System;
using System.Text.Json;
using SteamAPI;


namespace DataCollection
{
    /// <summary>
    /// Takes a csv of labelled accounts, uses the api to get information about the account
    /// Saves the information to a csv, for it to be used for training.
    /// </summary>
    internal class LabelledAccountsDataExporter
    {
        string _apiKey;
        string _sheetsID;
        string _sheetsGID;
        string _labelledAccountsDataFile;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configFile">Location of config</param>
        /// <param name="labelledAccountsDataFile">Location of output file with all the labelled accounts with user data</param>
        public LabelledAccountsDataExporter(string configFile, string labelledAccountsDataFile)
        {
            LoadConfigFile(configFile);
            _labelledAccountsDataFile = labelledAccountsDataFile;
        }

        /// <summary>
        /// Saves labelled accounts with the following data: games owned, total playtime, account lifetime, and total recent playtime
        /// </summary>
        public void ExportLabelledAccountsData()
        {
            List<User> accounts = GetAccounts();

            foreach (User user in accounts)
            {
                Console.WriteLine(user.GetGamesList().Count());
            }

            using (StreamWriter sw = new StreamWriter(_labelledAccountsDataFile))
            {
                foreach (User account in accounts)
                {
                    // games owned, total playtime, account lifetime, label
                    sw.WriteLine($"{account.GetGamesList().Count()},{account.TotalPlaytimeInHours()},{account.AccountLifeTimeInDays()},{account.RecentPlaytimeInHours().ToString()},{account.IsSmurf}");
                    Console.WriteLine($"total hr: {account.TotalPlaytimeInHours()}");
                }
            }

        }

        // We should probably refactor this later for loading API keys and maybe other things for GUI?
        private void LoadConfigFile(string configFile)
        {
            CreateConfigIfNotExists(configFile);
            using (StreamReader sr = new StreamReader(configFile))
            {
                string file = sr.ReadToEnd();
                Config config = JsonSerializer.Deserialize<Config>(file);
                _apiKey = config.key;
                SteamWeb.API_Key = config.key;
                _sheetsID = config.sheetsid;
                _sheetsGID = config.sheetsgid;
            }
        }

        /// <summary>
        /// If config.json not present, one is created with the correct format
        /// </summary>
        /// <param name="configFile"></param>
        private void CreateConfigIfNotExists(string configFile)
        {
            if (!File.Exists(configFile))
            {
                using (StreamWriter sw = new StreamWriter(configFile))
                {
                    Config newConfig = new Config();
                    sw.Write(JsonSerializer.Serialize(newConfig));
                    Console.WriteLine($"New config created, {configFile}, populate fields.");
                }
            }
        }

        /// <summary>
        /// Obtains a CSV from a given Google Sheets page
        /// </summary>
        /// <param name="sheetsID"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        private List<string> GetLinesFromSheets(string sheetsID, string gid)
        {
            // I'm just going to use the HTML stuff from SteamAPI because it's easy
            // Modified from https://stackoverflow.com/questions/37705553/how-to-export-a-csv-from-google-sheet-api
            Console.WriteLine("Getting CSV from Google Sheets");
            string result = SteamAPI.HTMLRequest.GetHTMLPage($"https://docs.google.com/spreadsheets/d/{sheetsID}/gviz/tq?tqx=out:csv&gid={gid}");
            return result.Replace("\"", string.Empty).Split("\n").ToList();
        }

        /// <summary>
        /// A list of accounts, all from the labelled accounts file. 
        /// </summary>
        /// <returns></returns>
        private List<User> GetAccounts()
        {
            List<string> lines = GetLinesFromSheets(_sheetsID, _sheetsGID);
            List<User> accounts = new List<User>();
            Console.WriteLine("Obtaining user data...");
            while (lines.Count >= 1)
            {
                string line = lines.First();
                lines.RemoveAt(0);
                List<string> lineParts = line.Split(',').ToList();

                User user = new User();
                string accountUrl = lineParts[0];
                int accountLabel = Convert.ToInt32(lineParts[1]);

                // Populate details, and game list
                if (
                    SteamXML.GetUserDetails(user, accountUrl) == 0 &&
                    SteamWeb.GetOwnedGames(user) == 0 &&
                    user.TotalPlaytimeInHours() != 0
                )
                {
                    user.IsSmurf = accountLabel;
                    accounts.Add(user);
                }
                else
                {
                    SteamAPI.Output.Error($"Didn't add user {user.GetSteamID()}");
                }
            }
            return accounts;
        }
    }
}
