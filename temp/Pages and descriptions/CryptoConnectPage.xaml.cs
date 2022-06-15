
using GalaxyLogicGame.Particles;
using Newtonsoft.Json;
using System.Text;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Desktop;

namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class CryptoConnectPage : ContentPage
{
    private StarsParticlesLayout stars;
    public CryptoConnectPage()
    {
        NavigationPage.SetHasNavigationBar(this, false);

        InitializeComponent();


        Functions.ScaleToScreen(this, scaleLayout);
        Functions.FillHeight(scaleLayout);

        SizeChanged += OnDisplaySizeChanged;
    }

    private void OnDisplaySizeChanged(object sender, EventArgs args)
    {
        Functions.ScaleToScreen(this, scaleLayout);
        Functions.FillHeight(scaleLayout);
    }
    public void AssignStars(StarsParticlesLayout stars)
    {
        this.stars = stars;
        starsLayout.Children.Add(stars);
    }

    public async Task TransitionIn()
    {
        await Task.WhenAll(
            wallpaper.TranslateTo(0, 0, 500, Easing.SinOut),
            wallpaper.FadeTo(1, 500, Easing.SinOut),
            stars.TransitionUpOut(),
            mainLayout.TranslateTo(0, 0, 500, Easing.SinOut),
            mainLayout.FadeTo(1, 500));

        starsLayout.Children.Clear();
    }

    private async void OnEthereumButtonClicked(object sender, EventArgs e)
    {
        /*
        ((Button)sender).BackgroundColor = Color.Orange;

        //string words = "legal junk salute shiver start fluid carbon afford stereo topple move system"; // Change later
        string words = wordPhraseEditor.Text;

        var web3 = new Web3("https://rinkeby.infura.io/v3/bd40a02e30d94feb94ac66df5304cd7a");
        var password = "";
        var wallet = new Wallet(words, password);

        var chainId = 4;
        var account = wallet.GetAccount(0, chainId);


        adressLabel.Text = "PubKey: " + account.Address;

        var balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);

        //var balance = await web3.Eth.GetBalance.SendRequestAsync("0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae");

        ((Button)sender).Text = Web3.Convert.FromWei(balance).ToString() + " Ether";
        ((Button)sender).BackgroundColor = Color.Green;
        */
    }

    private async void OnWalletButtonClicked(object sender, EventArgs e)
    {

        if (((Button)sender).BackgroundColor == Color.FromArgb("FF00008B")) return;
        ((Button)sender).BackgroundColor = Color.FromArgb("ffa500");
        var metadata = new ClientMeta()
        {
            Description = "This is a test of the Nethereum.WalletConnect feature",
            Icons = new[] { "None" },
            Name = "WalletConnect Test",
            URL = "http://rostislavlitovkin.pythonanywhere.com/"
        };

        var walletConnect = new WalletConnect(metadata);

        linkLabel.Text = walletConnect.URI;

        string fileName = walletConnect.URI.Substring(3, walletConnect.URI.IndexOf("@") - 3 );




        var person = new PostData { URI = walletConnect.URI, FileName = fileName };

        var json = JsonConvert.SerializeObject(person);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var url = "http://rostislavlitovkin.pythonanywhere.com/GenerateQR";
        using var client = new HttpClient();

        await client.PostAsync(url, data);

        var img = await client.GetAsync("http://rostislavlitovkin.pythonanywhere.com/QR/" + fileName);

        link.Source = "http://rostislavlitovkin.pythonanywhere.com/QR/" + fileName;



        if (walletConnect != null) try { await walletConnect.Connect(); } catch (Exception ex) { linkLabel.Text = ex.Message; }
        else linkLabel.Text = "It is null";

        ((Button)sender).BackgroundColor = Color.FromArgb("008000");


        //Console.WriteLine("The account " + walletConnect.Accounts[0] + " has connected!");
        linkLabel.Text = walletConnect.Accounts[0];

        link.Source = "randomanimegirl.png";
    }

    public class PostData
    {
        public string URI { get; set; }
        public string FileName { get; set; }

    }
}