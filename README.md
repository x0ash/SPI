# Steam Player Investigator

Steam player investigator TSE project.

### Sections

[GUI](#gui)

[Model Training](#model-training)

[Data Collection](#data-collection)

[SteamAPI](#steamapi)

## GUI

The final part, which consumes the API and model. 

## Model Training

Training an SVM off labelled data.

## Data Collection

Takes labelled accounts, uses the API to get account data and saves it to a file.

- Run the DataCollection main, to generate LabelledAccountsWithData.csv

### config.cfg

Requires this format:

| URL of steam account |
|----------------------|
| Steam Web API Key    |
| Google Sheets ID     |
| Google Sheets Tab GID|

### LabelledAccountsWithData.csv

The table/data for training will look something like this, open to change.

| No. Games owned | Total playtime (hrs)| Account lifetime (days)| Total Recent Playtime (hrs) | Label |
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