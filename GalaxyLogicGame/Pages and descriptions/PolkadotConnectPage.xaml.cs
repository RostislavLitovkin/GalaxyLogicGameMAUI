using GalaxyLogicGame.Particles;
using GalaxyLogicGame.Pages_and_descriptions;
using GalaxyLogicGame.Events;
using Plutonication;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using CommunityToolkit.Maui.Alerts;
using Substrate.NetApi;
using ZXing.Net.Maui.Controls;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Attributes;
using Substrate.NetApi.Model.Types.Metadata.V14;
using static Substrate.NetApi.Model.Meta.Storage;
using Substrate.NetApi.Model.Types.Primitive;
using Nethereum.JsonRpc.Client;
using Newtonsoft.Json.Linq;
using Substrate.NetApi.Model.Types;

namespace GalaxyLogicGame.Pagesanddescriptions;

public partial class PolkadotConnectPage : ContentPage
{
    private StarsParticlesLayout stars;
    //private SubstrateClient substrateClient;
    private string pubkey;
    private bool clicked = true;
    private WalletPreview wallet;
    private Account account;
    private bool walletConnectClicked = false;
    //private BuyPowerupTemplate template;
    //private U32 collectionId = new U32(7);

    public PolkadotConnectPage(WalletPreview wallet)
    {
        NavigationPage.SetHasNavigationBar(this, false);

        InitializeComponent();

        this.wallet = wallet;

        Functions.ScaleToScreen(this, scaleLayout);
        Functions.FillHeight(scaleLayout);

        SizeChanged += OnDisplaySizeChanged;

        /*template = new BuyPowerupTemplate();
        template.NetworkLabel.TextColor = Color.FromArgb("ff0000");
        template.NetworkLabel.Text = "Rockmine";

        template.PriceLabel.Text = "Mint for 1 ROC";
        template.PriceLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { await OnMintClicked(); }) });

        template.AddPowerupDescription(new AtomicBombEvent().GetEventDescription);
        template.IsVisible = false;

        mainStackLayout.Children.Add(template);*/
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

    public async Task Connect()
    {
        // Create a client that connects to the RPC node
        /*substrateClient = new SubstrateClient(
            new Uri("wss://rococo-asset-hub-rpc.polkadot.io"),
            Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await substrateClient.ConnectAsync();*/

        if (Preferences.ContainsKey("pubKey"))
        {
            try
            {
                if (await CheckNFTs())
                {
                    Console.WriteLine("NFT owned ^^");
                    await ShowNFTs();
                }
                else
                {
                    OfferConnectingNewWallet();
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
        checkingNfts.IsVisible = true;

        Console.WriteLine("Checking");
        // Actual checking
        /*
        CancellationToken token = CancellationToken.None;

        var account32 = new AccountId32();
        account32.Create(Utils.GetPublicKeyFrom(Preferences.Get("pubKey", "")));

        var keyBytes = RequestGenerator.GetStorageKeyBytesHash("Nfts", "Account");

        var temp = keyBytes.Concat(HashExtension.Hash(Hasher.BlakeTwo128Concat, account32.Encode()));
        byte[] prefix = temp.Concat(HashExtension.Hash(Hasher.BlakeTwo128, collectionId.Encode())).ToArray();

        byte[] startKey = new byte[0] { };

        Console.WriteLine(Utils.Bytes2HexString(prefix));

        var keysPaged = await substrateClient.State.GetKeysPagedAsync(prefix, 1, startKey, string.Empty, token);

        return keysPaged != null && keysPaged.Any();
        */
        return false;
    }
    private async Task OfferConnectingNewWallet()
    {
        ClearMainStackLayout();

        try
        {
            // Access credentials are used to show correct info to the wallet.
            AccessCredentials ac = new AccessCredentials
            {
                Url = "wss://plutonication-acnha.ondigitalocean.app/",
                Name = "Galaxy Logic Game",
                Icon = "https://rostislavlitovkin.pythonanywhere.com/logo",
            };

            Task.Run(async () =>
            {
                this.account = await PlutonicationDAppClient.InitializeAsync(
                    ac,
                    pubkey =>
                    {
                        Preferences.Set("pubKey", pubkey);

                        // This has to run on the main thread
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            header.SmallTitleText = "Connected to: " + pubkey.Substring(0, 6) + "..";
                            checkingNfts.IsVisible = true;

                            await wallet.Load();

                            /*
                            if (await CheckNFTs())
                            {
                                await ShowNFTs();
                            }
                            else
                            {
                                OfferMinting();
                            }
                            */

                            // logOutButton.IsVisible = true;

                            // Pop async
                            await Navigation.PopAsync();
                        });
                    });
                });

            bool supportsUri = await Launcher.Default.CanOpenAsync("plutonication:");

            if (supportsUri)
            {

                bool plutonicationOpened = await Launcher.Default.TryOpenAsync(ac.ToUri());

                if (plutonicationOpened)
                {
                    return;
                }
            }
            else
            {
                qrLabel.Text = "Scan with wallet";
                qrCodeLayout.IsVisible = true;
                qrCode.Value = ac.ToUri().ToString();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            NoInternetError();
        }
    }

    // unused for now
    private async void OnOfferDownloadingWalletClicked(System.Object sender, System.EventArgs e)
    {
        string downloadLink = "https://play.google.com/store/apps/details?id=com.rostislavlitovkin.plutowallet";

        bool opened = await Launcher.Default.TryOpenAsync(downloadLink);

        if (opened)
        {
            return;
        }

        qrLabel.Text = "Download on Google Play";
        qrCode.Value = downloadLink;
    }

    private void OfferMinting()
    {
        ClearMainStackLayout();

        //template.IsVisible = true;

        logOutButton.IsVisible = true;
    }

    private async Task ShowNFTs()
    {
        ClearMainStackLayout();

        /*
        TokenInfoTemplate template = new TokenInfoTemplate();
        template.ContractAddressLabel.Text = "Powerup";

        template.AddPowerupDescription(new AtomicBombEvent().GetEventDescription);
        mainStackLayout.Children.Add(template);

        mainStackLayout.Children.Add(new BoxView { HeightRequest = 10 });

        */

        logOutButton.IsVisible = true;

    }
    private void NoInternetError()
    {
        ClearMainStackLayout();
        noInternet.IsVisible = true;
    }
    private void ClearMainStackLayout()
    { 
        checkingNfts.IsVisible = false;
        noInternet.IsVisible = false;
        qrCodeLayout.IsVisible = false;
        logOutButton.IsVisible = false;
        checkYourWallet.IsVisible = false;
        //template.IsVisible = false;    

        //if (mainStackLayout.Children.Count > 0) mainStackLayout.Children.Clear();
    }

    /*private void AddLabelToStackLayout(string txt)
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
    }*/

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
        try
        {
            checkYourWallet.IsVisible = true;

            // Find the next id
            /*
            string sParameters = RequestGenerator.GetStorage("Nfts", "Collection", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128}, new Substrate.NetApi.Model.Types.IType[] {
                        collectionId});
            var result = await substrateClient.GetStorageAsync<CollectionDetails>(sParameters, CancellationToken.None);

            EnumMultiAddress mint_to = new EnumMultiAddress();
            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(Preferences.Get("pubKey", "")));
            mint_to.Create(MultiAddress.Id, account32);

            System.Collections.Generic.List<byte> parameters = new List<byte>();

            // collectionId
            parameters.AddRange(collectionId.Encode());
            // itemId
            parameters.AddRange(result != null ? result.Items.Encode() : new U32(0).Encode());

            // mintTo
            parameters.AddRange(mint_to.Encode());

            // witnessData
            parameters.AddRange(new byte[0] { });

            //await PlutonicationDAppClient.SendPayloadAsync(52, 3, parameters.ToArray());
            */
            while (!await CheckNFTs())
            {
                await Task.Delay(7000);
            }

            await ShowNFTs();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error when trying to connect to the Plutonication?");

            Console.WriteLine(ex.Message);

            if (await CheckNFTs())
            {
                await ShowNFTs();
            }

            else OfferMinting();
        }

    }

    private async void OnLogOutClicked(System.Object sender, System.EventArgs e)
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
        await Navigation.PopAsync();

        //await qrCodeLayout.FadeTo(0, 500);
        //qrCodeLayout.IsVisible = false;
    }

    // Helper function
    /*public async Task<JArray> GetKeysPagedAtAsync(byte[] keyPrefix, uint pageCount, byte[] startKey, string blockHash, CancellationToken token)
    {
        
        var fullParams = new List<object>(4)
            {
                Utils.Bytes2HexString(keyPrefix),
                pageCount,
            };

        if (startKey != null)
        {
            fullParams.Add(Utils.Bytes2HexString(startKey));
        }

        if (!string.IsNullOrEmpty(blockHash))
        {
            fullParams.Add(blockHash);
        }

        return await substrateClient.InvokeAsync<JArray>("state_getKeysPaged", fullParams.ToArray(), token);
    }
    */





    /// <summary>
    /// >> 0 - Composite[sp_core.crypto.AccountId32]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class AccountId32 : BaseType
    {

        /// <summary>
        /// >> value
        /// </summary>
        private Arr32U8 _value;

        public Arr32U8 Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        public override string TypeName()
        {
            return "AccountId32";
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Value.Encode());
            return result.ToArray();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Value = new Arr32U8();
            Value.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }

    /// <summary>
    /// >> 1 - Array
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Array)]
    public sealed class Arr32U8 : BaseType
    {

        private Substrate.NetApi.Model.Types.Primitive.U8[] _value;

        public override int TypeSize
        {
            get
            {
                return 32;
            }
        }

        public Substrate.NetApi.Model.Types.Primitive.U8[] Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        public override string TypeName()
        {
            return string.Format("[{0}; {1}]", new Substrate.NetApi.Model.Types.Primitive.U8().TypeName(), this.TypeSize);
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            foreach (var v in Value) { result.AddRange(v.Encode()); };
            return result.ToArray();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            var array = new Substrate.NetApi.Model.Types.Primitive.U8[TypeSize];
            for (var i = 0; i < array.Length; i++) { var t = new Substrate.NetApi.Model.Types.Primitive.U8(); t.Decode(byteArray, ref p); array[i] = t; };
            var bytesLength = p - start;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
            Value = array;
        }

        public void Create(Substrate.NetApi.Model.Types.Primitive.U8[] array)
        {
            Value = array;
            Bytes = Encode();
        }
    }

    /// <summary>
    /// >> 348 - Composite[pallet_nfts.types.CollectionDetails]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class CollectionDetails : BaseType
    {

        /// <summary>
        /// >> owner
        /// </summary>
        private AccountId32 _owner;

        /// <summary>
        /// >> owner_deposit
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U128 _ownerDeposit;

        /// <summary>
        /// >> items
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _items;

        /// <summary>
        /// >> item_metadatas
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _itemMetadatas;

        /// <summary>
        /// >> item_configs
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _itemConfigs;

        /// <summary>
        /// >> attributes
        /// </summary>
        private Substrate.NetApi.Model.Types.Primitive.U32 _attributes;

        public AccountId32 Owner
        {
            get
            {
                return this._owner;
            }
            set
            {
                this._owner = value;
            }
        }

        public Substrate.NetApi.Model.Types.Primitive.U128 OwnerDeposit
        {
            get
            {
                return this._ownerDeposit;
            }
            set
            {
                this._ownerDeposit = value;
            }
        }

        public Substrate.NetApi.Model.Types.Primitive.U32 Items
        {
            get
            {
                return this._items;
            }
            set
            {
                this._items = value;
            }
        }

        public Substrate.NetApi.Model.Types.Primitive.U32 ItemMetadatas
        {
            get
            {
                return this._itemMetadatas;
            }
            set
            {
                this._itemMetadatas = value;
            }
        }

        public Substrate.NetApi.Model.Types.Primitive.U32 ItemConfigs
        {
            get
            {
                return this._itemConfigs;
            }
            set
            {
                this._itemConfigs = value;
            }
        }

        public Substrate.NetApi.Model.Types.Primitive.U32 Attributes
        {
            get
            {
                return this._attributes;
            }
            set
            {
                this._attributes = value;
            }
        }

        public override string TypeName()
        {
            return "CollectionDetails";
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Owner.Encode());
            result.AddRange(OwnerDeposit.Encode());
            result.AddRange(Items.Encode());
            result.AddRange(ItemMetadatas.Encode());
            result.AddRange(ItemConfigs.Encode());
            result.AddRange(Attributes.Encode());
            return result.ToArray();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Owner = new AccountId32();
            Owner.Decode(byteArray, ref p);
            OwnerDeposit = new Substrate.NetApi.Model.Types.Primitive.U128();
            OwnerDeposit.Decode(byteArray, ref p);
            Items = new Substrate.NetApi.Model.Types.Primitive.U32();
            Items.Decode(byteArray, ref p);
            ItemMetadatas = new Substrate.NetApi.Model.Types.Primitive.U32();
            ItemMetadatas.Decode(byteArray, ref p);
            ItemConfigs = new Substrate.NetApi.Model.Types.Primitive.U32();
            ItemConfigs.Decode(byteArray, ref p);
            Attributes = new Substrate.NetApi.Model.Types.Primitive.U32();
            Attributes.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }

    public enum MultiAddress
    {

        Id = 0,

        Index = 1,

        Raw = 2,

        Address32 = 3,

        Address20 = 4,
    }

    /// <summary>
    /// >> 197 - Variant[sp_runtime.multiaddress.MultiAddress]
    /// </summary>
    public sealed class EnumMultiAddress : BaseEnumExt<MultiAddress, AccountId32, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Base.BaseTuple>, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>, Arr32U8, Arr20U8>
    {
    }


    /// <summary>
    /// >> 74 - Array
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Array)]
    public sealed class Arr20U8 : BaseType
    {

        private Substrate.NetApi.Model.Types.Primitive.U8[] _value;

        public override int TypeSize
        {
            get
            {
                return 20;
            }
        }

        public Substrate.NetApi.Model.Types.Primitive.U8[] Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        public override string TypeName()
        {
            return string.Format("[{0}; {1}]", new Substrate.NetApi.Model.Types.Primitive.U8().TypeName(), this.TypeSize);
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            foreach (var v in Value) { result.AddRange(v.Encode()); };
            return result.ToArray();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            var array = new Substrate.NetApi.Model.Types.Primitive.U8[TypeSize];
            for (var i = 0; i < array.Length; i++) { var t = new Substrate.NetApi.Model.Types.Primitive.U8(); t.Decode(byteArray, ref p); array[i] = t; };
            var bytesLength = p - start;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
            Value = array;
        }

        public void Create(Substrate.NetApi.Model.Types.Primitive.U8[] array)
        {
            Value = array;
            Bytes = Encode();
        }
    }
}
