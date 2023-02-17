using CommunityToolkit.Maui;
using GalaxyLogicGame.Mobile;
using ZXing.Net.Maui;

namespace GalaxyLogicGameMAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseBarcodeReader()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("SamsungOne700.ttf", "SamsungOne");
				fonts.AddFont("bignoodletitling.ttf", "BigNoodleTitling");

            })
            .UseMauiCommunityToolkit();

		return builder.Build();
	}
}
