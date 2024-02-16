/*
 *      -- SteamXML.cs --
 *      By: Ash Duck
 *      Date: 12/12/2023
 *      Description: This file retrieves profile data as an XML format and deserializes it.
 */

using System.Xml;

namespace SteamAPI
{
    public class SteamXML
    {
        public static int GetUserDetails(User user, string url)
        {
            //
            // This method gets information from a given URL that might be useful to us and populates a user object with the data.
            // Requires: user != null, Steam Community URL
            // Returns: 0 (if success), 1 (if unsuccessful)
            //

            // This could become a bool in future to signify Public/Private acc (true/false) to halt other areas of the code executing.

            // XML parsing is slow, I don't really want to do it multiple times.
            Output.LogProgress("Requesting Community XML");
            string XMLPage = HTMLRequest.GetHTMLPage(url + "?xml=1");
            Output.LogProgress("Converting response to document");
            XmlDocument document = new XmlDocument();
            document.LoadXml(XMLPage);

            // These paths are always the same (unless Valve updates the format, which would not be great.)
            // Even though VS complains they *may* be null, it can be assumed they won't be.
            Output.LogProgress("Finding id64, id, vac & member");
            try
            {
                string id64 = document.SelectSingleNode("/profile/steamID64").InnerText;
                string id = document.SelectSingleNode("/profile/steamID").InnerText;
                string vac = document.SelectSingleNode("/profile/vacBanned").InnerText;
                string member = document.SelectSingleNode("/profile/memberSince").InnerText;

                // They all come back as strings, so I do all the conversions here.
                Output.LogProgress("Converting all to correct types");
                user.SetSteamID64(ulong.Parse(id64));
                user.SetSteamID(id);
                user.SetVacStatus(Convert.ToBoolean(int.Parse(vac)));
                user.SetJoinDate(member);

                return 0;           // 0 means success
            }

            catch
            {
                Output.Error($"User account details cannot be obtained!\nURL: {url}");
                return 1;
            }
        }
    }
}