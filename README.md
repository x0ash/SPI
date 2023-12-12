# SteamAPI

This is a proof of concept for using C# & .NET to interact with the Steam Web API and Steam Community XML Profiles.

### What is included?

Currently, there is the ability to find:
- SteamID64 via Steam Community URL (XML)
- Steam Username via Steam Community URL (XML) & SteamID64 (Web API)
- VAC Ban Status via Steam Community URL (XML)
- Steam Join Date via Steam Community URL (XML) & SteamID64 (Web API)
- Owned Games via SteamID64 (Web API)
	- Most played games (sorted)
	- Most played games across last two weeks (sorted)

*(Please note that getting information via Steam Community URL is preferred where possible)*

Also included is an example Program.cs to get you started.

### What are the limitations?

Currently, whoever is using the program will have to provide their [*own Web API key*](https://steamcommunity.com/dev/apikey), though **vague** instructions have been provided on how you get one.
Additionally, it comes with all of the costs of using the Steam Web API key, which is currently restricted to **100,000 calls** per day [[Source](https://steamcommunity.com/dev/apiterms)]

It also does *not* save the API key locally, so it will have to be provided every time.

It's also currently untested with private Steam accounts -- I only tested it with my own. I'd expect a crash however.