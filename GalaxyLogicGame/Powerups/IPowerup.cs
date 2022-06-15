using System;
using System.Collections.Generic;
using System.Text;

namespace GalaxyLogicGame.Powerups
{
    public interface IPowerup
    {
        void Prerequisites();
        bool Equiped { get; set; }
        bool Owned { get; set; }
    }
}
