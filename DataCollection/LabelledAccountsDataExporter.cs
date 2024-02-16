/*
    Description: Takes a spreadsheet of labelled accounts, uses the api to get information about the account
                 Outputs the information into another spreadsheet, to be used for training. 
*/


using System;
using SteamAPI;


namespace DataCollection
{
    /// <summary>
    /// Takes a csv of labelled accounts, uses the api to get information about the account
    /// Saves the information to a csv, for it to be used for training.
    /// </summary>
    internal class LabelledAccountsDataExporter
    {

        // in: api key, csv
        // out: csv
        string _apiKey;
        string _labelledAccountsFile;
        string _labelledAccountsDataFile;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="labelledAccountsFile">Location of the file with all the labelled accounts</param>
        /// <param name="labelledAccountsDataFile">Location of output file with all the labelled accounts with user data</param>
        public LabelledAccountsDataExporter(string apiKey, string labelledAccountsFile, string labelledAccountsDataFile)
        {
            _apiKey = apiKey;
            SteamWeb.API_Key = apiKey;
            _labelledAccountsFile = labelledAccountsFile;
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
                    sw.WriteLine($"{account.GetGamesList().Count()},{account.TotalPlaytimeInHours()},{account.AccountLifeTimeInDays()},{account.RecentPlaytimeInHours().ToString()}");
                    Console.WriteLine($"total hr: {account.TotalPlaytimeInHours()}");
                }
            }
            
        }

        /// <summary>
        /// Gets each line in the labelled accounts file.
        /// </summary>
        /// <returns></returns>
        private List<string> GetLabelledAccountLines()
        {
            List<string> lines = new List<string>();
            using (StreamReader sr = new StreamReader(_labelledAccountsFile))
            {
                // get rid of first line
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    lines.Add(sr.ReadLine());
                }
            }
            return lines;
        }

        /// <summary>
        /// A list of accounts, all from the labelled accounts file. 
        /// </summary>
        /// <returns></returns>
        private List<User> GetAccounts()
        {
            List<string> lines = GetLabelledAccountLines();
            List<User> accounts = new List<User>();
            while (lines.Count >= 1)
            {
                string line = lines.First();
                lines.RemoveAt(0);
                List<string> lineParts = line.Split(',').ToList();

                User user = new User();
                string accountUrl = lineParts[0];
                int accountLabel = Convert.ToInt32(lineParts[1]);

                // Populate details, and game list
                ;
                ;
                if (
                    SteamXML.GetUserDetails(user, accountUrl) == 0 &&
                    SteamWeb.GetOwnedGames(user) == 0 &&
                    user.TotalPlaytimeInHours() != 0
                )
                {
                    user.isSmurf = accountLabel;
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
