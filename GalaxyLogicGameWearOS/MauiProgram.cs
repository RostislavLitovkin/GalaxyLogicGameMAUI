using CommunityToolkit.Maui;
using GalaxyLogicGame.Mobile;

namespace GalaxyLogicGameWearOS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("samsungone700.ttf", "SamsungOne");
                    fonts.AddFont("bignoodletitling.ttf", "BigNoodleTitling");
                })
                .UseMauiCommunityToolkit(); ;

            return builder.Build();
        }
    }
}