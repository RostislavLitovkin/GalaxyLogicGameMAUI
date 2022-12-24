namespace GalaxyLogicGame.Events;

public partial class LightUpEvent : AbsoluteLayout
{
	public LightUpEvent()
	{
		InitializeComponent();
	}

    public async Task Appear(IGameBG bg)
    {
        Random random = new Random();

        bg.EventLayout.IsVisible = true;
        bg.EventLayout.Children.Add(this);

        await Functions.EventTitleAnimation(eventTitle, eventIcon, darken);
        await Task.Delay(500);
        bg.EventLayout.Children.Remove(this);
        bg.EventLayout.IsVisible = false;
    }
}
