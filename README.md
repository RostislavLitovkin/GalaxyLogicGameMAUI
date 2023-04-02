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
- WearOS
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
