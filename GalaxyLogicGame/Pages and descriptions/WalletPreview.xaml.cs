namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class WalletPreview : AbsoluteLayout
{
    private bool owns = false;

    public WalletPreview()
	{
		InitializeComponent();
		Load();
	}
	public async Task Load()
    {
        if (Preferences.ContainsKey("pubKey"))
        {
            try
            {
                label.Text = "Checking NFTs";
                //if (await EthFunctions.CheckAtomicBombNFTOwnership(Preferences.Get("pubKey", "Failed")))
                if (true)
                {
                    label.Text = Text;
                    owns = true;
                }
                else
                {
                    owns = false;
                    label.Text = "Connect wallet";
                }

            }
            catch
            {
                owns = false;
                label.Text = Text;
            }
        }
        else { label.Text = "Connect wallet"; owns = false; }
    }

    private string Text => "Connected to: \n" + Preferences.Get("pubKey", "Failed").Substring(0, 6) + "..";

    public bool Owns => owns;
}