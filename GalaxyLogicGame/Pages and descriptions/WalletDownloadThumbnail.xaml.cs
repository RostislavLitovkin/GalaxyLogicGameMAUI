namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class WalletDownloadThumbnail : AbsoluteLayout
{
	public WalletDownloadThumbnail()
	{
		InitializeComponent();
	}

    public Color TitleColor { set { title.TextColor = value; } }
    public string Title { set { title.Text = value; } get { return title.Text; } }
    public string Description { set { description.Text = value; } get { return description.Text; } }
    public string Icon { set { icon.Source = value; } }
    public ImageSource IconImageSource { set { icon.Source = value; } }
}