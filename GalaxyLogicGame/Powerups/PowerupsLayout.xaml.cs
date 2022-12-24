namespace GalaxyLogicGame.Powerups;

public partial class PowerupsLayout : StackLayout
{
	public PowerupsLayout()
	{
		InitializeComponent();

		if (Functions.IsSquareScreen())
		{

        }
		else
		{
			TranslationY = 270;
        }
	}
}