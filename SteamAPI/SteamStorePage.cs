/*
 *      -- SteamStorePage.cs --
 *      By: Ash Duck
 *      Date: 05/02/2024
 *      Description: This file retrieves data from Steam Store Pages (in HTML) and parses relevant information.
 */

using System;

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

            // We know the appID already, and Steam makes it very convenient to find store pages based on this.
            string StorePage = HTMLRequest.GetHTMLPage("https://store.steampowered.com/app/"+game.appid);
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(StorePage);

            // Next we can obtain the tags (all have class 'app_tag')
            IEnumerable<HtmlAgilityPack.HtmlNode> tagNodes = Fizzler.Systems.HtmlAgilityPack.HtmlNodeSelection.QuerySelectorAll(document.DocumentNode, "a.app_tag");

            // Iterate through and add to the game's tags.
            foreach (HtmlAgilityPack.HtmlNode node in tagNodes)
            {
                // Tags have awkward formatting like "\r\n\t\t\t\t\t\t\t\t\t\t\t\tFPS\t\t\t\t\t\t\t\t\t\t\t\t"
                // This filter removes all the leading/trailing special characters.
                string filteredText = node.InnerText.Substring(14, node.InnerText.Length - 26);
                game.tags.Add(filteredText);
            }

        }
	}
}

