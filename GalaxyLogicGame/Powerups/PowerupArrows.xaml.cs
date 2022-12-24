namespace GalaxyLogicGame.Powerups;

public partial class PowerupArrows : AbsoluteLayout
{
    private bool isVisible = false;

    public PowerupArrows()
	{
		InitializeComponent();
        if (!Functions.IsSquareScreen())
        {
            this.IsVisible = false;
        }
	}

    public async Task Play()
    {
        arrow1.TranslationY = 40;
        arrow2.TranslationY = 40;
        arrow1.Opacity = 0;
        arrow2.Opacity = 0;


        await Task.WhenAll(
            arrow1.TranslateTo(0, -10, 500, Easing.SinOut),
            arrow1.FadeTo(1, 100));
        await Task.WhenAll(
            arrow2.TranslateTo(0, 10, 500, Easing.SinOut),
            arrow2.FadeTo(1, 100));
        await Task.Delay(1000);
        await Task.WhenAll(
            arrow1.FadeTo(0, 500),
            arrow2.FadeTo(0, 500));
        if (isVisible) await Play();
    }
    public bool HasAppeared { get => isVisible; set { isVisible = value; } }
}
