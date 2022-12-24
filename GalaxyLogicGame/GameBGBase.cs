using GalaxyLogicGame.Powerups;

namespace GalaxyLogicGame
{
    public class GameBGBase : ContentPage
    {
        // all powerups
        public IPowerup[] AllPowerups =
        {
            new LightUpPowerup(),
            new Telescope(),
            new AtomicBomb()
        };
    }
}
