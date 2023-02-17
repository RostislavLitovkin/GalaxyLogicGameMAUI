using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Pages_and_descriptions;
using GalaxyLogicGame.Pagesanddescriptions;

using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Tutorial;
//using MarcTron.Plugin;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

[assembly: ExportFont("SamsungOne700.ttf", Alias = "SamsungOne")]
[assembly: ExportFont("bignoodletitling.ttf", Alias = "BigNoodleTitling")]

namespace GalaxyLogicGame.Mobile
{

    public partial class MainPage : ContentPage, IMainPage
    {
        public const bool IS_ULTRA = true;

        public const string COMPLETED_TUTORIAL = "org.tizen.myApp.completedTutorial";
        public const string FIRST = "org.tizen.myApp.first";
        public const string HIGHSCORE = "org.tizen.myApp.highscore";

        public IGameBG gameBG;
        private bool clicked = true;
        private StarsParticlesLayout stars = new StarsParticlesLayout();

        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
            SizeChanged += OnDisplaySizeChanged;

            if (Functions.IsSquareScreen())
            {
                AbsoluteLayout.SetLayoutBounds(buttonLayout, new Rect(0.5, 0.5, 1, 1));
            }

            Functions.ScaleToScreen(this, scaleLayout);
            Functions.FillHeight(scaleLayout);
            //tutorialButtonsLayout.Scale = mainLayout.Scale;

            starsLayout.Children.Add(stars);

            if (!Preferences.Get("experimental", false)) highscoreLabel.Text = "Highscore: " + Preferences.Get(HIGHSCORE, 0);
            else highscoreLabel.Text = "Experimental mode";

            if (!Preferences.Get("tutorialCompleted", false))
            {

                /*tutorialButtonsLayout.TranslationX = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

                tutorialLayout.IsVisible = true;

                Task.Run(async () => {
                    await Task.Delay(500);
                    await tutorialButtonsLayout.TranslateTo(0, 0, 500, Easing.SpringOut);
                });*/
                tutorialLayout.IsVisible = true;
            }
            else
            {
                Astronaut();
            }


        }

        private void OnDisplaySizeChanged(object sender, EventArgs args)
        {
            if (Functions.IsSquareScreen())
            {
                AbsoluteLayout.SetLayoutBounds(buttonLayout, new Rect(0.5, 0.5, 1, 1));
            }
            Functions.ScaleToScreen(this, scaleLayout);
            Functions.FillHeight(scaleLayout);
        }
        private async Task Astronaut()
        {
            Random random = new Random();
            await Task.Delay(2000);
            astronaut.Rotation = random.Next(360);
            await astronaut.TranslateTo(-210, -60, 10000);
        }
        private async void OnShowMoreClicked(object sender, EventArgs e)
        {
            DisplayInfo display = DeviceDisplay.MainDisplayInfo;
            double ratio = display.Height / display.Width > 1 ? display.Height / display.Width : 1;


            //showMoreButton.Opacity = 0.7;
            await Task.WhenAll(
                stars.TransitionUpIn(),

                scaleLayout.TranslateTo(0, -360 * ratio * scaleLayout.Scale, 500, Easing.SinIn),
                stars.TranslateTo(0, 360 * ratio, 500, Easing.SinIn),
                //wallpaper.TranslateTo(0, -180, 500, Easing.SinIn),
                wallpaper.FadeTo(0, 500, Easing.SinIn));
            LongMainPage mainPage = new LongMainPage(stars);
            stars.TranslationY = 0;
            await Navigation.PushAsync(mainPage, false);
            await mainPage.TransitionIn();
            Navigation.RemovePage(this);
        }

        private async void OnPlayClicked(object sender, EventArgs e)
        {
            //((Label)sender).Opacity = 0.7;
            Preferences.Set("save", "");

            if (Preferences.Get("save", "") != "")
            {
                if (clicked)
                {
                    CasualGame game = new GameWithEvents();
                    GameBG gameBG = new GameBG();
                    clicked = false;

                    //TransitionIn transition = new TransitionIn();
                    //await transition.Play(transitionLayout, 500);

                    this.gameBG?.StopLoop();
                    this.gameBG = gameBG;
                    gameBG.StartLoop();

                    game.BG = gameBG;
                    game.MainMenuPage = this;

                    await Navigation.PushAsync(gameBG, false);
                    await gameBG.ContinueFromSave(game);

                    clicked = true;

                    //transition.Stop();
                }
            }
            else
            {
                await NavigateToGame(new GameBG(), new GameWithEvents());
            }
            //((Label)sender).Opacity = 1;
        }

        private async void OnTutorialClicked(object sender, EventArgs e)
        {
            await NavigateToGame(new GameBG(Color.FromHex("222")), new TutorialPlacingPlanets());
            tutorialLayout.IsVisible = false;
        }

        private async Task NavigateToGame(GameBG gameBG, CasualGame game)
        {
            if (clicked)
            {
                clicked = false;

                //TransitionIn transition = new TransitionIn();
                //await transition.Play(transitionLayout, 500);

                this.gameBG?.StopLoop();
                this.gameBG = gameBG;
                gameBG.StartLoop();

                game.BG = gameBG;
                game.MainMenuPage = this;

                await Navigation.PushAsync(gameBG, false);
                await gameBG.Setup(game);

                clicked = true;

                //transition.Stop();
            }
        }

        /*private async void OnConnectCryptoClicked(object sender, EventArgs e)
        {
            //SolanaConnectPage page = new SolanaConnectPage(wallet);
            CryptoConnectPage page = new CryptoConnectPage(wallet);

            DisplayInfo display = DeviceDisplay.MainDisplayInfo;
            double ratio = display.Height / display.Width > 1 ? display.Height / display.Width : 1;

            await Task.WhenAll(
                this.stars.TransitionUpIn(),

                scaleLayout.TranslateTo(0, -360 * ratio * scaleLayout.Scale, 500, Easing.SinIn),

                //wallpaper.TranslateTo(0, -180, 500, Easing.SinIn),
                wallpaper.FadeTo(0, 500, Easing.SinIn));

            starsLayout.Children.Remove(this.stars);
            page.AssignStars(stars);
            await Navigation.PushAsync((Page)page, false);
            await page.TransitionIn();

            scaleLayout.TranslationX = 0;
            scaleLayout.TranslationY = 0;
            wallpaper.Opacity = 1;

            this.stars = new StarsParticlesLayout();
            starsLayout.Children.Add(this.stars);

            await page.Connect();
        }*/

        private async void OnConnectPolkadotClicked(object sender, EventArgs e)
        {
            //SolanaConnectPage page = new SolanaConnectPage(wallet);
            PolkadotConnectPage page = new PolkadotConnectPage(wallet);

            DisplayInfo display = DeviceDisplay.MainDisplayInfo;
            double ratio = display.Height / display.Width > 1 ? display.Height / display.Width : 1;

            await Task.WhenAll(
                this.stars.TransitionUpIn(),

                scaleLayout.TranslateTo(0, -360 * ratio * scaleLayout.Scale, 500, Easing.SinIn),

                //wallpaper.TranslateTo(0, -180, 500, Easing.SinIn),
                wallpaper.FadeTo(0, 500, Easing.SinIn));

            starsLayout.Children.Remove(this.stars);
            page.AssignStars(stars);
            await Navigation.PushAsync((Page)page, false);
            await page.TransitionIn();

            scaleLayout.TranslationX = 0;
            scaleLayout.TranslationY = 0;
            wallpaper.Opacity = 1;

            this.stars = new StarsParticlesLayout();
            starsLayout.Children.Add(this.stars);

            await page.Connect();
        }

        public int Highscore { get { return Preferences.Get(HIGHSCORE, 0); } set { Preferences.Set(HIGHSCORE, value); highscoreLabel.Text = "Highscore: " + value; } }
        public string HighscoreLabel { get { return highscoreLabel.Text; } set { highscoreLabel.Text = value; } }

        public bool IsUltra => true;

        
    }
}