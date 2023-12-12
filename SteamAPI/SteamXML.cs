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
        public static void GetUserDetails(User user, string url)
        {
            //
            // This method gets information from a given URL that might be useful to us and populates a user object with the data.
            // Requires: user != null, Steam Community URL
            //

            // This could become a bool in future to signify Public/Private acc (true/false) to halt other areas of the code executing.

            // XML parsing is slow, I don't really want to do it multiple times.
            string XMLPage = HTMLRequest.GetHTMLPage(url + "?xml=1");
            XmlDocument document = new XmlDocument();
            document.LoadXml(XMLPage);

            // These paths are always the same (unless Valve updates the format, which would not be great.)
            // Even though VS complains they *may* be null, it can be assumed they won't be.
            string id64 = document.SelectSingleNode("/profile/steamID64").InnerText;
            string id = document.SelectSingleNode("/profile/steamID").InnerText;
            string vac = document.SelectSingleNode("/profile/vacBanned").InnerText;
            string member = document.SelectSingleNode("/profile/memberSince").InnerText;

            // They all come back as strings, so I do all the conversions here.
            user.steamID64 = ulong.Parse(id64);
            user.steamID = id;
            user.vacBanned = Convert.ToBoolean(int.Parse(vac));
            user.memberSince = member;
        }
    }
}