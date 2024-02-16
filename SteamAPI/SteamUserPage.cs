/*
 *      -- SteamUserPage.cs --
 *      By: Ash Duck
 *      Date: 05/02/2024
 *      Description: This file retrieves data from Steam User Pages (in HTML) and parses relevant information.
 */

namespace SteamAPI
{
	public class SteamUserPage
	{
		public static void GetUserLevel(User user, string url)
		{
			//
			// This method gets the user level from a given URL and populates a user object with the data.
			// Requires: user != null, Steam Community URL
			//

			Output.LogProgress("Requesting user page (HTML)");
            string userPage = HTMLRequest.GetHTMLPage(url);
			Output.LogProgress("Converting user page to document");
			HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
			document.LoadHtml(userPage);

			// There is only a single node with this class, and inside is the user level.
			Output.LogProgress("Obtaining the user's level");
			HtmlAgilityPack.HtmlNode levelNode = Fizzler.Systems.HtmlAgilityPack.HtmlNodeSelection.QuerySelectorAll(document.DocumentNode, "span.friendPlayerLevelNum").First();

			user.SetUserLevel(int.Parse(levelNode.InnerText));
		}
	}
}

