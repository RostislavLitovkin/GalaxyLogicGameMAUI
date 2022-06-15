namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class WalletPreview : AbsoluteLayout
{
	public WalletPreview()
	{
		InitializeComponent();
		Load();
	}
	public void Load()
    {
		if (Preferences.ContainsKey("pubKey")) label.Text = Preferences.Get("pubKey", "Failed").Substring(0, 6) + "..";
    }
}