
using GalaxyLogicGame.Events;
using GalaxyLogicGame.Particles;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Util;
using Nethereum.Web3;
using Newtonsoft.Json;
using System.Text;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Desktop;
using WalletConnectSharp.NEthereum;

namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class CryptoConnectPage : ContentPage
{
    private StarsParticlesLayout stars;
    private Web3 web3;
    private string account;
    private const string contractAddress = "0xA6139A9Bf36c7b3cEA9D6E70eEACa49a3156A402";
    private const string providerAddress = "https://optimism-kovan.infura.io/v3/d55a983e95774bd9abbaeb2ba7e8f275";
    private bool clicked = true;
    private WalletPreview wallet;
    public CryptoConnectPage(WalletPreview wallet)
    {

        NavigationPage.SetHasNavigationBar(this, false);

        InitializeComponent();

        this.wallet = wallet;

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


        if (Preferences.ContainsKey("pubKey"))
        {
            try
            {
                Web3 fakeWeb3 = new Web3(providerAddress);

                var service = fakeWeb3.Eth.ERC721.GetContractService(contractAddress);
                var balance = await service.BalanceOfQueryAsync(Preferences.Get("pubKey", ""));
                if (balance > 0)
                {
                    nftLayout.Children.Add(new AtomicBombEvent().GetEventDescription);

                    //link.Source = "happygirl.gif";
                    link.IsVisible = false;

                    return;
                }
            }
            catch
            {
                if (nftLayout.Children.Count > 0) nftLayout.Children.Clear();
                link.IsVisible = true;
                link.Source = "nointernet.png";
            }
        }

        await ConnectWallet();
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

    }


    public async Task ConnectWallet()
    {
        try
        {
            logOutButton.IsVisible = false;

            var metadata = new ClientMeta()
            {
                Description = "This is a test of the Nethereum.WalletConnect feature",
                Icons = new[] { "https://rostislavlitovkin.pythonanywhere.com/logo" },
                Name = "WalletConnect Test",
                URL = "http://rostislavlitovkin.pythonanywhere.com/"
            };

            var walletConnect = new WalletConnect(metadata);

            //linkLabel.Text = walletConnect.URI; // good for debugging

            string fileName = walletConnect.URI.Substring(3, walletConnect.URI.IndexOf("@") - 3);


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


            //Console.WriteLine("The account " + walletConnect.Accounts[0] + " has connected!");
            account = walletConnect.Accounts[0];
            Preferences.Set("pubKey", account);
            wallet.Load();

            link.Source = "checkingnfts.png";

            linkLabel.Text = "Connected to: " + account.Substring(0, 6) + "..";

            logOutButton.IsVisible = true;

            //link.Source = "randomanimegirl.png";




            web3 = new Web3(walletConnect.CreateProvider(new Uri(providerAddress)));

            //var balance = await web3.Eth.GetBalance.SendRequestAsync(account);

            await CheckNFTBalance();
        }
        catch
        {
            if(nftLayout.Children.Count > 0) nftLayout.Children.Clear();
            link.IsVisible = true;
            link.Source = "nointernet.png";
        }
    }


    private async Task CheckNFTBalance()
    {
        var service = web3.Eth.ERC721.GetContractService(contractAddress);
        var balance = await service.BalanceOfQueryAsync(account);

        //linkLabel.TextColor = Color.FromArgb("00008B");
        linkLabel.Text = "You own: " + balance.ToString() + " NFT powerup";
        if (balance > 0)
        {

            nftLayout.Children.Add(new AtomicBombEvent().GetEventDescription);

            //link.Source = "happygirl.gif";
            link.IsVisible = false;
        }
        else
        {
            link.Source = "sadgirl.gif";
            singTransactionButton.IsVisible = true;
        }
    }

    private async void SingTransaction(object sender, EventArgs e)
    {
        /*
        var receiverAddress = "0x421Bb73746D5d8CBAba82a342d06246F8c570509";

        var transaction = await web3.Eth.GetEtherTransferService()
                .TransferEtherAsync(receiverAddress, .01m);

        linkLabel.Text = transaction;
        */
        singTransactionButton.IsVisible = false;
        if (clicked)
        {
            clicked = false;

            await MintNFT();

            clicked = true;
        }

    }
    public async Task MintNFT()
    {
        /*var mintFunctionMessage = new MintFunction()
        {
            Uri = "http://rostislavlitovkin.pythonanywhere.com/tGLG",
        };*/
        //var balanceHandler = web3.Eth.GetContractQueryHandler<MintFunction>();
        //await balanceHandler.QueryAsync<IFunctionOutputDTO>(account, mintFunctionMessage);

        /*var abi = @"function mint(string memory _uri) public payable returns(bool)";
        var contract = web3.Eth.GetContract(abi, contractAddress);
        var result = await contract.GetFunction("mint").SendTransactionAsync(contractAddress, "http://rostislavlitovkin.pythonanywhere.com/tGLG");  //.CallAsync<bool>("http://rostislavlitovkin.pythonanywhere.com/tGLG", account);
        */


        try
        {
            link.Source = "checkyourwallet.png";
            var transferHandler = web3.Eth.GetContractTransactionHandler<MintFunction>();
            var mint = new MintFunction()
            {
                Uri = "https://rostislavlitovkin.pythonanywhere.com/tGLG",
                Address = account
            };

            //var estimate = await transferHandler.EstimateGasAsync(contractAddress, transfer);
            //transfer.Gas = estimate.Value;
            //transfer.GasPrice = Nethereum.Web3.Web3.Convert.ToWei(25, UnitConversion.EthUnit.Gwei);

            var index = await transferHandler.SendRequestAsync(contractAddress, mint);
            if (/*index != -1 ||*/ true)
            {
                //linkLabel.TextColor = Color.FromArgb("ffffff");
                linkLabel.Text = "NFT minted!";


                // QR code
                var person = new PostData { URI = "https://kovan-optimistic.etherscan.io/tx/" + index, FileName = index };

                var json = JsonConvert.SerializeObject(person);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "http://rostislavlitovkin.pythonanywhere.com/GenerateQR";
                using var client = new HttpClient();

                await client.PostAsync(url, data);

                var img = await client.GetAsync("http://rostislavlitovkin.pythonanywhere.com/QR/" + index);

                link.Source = "http://rostislavlitovkin.pythonanywhere.com/QR/" + index;


            }
        }
        catch
        {
            singTransactionButton.IsVisible = true;
        }

    }

    [Function("mint", "int")]
    public class MintFunction : FunctionMessage
    {
        [Parameter("string", "_uri", 1)]
        public string Uri { get; set; }

        [Parameter("address", "_account", 2)]
        public string Address { get; set; }
    }

    public class PostData
    {
        public string URI { get; set; }
        public string FileName { get; set; }

    }

    private async void OnLogOutClicked(object sender, EventArgs e)
    {
        Preferences.Remove("pubKey");

        if (nftLayout.Children.Count > 0) nftLayout.Children.Clear();
        link.IsVisible = true;
        await ConnectWallet();
    }
}