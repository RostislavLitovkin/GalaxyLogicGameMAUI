# ETHBrno

Added a competition into the game. Will be live in the upcomming weeks.

The code for the competitions part can be found in Competitions branch

![MicrosoftTeams-image](https://user-images.githubusercontent.com/77352013/201514859-bdacfb3d-1259-4094-b3bd-244eda8a3fbc.png)

The record of the game is sent to the server to generate a proof and then saved to the leaderboard.

The winner will win a super rare NFT that can be used in the game, or be sold for a small profit.

You can connect your wallet to the game via WalletConnect. Yes, even on your smartwatch! ^^

![MicrosoftTeams-image (1)](https://user-images.githubusercontent.com/77352013/201514867-41058618-b5df-46b6-b4b0-eb2c31110059.png)

Have a great time competing.

# GalaxyLogicGameMAUI

![GithubPreview](https://user-images.githubusercontent.com/77352013/186893566-edaca4e1-90ed-4a70-a4b1-b8c9adc01bfb.png)

### Description:

Galaxy Logic Game is a logical game for multiple popular platforms.

### Disclaimer:

This game was originaly developed with Xamarin framework (a .net MAUI predecessor).
Also this project exists since I was just beginning my programming journey, so some code oddities are the norm.

Despite this, I am very proud of what I was able to accomplish.

### Platforms supported:

- Android
- WearOS (not fully optimised yet)
- iOS, ipadOS

#### Can be supported, but are untested:

- MacCatalyst
- Windows
- Xbox
- Tizen

### How to run:

I recommend using Visual Studio 2022: https://visualstudio.microsoft.com/vs/community/

Detailed description on how to run the code: https://docs.microsoft.com/en-us/dotnet/maui/get-started/first-app?tabs=vswin&pivots=devices-android

### Folder structure:

GalaxyLogicGame
- code that will be used by every single platform

GalaxyLogicGameMaui
- base, used to setup the code for the majority of platforms (currently android)

GalaxyLogicGameWearOS
- More light-weight base, that loads low-res images, ideal for smartwatch platforms
