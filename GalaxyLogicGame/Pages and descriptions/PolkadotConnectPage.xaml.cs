using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Pages_and_descriptions;
using GalaxyLogicGame.Events;
using Plutonication;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using CommunityToolkit.Maui.Alerts;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace GalaxyLogicGame.Pagesanddescriptions;

public partial class PolkadotConnectPage : ContentPage
{
    private StarsParticlesLayout stars;
    PlutoEventManager manager = new PlutoEventManager();
    private string account;
    private readonly string contractAddress = EthFunctions.GetAtomicBombContractAddress;
    private readonly string providerAddress = EthFunctions.GetEthereumProvider;
    private bool clicked = true;
    private WalletPreview wallet;
    private bool walletConnectClicked = false;
    public PolkadotConnectPage(WalletPreview wallet)
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
    public void AssignStars(Particles.StarsParticlesLayout stars)
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

    public async Task Connect()
    {
        if (Preferences.ContainsKey("pubKey") && false)
        {
            try
            {
                if (await CheckNFTs())
                {
                    await ShowNFTs();
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                NoInternetError();
            }
        }

        else await OfferConnectingNewWallet();
    }
    private async Task<bool> CheckNFTs()
    {
        ClearMainStackLayout();
        mainStackLayout.Children.Add(new Image
        {
            Source = "checkingnfts.png",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 270,
            HeightRequest = 270
        });
        return await EthFunctions.CheckAtomicBombNFTOwnership(Preferences.Get("pubKey", ""));
    }
    private async Task OfferConnectingNewWallet()
    {
        ClearMainStackLayout();
        try
        {
            AccessCredentials ac = new AccessCredentials(
                address: IPAddress.Parse("192.168.177.143"),
                port: 8080,
                key: "samplePassword",
                icon: "https://rostislavlitovkin.pythonanywhere.com/logo",
                name: "Galaxy Logic Game"
            );

            ac.Address = TestIpFinding();

            qrLayout.IsVisible = true;

            qrLayout.Children.Add(new ZXing.Net.Maui.Controls.BarcodeGeneratorView
            {
                Format = ZXing.Net.Maui.BarcodeFormat.QrCode,
                Value = ac.ToUri().ToString(),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 250,
                HeightRequest = 250
            });


            await Task.Delay(500);
            Console.WriteLine("QR?");

            manager.ListenSafeAsync(ac.Key, ac.Port).GetAwaiter().GetResult();

            qrLayout.IsVisible = false;


            manager.ConnectionEstablished += () =>
            {
                Console.WriteLine("Connectin Established! :)");
            };

            manager.ConnectionRefused += () =>
            {
                Console.WriteLine("Connectin Refused! :(");
            };


            manager.MessageReceived += () => {
                Console.WriteLine("message received!");

                // Pop oldest message from message queue
                PlutoMessage msg = manager.IncomingMessages.Dequeue();

                // Based on MessageCode process you message
                switch (msg.Identifier)
                {
                    case MessageCode.Success:
                        Console.WriteLine("Code: '{0}'. public key delivered!", msg.Identifier);
                        break;
                    case MessageCode.PublicKey:
                        Preferences.Set("pubKey", msg.CustomDataToString());
                        wallet.Load();
                        break;

                    default:
                        Console.WriteLine("Unknown code: " + msg.Identifier);
                        break;
                }
            };


            //linkLabel.Text = walletConnect.URI; // good for debugging
            if (Functions.IsSquareScreen() || walletConnectClicked || true)
            {
                
                await manager.SetupReceiveLoopAsync();
            }
            else
            {
                try
                {
                    Uri uri = ac.ToUri();
                    await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception ex)
                {
                    OfferDownloadingWallet();
                }
            }
            /*
            await Task.Delay(2000);
            

            mainStackLayout.Children.Add(new Image
            {
                Source = "checkingnfts.png",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 270,
                HeightRequest = 270
            });
            //AddLabelToStackLayout("Connected to: " + account.Substring(0, 6) + "..");


            AddLogOutButton();

            if (await CheckNFTs())
            {
                await ShowNFTs();
            }
            else
            {
                OfferMinting();
            }
            */
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            NoInternetError();
        }
    }

    public static IPAddress GetMyIpAddress()
    {
        string hostName = Dns.GetHostName();
        IPAddress[] ipAddresses = Dns.GetHostEntry(hostName).AddressList;
        var ip = ipAddresses.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Where(x =>
        {
            var nums = x.ToString().Split(".");
            int first = Int32.Parse(nums[0]);
            int second = Int32.Parse(nums[1]);
            return (
                first == 192 && second == 168)
            || (first == 172 && ((second >= 16) && (second <= 31))
            );
        }).FirstOrDefault();
        return ip;
    }

    private async Task OfferDownloadingWallet()
    {
        mainStackLayout.Children.Add(new WalletDownloadThumbnail
        {
            Icon = "metamask.png",
            Title = "Metamask",
            Description = "The most popular Ethereum wallet",
            Link = "https://play.google.com/store/apps/details?id=io.metamask",
            ConnectWalletMethod = OfferConnectingNewWallet,
        });

        mainStackLayout.Children.Add(new WalletDownloadThumbnail
        {
            Icon = "",
            Title = "Wallet connect",
            Description = "Universal way to connect all Ethereum wallets",
            ConnectWalletMethod = ShowWalletConnectQRCode,
        });
        /*
       var walletConnectThumbnail = new WalletDownloadThumbnail
       {
           Icon = "walletconnecticon.png",
           Title = "Wallet connect",
           Description = "Connect your wallet securely via QR code",
           ConnectWalletMethod = OfferConnectingNewWallet,
       };

       walletConnectThumbnail.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => {
           walletConnectClicked = true;
           await OfferConnectingNewWallet();
       }) });
       mainStackLayout.Children.Add(walletConnectThumbnail);
        */


    }
    private void OfferMinting()
    {
        ClearMainStackLayout();

        BuyPowerupTemplate template = new BuyPowerupTemplate();
        template.NetworkLabel.TextColor = Color.FromArgb("ff0000");
        template.NetworkLabel.Text = "Optimism testnet";

        template.PriceLabel.Text = "Mint for 0 Eth";
        template.PriceLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { await OnMintClicked(); }) });

        template.AddPowerupDescription(new AtomicBombEvent().GetEventDescription);

        mainStackLayout.Children.Add(template);
    }
    private async Task ShowNFTs()
    {
        ClearMainStackLayout();

        TokenInfoTemplate template = new TokenInfoTemplate();
        template.ContractAddressLabel.Text = contractAddress.Substring(0, 20) + "..";
        template.ContractAddressLabel.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(async () => {
                if (Functions.IsSquareScreen()) { qrCodeLayout.IsVisible = true; await qrCodeLayout.FadeTo(1, 500); }
                else if (template.ContractAddressLabel.Text != "Copied to clipboard")
                {
                    await Clipboard.Default.SetTextAsync(contractAddress);

                    template.ContractAddressLabel.Text = "Copied to clipboard";

                    var toast = Toast.Make("Copied to clipboard");
                    await toast.Show();

                    template.ContractAddressLabel.Text = contractAddress.Substring(0, 20) + "..";
                }
            })
        });

        string[] address = { contractAddress };
        try
        {
            //var thing = await tempWeb3.Eth.ERC721.GetContractService(contractAddress).Owner; //tempWeb3.Eth.ERC721.GetAllTokenIdsOfOwnerUsingTokenOfOwnerByIndexAndMultiCallAsync(Preferences.Get("pubKey", "Failed"), address);
            //template.TokenIdLabel.Text = "Token ID: " + thing[0];
        }
        catch (Exception ex)
        {
            Console.WriteLine("Token ID not loaded");
            Console.WriteLine(ex.Message);
            template.TokenIdLabel.Text = "Token ID not loaded";
            template.TokenIdLabel.TextColor = Color.FromArgb("bbb");
        }

        template.AddPowerupDescription(new AtomicBombEvent().GetEventDescription);
        mainStackLayout.Children.Add(template);

        mainStackLayout.Children.Add(new BoxView { HeightRequest = 10 });

        AddLogOutButton();
    }
    private void NoInternetError()
    {
        ClearMainStackLayout();
        mainStackLayout.Children.Add(new Image
        {
            Source = "nointernet.png",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 270,
            HeightRequest = 270
        });
    }
    private void ClearMainStackLayout()
    {
        if (mainStackLayout.Children.Count > 0) mainStackLayout.Children.Clear();
    }
    private void AddLabelToStackLayout(string txt)
    {
        mainStackLayout.Children.Add(new Label
        {
            Text = txt,
            HorizontalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 270,
            FontSize = 40
        });
    }
    private void AddLogOutButton()
    {
        Label logOutButton = new Label
        {
            Text = "Log out",
            TextColor = Color.FromArgb("FF8B0000"),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold,
            FontFamily = "BigNoodleTitling",
            FontSize = 40
        };
        logOutButton.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { await OnLogOutClicked(); }) });
        mainStackLayout.Children.Add(logOutButton);
    }
    private async Task OnMintClicked()
    {
        ClearMainStackLayout();
        if (clicked)
        {
            clicked = false;

            await MintNFT();

            clicked = true;
        }
    }
    public async Task MintNFT()
    {

    }
    
    public class PostData
    {
        public string URI { get; set; }
        public string FileName { get; set; }

    }

    private async Task OnLogOutClicked()
    {
        Preferences.Remove("pubKey");

        ClearMainStackLayout();

        await wallet.Load();
        await Connect();
    }
    
    private async Task ShowWalletConnectQRCode()
    {
        walletConnectClicked = true;

        await OfferConnectingNewWallet();
    }

    private async void OnQRCodeLayoutClicked(object sender, EventArgs e)
    {
        await qrCodeLayout.FadeTo(0, 500);
        qrCodeLayout.IsVisible = false;
    }

    private string TestIpFinding()
    {

        var result = new List<IPAddress>();
        try
        {
            var upAndNotLoopbackNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces().Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback
                                                                                                          && n.OperationalStatus == OperationalStatus.Up);

            foreach (var networkInterface in upAndNotLoopbackNetworkInterfaces)
            {
                var iPInterfaceProperties = networkInterface.GetIPProperties();

                var unicastIpAddressInformation = iPInterfaceProperties.UnicastAddresses.FirstOrDefault(u => u.Address.AddressFamily == AddressFamily.InterNetwork);
                if (unicastIpAddressInformation == null) continue;

                result.Add(unicastIpAddressInformation.Address);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unable to find IP: {ex.Message}");
        }

        string allIpInfo = string.Empty;
        foreach (var item in result)
        {
            allIpInfo += item.ToString() + "---";
        }
        return result.FirstOrDefault()?.ToString();
        //Test2 = PlutoManager.GetMyIpAddress().ToString();

    }
}
