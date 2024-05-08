// This is the main program flow file
// This will ensure everything is accessible through the console at the very least.

using SteamAPI;
using IO;
using SmurfPredictor;

FileOperations.GetPath();        // We need to start by obtaining the path for all files we use
FileOperations.LoadConfigFile(); // Next we need to load the config file. This gives us information such as the API key

// Now we can start initialising dependent processes.

SteamWeb.API_Key = FileOperations.loaded_config.key;

// Now we can ask the user for SteamIDs

IO.Output.Print("Please enter a Steam URL");
string steam_url = IO.Input.Read();

// Get important details about the Steam user.

User user = new User();
SteamXML.GetUserDetails(user, steam_url);
SteamUserPage.GetUserLevel(user, steam_url);
if (SteamWeb.GetOwnedGames(user) != 0)
{
    SteamUserPage.GetGameCountFromBadge(user, steam_url);
}

// Initialise the predictor

SmurfPredictor.SmurfPredictor smurfPredictor = new SmurfPredictor.SmurfPredictor();

AccountDataSchema accountInfo = new AccountDataSchema(user);
IO.Output.Print(accountInfo.ToString());

AccountPrediction prediction = smurfPredictor.Predict(user);

Console.WriteLine($"{prediction.IsSmurf}, {prediction.Probability}");