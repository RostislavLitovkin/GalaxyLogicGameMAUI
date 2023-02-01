using System;
using System.Collections.Generic;
using System.Text;
using GalaxyLogicGame.Events;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Powerups
{
    public class LightUpPowerup : PowerupBase, IPowerup
    {
        
        public LightUpPowerup()
        {
            //BGColor = Color.Orange;
            Cooldown = 30;

            BackgroundColor = Color.FromArgb("888");
            // more
        }

        public override void Prerequisites()
        {
            IsAllowed = BG.Game is GameWithEvents && ((GameWithEvents)BG.Game).Blindness != null;
        }

        public override async void UsePowerupClicked()
        {
            LightUpEvent lightUp = new LightUpEvent();
            await lightUp.Appear(BG);
            await ((GameWithEvents)BG.Game).Blindness.Disappear();
            
            Prerequisites();
        }
        public override void SeePowerupDetailsClicked()
        {
            if (!(BG.Game is GameWithEvents)) FullscreenTitlePopup.Appear(BG.MainLayout, "Available only in eventful game mode", Color.FromArgb("fff"), 0.5);
            else if (((GameWithEvents)BG.Game).Blindness == null) FullscreenTitlePopup.Appear(BG.MainLayout, "There is no Blindness effect", Color.FromArgb("fff"), 0.5);
            // add details page
        }

        public static bool Equiped { get => Preferences.Get("lightUpEquiped", false); set { Preferences.Set("lightUpEquiped", value); } } // change to false
        public static bool Owned { get => Preferences.Get("lightUpOwned", true); set { Preferences.Set("lightUpOwned", value); } } // change to false
    }
}
