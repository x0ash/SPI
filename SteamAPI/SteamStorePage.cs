/*
 *      -- SteamStorePage.cs --
 *      By: Ash Duck
 *      Date: 05/02/2024
 *      Description: This file retrieves data from Steam Store Pages (in HTML) and parses relevant information.
 */

using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace SteamAPI
{
	public class SteamStorePage
	{
        // We want to be using this sparingly - full webpage requests are significantly slower than making API/XML requests
		public static void GetCommunityTags(Game game)
		{
            //
            // This method obtains the community tags from the Steam store to conclude the genre of the game.
            // Requires: game != null
            //

            if (game.GetStoreTags().Count() == 0)
            {
                // We know the appID already, and Steam makes it very convenient to find store pages based on this.
                Output.LogProgress("Requesting store page");

                // Games with 'mature content' will redirect to a age verification page upon loading.
                // We need to simulate someone passing the age-verification check.

                uint appID = game.GetAppID();
                string storePage = HTMLRequest.GetHTMLPage("https://store.steampowered.com/app/" + appID);
                Output.LogProgress("Converting store page to document");
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(storePage);

                // After loading the page, we can check to see if we got age-gated.
                IEnumerable<HtmlAgilityPack.HtmlNode> ageGateNode = Fizzler.Systems.HtmlAgilityPack.HtmlNodeSelection.QuerySelectorAll(document.DocumentNode, "div.agegate_birthday_desc");
                if (ageGateNode.Count() != 0)             // On a regular store page, this won't exist.
                {

                    // Assuming that we've been age-gated, then we need to find the session ID to spoof the POST request.
                    // We have the full document at this point, and it's stored as a JavaScript variable. Unfortunately, HTMLAgilityPack
                    // doesn't support finding JavaScript variables out of that, so we have to use Regex.

                    Match match = Regex.Match(storePage, "g_sessionID = ");

                    // The content string has to match *exactly* what Valve's CheckAgeGateSubmit(callbackFunc) function POSTS to the server
                    // on their store page. It has a fixed arbitrary valid date alongside the obtained session id. It also needs to have
                    // the same content-type and probably encoding?

                    // Session ids are always 24 characters long and are always prefaced with "g_sessionID = " so that needs omitting.
                    StringContent content = new StringContent(
                        $"sessionid={storePage.Substring(match.Index + 15, 24)}&ageDay=5&ageMonth=February&ageYear=2004",
                        Encoding.UTF8,
                        "application/x-www-form-urlencoded"
                        );

                    // GetPOST returns the string response from the server, but it's not needed unless for debugging purposes.
                    // However, if it's needed, success code 1 is what we're looking for. 15 means the data supplied is malformed.
                    // If 15 is encountered in the future, CheckAgeGateSubmit has probably been updated and needs matching again.

                    HTMLRequest.GetPOST($"https://store.steampowered.com/agecheckset/app/{appID}/", content);

                    // The Steam website uses a cookie to record whether the user wants to see mature content or not. However, it's not
                    // required as long as the user remains in the same session, which will happen as long as the program is open.
                    // After this, load the original page again to get the tags.

                    storePage = HTMLRequest.GetHTMLPage("https://store.steampowered.com/app/" + appID);
                    document.LoadHtml(storePage);
                }

                // Next we can obtain the tags (all have class 'app_tag')
                Output.LogProgress("Obtaining tags");
                IEnumerable<HtmlAgilityPack.HtmlNode> tagNodes = Fizzler.Systems.HtmlAgilityPack.HtmlNodeSelection.QuerySelectorAll(document.DocumentNode, "a.app_tag");

                // Iterate through and add to the game's tags.
                Output.LogProgress("Adding tags to list");
                foreach (HtmlAgilityPack.HtmlNode node in tagNodes)
                {
                    // Tags have awkward formatting like "\r\n\t\t\t\t\t\t\t\t\t\t\t\tFPS\t\t\t\t\t\t\t\t\t\t\t\t"
                    // This filter removes all the leading/trailing special characters.
                    string filteredText = node.InnerText.Substring(14, node.InnerText.Length - 26);
                    game.AddStoreTag(filteredText);

                }
            }
        }
	}
}

