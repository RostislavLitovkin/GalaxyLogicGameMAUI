using System;
using System.Collections.Generic;
using System.Text;
using GalaxyLogicGame.Events;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Powerups
{
    class AtomicBomb : PowerupBase, IPowerup
    {
        public AtomicBomb()
        {
            //BGColor = Color.LightGreen;
            Cooldown = 40;
            Icon = "atomicBombFrame.png";
        }
        public void Prerequisites()
        {
            IsAllowed = BG.Game.Atoms.Count > 1;
        }

        public override async void UsePowerupClicked()
        {
            AtomicBombEvent atomicBombEvent = new AtomicBombEvent();
            await atomicBombEvent.Appear(BG.Game);

            Prerequisites();
        }
        public override void SeePowerupDetailsClicked()
        {
            if (BG.Game.Atoms.Count  <= 1) FullscreenTitlePopup.Appear(BG.MainLayout, "You need to have at least 2 planets", Color.FromArgb("fff"), 0.5);
            // add details page
        }

        public bool Equiped { get => Preferences.Get("org.tizen.myApp.challenge11", false); set { Preferences.Set("atomicBombEquiped", value); } } // change to false
        public bool Owned { get => Preferences.Get("atomicBombOwned", true); set { Preferences.Set("atomicBombOwned", value); } } // change to false
    }
}
