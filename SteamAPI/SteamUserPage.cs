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

            string UserPage = HTMLRequest.GetHTMLPage(url);
			HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
			document.LoadHtml(UserPage);

			// There is only a single node with this class, and inside is the user level.
			HtmlAgilityPack.HtmlNode levelNode = Fizzler.Systems.HtmlAgilityPack.HtmlNodeSelection.QuerySelectorAll(document.DocumentNode, "span.friendPlayerLevelNum").First();

			user.userLevel = int.Parse(levelNode.InnerText);
		}
	}
}

