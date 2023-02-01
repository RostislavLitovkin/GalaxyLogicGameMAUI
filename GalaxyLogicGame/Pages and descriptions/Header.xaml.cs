using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Pages_and_descriptions
{

    public partial class Header : AbsoluteLayout
    {
        public Header()
        {
            InitializeComponent();
            this.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { await Navigation.PopAsync(); }) });

            if (Functions.IsSquareScreen())
            {
                HeightRequest = 140;
                AbsoluteLayout.SetLayoutBounds(this, new Rect(0, 0, 360, 140));

                AbsoluteLayout.SetLayoutBounds(backButton, new Rect(0, 80, 360, 60));
                backButton.FontSize = 60;
            }
            else if (DevicePlatform.Android == DeviceInfo.Platform)
            {
                HeightRequest = 110;
                AbsoluteLayout.SetLayoutBounds(this, new Rect(0, 0, 360, 110));
                AbsoluteLayout.SetLayoutBounds(title, new Rect(0.5, 0.7, 260, 40));
                AbsoluteLayout.SetLayoutBounds(backButton, new Rect(0, 50, 360, 60));
            }
        }
        public bool InvertColors { set
            {
                if (value)
                {
                    backButton.TextColor = Color.FromArgb("000");
                    title.TextColor = Color.FromArgb("000");
                    smallTitle.TextColor = Color.FromArgb("000");
                }
            } }
        public string TitleText { set { title.Text = value; } }
        public string SmallTitleText { set { smallTitle.Text = value; smallTitle.IsVisible = true;
                if (!Functions.IsSquareScreen()) {
                    if (DevicePlatform.Android == DeviceInfo.Platform)
                    {
                        AbsoluteLayout.SetLayoutBounds(title, new Rect(0.5, 0.55, 260, 40));
                        AbsoluteLayout.SetLayoutBounds(smallTitle, new Rect(0.5, 0.9, 260, 20));
                        AbsoluteLayout.SetLayoutBounds(backButton, new Rect(0, 70, 360, 60));
                    }
                    else
                    {
                        AbsoluteLayout.SetLayoutBounds(title, new Rect(0.5, 0.2, 260, 40));
                    }
                }
            } }
        public Color TitleColor { set { title.TextColor = value; } }
        //public Color TitleColor { set { title.TextColor = value; } }
    }
}