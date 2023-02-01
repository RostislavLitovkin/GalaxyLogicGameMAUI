using System.Collections;

namespace GalaxyLogicGame.Pagesanddescriptions;

public partial class Leaderboard : AbsoluteLayout
{
#if __IOS__        
    static HttpClient client = new HttpClient(new NSUrlSessionHandler());
#elif __ANDROID__
    static HttpClient client = new HttpClient(new Xamarin.Android.Net.AndroidMessageHandler());
#else
    static HttpClient client= new HttpClient();
#endif

    public Leaderboard()
	{
		InitializeComponent();

        Load();
	}

	public async Task Load()
    {

        NetworkAccess accessType = Connectivity.Current.NetworkAccess;

        if (accessType == NetworkAccess.Internet)
        {
            // Connection to internet is available

            var url = "http://aspapitest.azurewebsites.net/Leaderboard";

            //var url = "https://localhost:7204/Leaderboard";

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                List<PlayerDTO> content = await response.Content.ReadAsAsync<List<PlayerDTO>>();

                if (content.Count == 0) return;

                addressLabel.Text = content[0].address;
                scoreLabel.Text = content[0].highscore.ToString();
            }
            else
            {
                addressLabel.Text = "Failed to load";
            }
        }
        else
        {
            addressLabel.Text = "No network access";

        }


    }

    public class PlayerDTO
    {
        public string address { get; set; }
        public int highscore { get; set; }
    }
}
