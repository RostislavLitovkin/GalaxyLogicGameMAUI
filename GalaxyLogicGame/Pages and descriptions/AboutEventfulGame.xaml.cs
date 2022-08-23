﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Events;
using GalaxyLogicGame.Pages_and_descriptions;
using GalaxyLogicGame.Particles;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame
{

    public partial class AboutEventfulGame : ContentPage, IStarsPage
    {
        private StarsParticlesLayout stars;
        public AboutEventfulGame()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            Setup();
            Functions.ScaleToScreen(this, scaleLayout);
            Functions.FillHeight(scaleLayout);
            SizeChanged += OnDisplaySizeChanged;

        }
        public AboutEventfulGame(StarsParticlesLayout stars)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            Setup();
            Functions.ScaleToScreen(this, scaleLayout);
            Functions.FillHeight(scaleLayout);
            SizeChanged += OnDisplaySizeChanged;

            this.stars = stars;
            starsLayout.Children.Add(stars);
        }

        public void AssingStars(StarsParticlesLayout stars)
        {
            this.stars = stars;
            starsLayout.Children.Add(stars);
        }
        private void OnDisplaySizeChanged(object sender, EventArgs args)
        {
            Functions.ScaleToScreen(this, scaleLayout);
            Functions.FillHeight(scaleLayout);
        }
        private void Setup()
        {
            TapGestureRecognizer binaryTapGesture = new TapGestureRecognizer();
            binaryTapGesture.Tapped += OnBinaryInfoClicked;
            TapGestureRecognizer dreamsTapGesture = new TapGestureRecognizer();
            dreamsTapGesture.Tapped += OnDreamsInfoClicked;



            stackLayout.Children.Add(new ShootingStarEvent().GetEventDescription);
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new BlindnessEvent().GetEventDescription);
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new ThreeInRowEvent().GetEventDescription);
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new Polymorph().GetEventDescription);
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new AbsoluteLayout
            {
                WidthRequest = 320, HeightRequest = 125, HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    GenerateBackground(),
                    new StackLayout{
                        Children = {
                            new BinaryEvent().GetEventDescription
                        }
                    },
                    GenerateMoreInfoLayout()
                },
                GestureRecognizers =
                {
                    binaryTapGesture
                }
            });
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new AstronautEvent().GetEventDescription);
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new AbsoluteLayout
            {
                WidthRequest = 320, HeightRequest = 125, HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    GenerateBackground(),
                    new StackLayout {
                        Children = {
                            new DreamsEvent().GetEventDescription
                        }
                    },
                    GenerateMoreInfoLayout()
                },
                GestureRecognizers =
                {
                    dreamsTapGesture
                }
            });
            //stackLayout.Children.Add(new AtomicBombEvent().GetEventDescription);
            //stackLayout.Children.Add(new ChristmasEvent().GetEventDescription);
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new WorldUpsideDownEvent().GetEventDescription);
            stackLayout.Children.Add(new BoxView { HeightRequest = 10 });

            stackLayout.Children.Add(new BlueberriesEvent().GetEventDescription);

            stackLayout.Children.Add(new BoxView { HeightRequest = 120 });

        }

        public async Task TransitionIn()
        {
            await Task.WhenAll(
                wallpaper.TranslateTo(0, 0, 500, Easing.SinOut),
                wallpaper.FadeTo(1, 500, Easing.SinOut),
                stars.TransitionUpOut(),
                mainLayout.TranslateTo(0, 0, 500, Easing.SinOut),
                mainLayout.FadeTo(1, 500));

            starsLayout.Children.Clear();
        }

        private Button GenerateBackground()
        {
            Button background = new Button
            {
                BackgroundColor = Color.FromHex("1f1f1f"),
                CornerRadius = 30,
            };
            //BoxView background = new BoxView { BackgroundColor = Color.FromHex("1f1f1f"), CornerRadius = 30 };
            AbsoluteLayout.SetLayoutBounds(background, new Rect(0.5, 0.5, 1, 1));
            AbsoluteLayout.SetLayoutFlags(background, AbsoluteLayoutFlags.All);

            return background;
        }
        private AbsoluteLayout GenerateMoreInfoLayout()
        {
            Label moreInfoLabel = new Label
            {
                VerticalTextAlignment = TextAlignment.Center, FontSize = 14, FontFamily = "SamsungOne", TextColor = Color.FromArgb("fff"), FontAttributes = FontAttributes.Bold, Text = "Click here for more info"
            };
            AbsoluteLayout.SetLayoutBounds(moreInfoLabel, new Rect(20, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(moreInfoLabel, AbsoluteLayoutFlags.YProportional);

            Image moreInfoImage = new Image
            {
                Source = "info.png", Aspect = Aspect.AspectFit
            };
            AbsoluteLayout.SetLayoutBounds(moreInfoImage, new Rect(280, 10, 20, 20));


            AbsoluteLayout moreInfoLayout = new AbsoluteLayout
            {
                Children =
                {
                    moreInfoLabel,
                    moreInfoImage
                }
            };
            AbsoluteLayout.SetLayoutBounds(moreInfoLayout, new Rect(0, 85, 320, 40));
            return moreInfoLayout;
        }
        private async void OnBinaryInfoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BinaryInfoPage());
        }

        private async void OnDreamsInfoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DreamsInfoPage());
        }
    }
}