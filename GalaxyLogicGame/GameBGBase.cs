using GalaxyLogicGame.Powerups;

namespace GalaxyLogicGame
{
    public class GameBGBase : ContentPage
    {
        // all powerups
        public IPowerup[] AllPowerups =
        {
            new KindlePowerup(),
            new Telescope(),
            new AtomicBomb()
        };
    }
}
