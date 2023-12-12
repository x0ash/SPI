/*
 *      -- Program.cs --
 *      By: Ash Duck
 *      Date: 12/12/2023
 *      Description: This is a test program just for SteamWeb & SteamXML functionality.
 */

using System;
using SteamAPI;

// Ideally in this bit we'd be saving the API key so the user wouldn't have to go through this process all the time.

Console.WriteLine("Hi! It seems like you haven't provided an API key...\nDon't Worry! Just go to: 'https://steamcommunity.com/dev/apikey' and register for one!\nWhen you're done, come back here and provide me with the key :)");
Console.Write("Insert API Key Here >> ");

string api_key = Console.ReadLine();
SteamWeb.API_Key = api_key;

Console.WriteLine("Yum! I love API keys!\nFor desserts: I want a Steam URL to check :)");
Console.Write("Insert Steam Community URL Here >> ");

string steam_url = Console.ReadLine();

Console.WriteLine("Here's your info :)\n");

// Here's where the important stuff starts

User testUser = new User();

// Firstly we should obtain the XML page, as it gives us the SteamID64, which is required for Web Requests.
// We can get information from the XML page...
//      1: Without running out of requests (which happens with the API)
//      2: With just the URL (which means custom URLs are supported)

SteamAPI.SteamXML.GetUserDetails(testUser, steam_url);

// This populates the user with:
//      1: SteamID64 (their actual unique ID on Steam)
//      2: SteamID (their custom username)
//      3: Their VAC Ban Status
//      4: When they joined Steam

// To access the full games list, we need to use the Steam Web API.

SteamAPI.SteamWeb.GetOwnedGames(testUser);

// This populates their games list with metadata including:
//      1: Unique App ID (ex. 730 = CS:GO, 70 = Half-Life)
//      2: Game Title
//      3: Time Played in Total
//      4: Time Played in the Last Two Weeks (this is not returned if that number is 0hrs)

Console.WriteLine(testUser.ToString());

// This outputs the user data in a nice way in the console, which is good for testing.

Console.WriteLine("You can press any key to exit! :)");
Console.Read();         // Wait for the user to press a key to exit