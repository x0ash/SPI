﻿/*
    Description: Takes a spreadsheet of labelled accounts, uses the api to get information about the account
                 Outputs the information into another spreadsheet, to be used for training. 
*/


using System;
using SteamAPI;


namespace DataCollection
{
    internal class LabelledAccountsDataExporter
    {

        // in: api key, csv
        // out: csv
        string _apiKey;
        string _labelledAccountsFile;
        string _labelledAccountsDataFile;

        public LabelledAccountsDataExporter(string apiKey, string labelledAccountsFile, string labelledAccountsDataFile)
        {
            _apiKey = apiKey;
            _labelledAccountsFile = labelledAccountsFile;
            _labelledAccountsDataFile = labelledAccountsDataFile;
        }

        public void ExportLabelledAccountsData()
        {
            List<User> accounts = GetAccounts();

            foreach (User user in accounts)
            {
                Console.WriteLine(user.gamesList.Count());
            }

            using (StreamWriter sw = new StreamWriter(_labelledAccountsDataFile))
            {
                foreach (User account in accounts)
                {
                    // games owned, total playtime, account lifetime, label
                    sw.WriteLine($"{account.gamesList.Count()},{account.TotalPlaytime()}");
                }
            }
            
        }

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
                // Need to change this
                // This does not populate gamelist
                SteamXML.GetUserDetails(user, accountUrl);
                user.isSmurf = accountLabel;
                accounts.Add(user);
            }
            return accounts;
        }
    }
}
