namespace GalaxyLogicGame.Powerups;

public partial class PowerupsWatchLayout : AbsoluteLayout
{
    private bool clicked = true;
    private Layout layout;
    private List<PowerupBase> powerups;

    public PowerupsWatchLayout()
	{
		InitializeComponent();
	}

    private async void OnHideClicked(System.Object sender, System.EventArgs e)
    {
        await Hide();
    }

    private async Task Hide()
    {
        if (clicked)
        {
            clicked = false;
            protectiveLayout.IsVisible = true;

            await this.FadeTo(0, 500);

            for (int i = 0; i < powerups.Count; i++)
            {
                powerupsLayout.Children.Remove(powerups[i]);
            }

            layout.Children.Remove(this);

            protectiveLayout.IsVisible = false;

            clicked = true;
        }
    }

    public async Task Appear(Layout layout, List<PowerupBase> powerups)
    {
        if (clicked)
        {
            clicked = false;
            protectiveLayout.IsVisible = true;

            this.layout = layout;
            this.powerups = powerups;

            for (int i = 0; i < powerups.Count; i++)
            {
                powerupsLayout.Children.Add(powerups[i]);
                powerups[i].HideWatchLayoutFunction = Hide;
            }

            layout.Children.Add(this);
            await this.FadeTo(1, 500);

            protectiveLayout.IsVisible = false;
            clicked = true;
        }
    }
}
