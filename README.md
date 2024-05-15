# Steam Player Investigator

Steam Player Investigator

### Sections

[GUI](#gui)

[Model Training](#model-training)

[Data Collection](#data-collection)

[SteamAPI](#steamapi)

## GUI

The final part, which consumes the API and model. 

## Smurf predictor and training

Train binary classification models using labelled data produced from data collection.
Currently 2 models, LdSvm and fast tree models.

LdSvm does not provide score, fast tree does.

Make sure there are models in assembly folder before using prediction (copy from SteamPlayerInvestigator/Models).

Example of using smurfPredictor in SmurfPredictor Program.cs. 

## Data Collection

Takes labelled accounts, uses the API to get account data and saves it to a file.

- Run the DataCollection main, to generate LabelledAccountsWithData.csv

### config.json

Requires this format:

| Key | Description |
|-----|-------------|
| key | Steam Web API key |
| sheetsid | Google Sheets document ID |
| sheetsgid | Google Sheets tab GID |

### LabelledAccountsWithData.csv

The table/data for training will look something like this, open to change.

| No. Games owned |Account lifetime (days) |  Total playtime (hrs) | Total Recent Playtime (hrs) | Label |
|-----------------|----------------|------------------|-----------------------|-------|
| | | | | |
| | | | | |
| | | | | |


## SteamAPI

This is a proof of concept for using C# & .NET to interact with the Steam Web API and Steam Community Profiles.

### What is included?

Currently, there is the ability to find:
- SteamID64 via Steam Community URL (XML)
- Steam Username via Steam Community URL (XML) & SteamID64 (Web API)
- Steam User Level via Steam Community URL (HTML)
- VAC Ban Status via Steam Community URL (XML)
- Steam Join Date via Steam Community URL (XML) & SteamID64 (Web API)
- Owned Games via SteamID64 (Web API)
	- Most played games (sorted)
	- Most played games across last two weeks (sorted)
- Ability to find user level
- Ability to determine game tags (obtained via the store page)

*(Please note that getting information via Steam Community URL is preferred where possible)*

Also included is an example Program.cs to get you started.

### What are the limitations?

Currently, whoever is using the program will have to provide their [*own Web API key*](https://steamcommunity.com/dev/apikey), though **vague** instructions have been provided on how you get one.
Additionally, it comes with all of the costs of using the Steam Web API key, which is currently restricted to **100,000 calls** per day [[Source](https://steamcommunity.com/dev/apiterms)]

It also does *not* save the API key locally, so it will have to be provided every time.

Some features (specifically requesting full HTML pages) lead to extreme slowdown and many requests may eventually lead to access being temporarily denied.
