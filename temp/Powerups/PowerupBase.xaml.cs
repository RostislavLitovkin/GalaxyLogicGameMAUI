using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Powerups
{

    public abstract partial class PowerupBase : AbsoluteLayout
    {
        private int cooldown;
        private IGameBG bg;
        private IPowerupView powerupView;
        private int lastTurn = 0;
        private bool clicked = true;
        public PowerupBase()
        {
            InitializeComponent();
        }

        public int Cooldown { set { cooldown = value; lastTurn = -value; } }
        public void UpdateCooldown()
        {
            if (bg.Game.Turn - lastTurn < cooldown)
            {
                int height = (int)((double)(bg.Game.Turn - lastTurn) / cooldown * 170);
                int width = 0;
                if (height < 30)
                {
                    width = 30 - height;
                }
                AbsoluteLayout.SetLayoutBounds(filledBG, new Rect(5 + width/2, 175 - height, 170 - width, height));
            }
            else AbsoluteLayout.SetLayoutBounds(filledBG, new Rect(5, 5, 170, 170));
        }

        public Color BGColor { set { filledBG.BackgroundColor = value; } }
        public string Icon { set { icon.Source = value; } }

        public bool IsAllowed { get { return !allowed.IsVisible; } set { allowed.IsVisible = !value; protectiveLayer.IsVisible = !value; } }
        public IGameBG BG { get => bg; set { bg = value; } }
        public IPowerupView PowerupView { set { powerupView = value; } }

        public int LastTurn { get => lastTurn; set { lastTurn = value; } }
        public abstract void UsePowerupClicked();
        public abstract void SeePowerupDetailsClicked();
        
        public async void OnUsePowerupClicked(object sender, EventArgs args)
        {
            if (bg.Game.Turn - lastTurn >= cooldown && IsAllowed && clicked)
            {
                clicked = false;
                uint duration = 750;
                int yOffset = 80;
                //bg.PowerUpAnimationLayout.IsVisible = true;
                //bg.PowerUpAnimationLayout.Children.Add(this);


                // maybe a animation here
                BoxView protectiveLayer = new BoxView();

                if (powerupView != null) powerupView.Disappear();
                bg.MainLayout.Children.Add(this);
                this.TranslationY = yOffset;
                await Task.WhenAll(
                    this.TranslateTo(0, yOffset-80, duration, Easing.SinInOut),
                    this.ScaleTo(1.2, duration, Easing.SinOut)
                    /*,Task.Run(async() => {
                        await this.ScaleYTo(0.9, duration/2, Easing.SinIn);
                        await this.ScaleYTo(1, duration/2, Easing.SinOut);
                    })*/);
                await Task.Delay(100);
                await Task.WhenAll(
                    //this.TranslateTo(0, yOffset-50, duration/2, Easing.SinIn),
                    Task.Run(async() => { await this.ScaleTo(1, 100, Easing.SinOut); await this.ScaleTo(2.5, duration / 2 - 100, Easing.SinIn); }),
                    this.FadeTo(0, duration/2, Easing.SinIn));

                //bg.PowerUpAnimationLayout.IsVisible = false;
                //bg.PowerUpAnimationLayout.Children.Remove(this);
                bg.MainLayout.Children.Remove(this);


                this.Opacity = 1;
                this.Scale = 1;
                //this.TranslationX = 0; //not needed
                this.TranslationY = 0;
                
                //await Task.Delay(1000);
                // needs major improvements



                

                UsePowerupClicked();
                lastTurn = BG.Game.Turn;
                UpdateCooldown();

                clicked = true;
            }
            else if (clicked)
            {
                //await Task.WhenAll(
                //    this.TranslateTo(0, -100, 750, Easing.CubicIn),
                //   this.ScaleTo(1.2, 750, Easing.CubicIn));
                SeePowerupDetailsClicked();
                // special animation
            }
        }
    }
}