namespace GalaxyLogicGame.Pagesanddescriptions;

public partial class Leaderboard : AbsoluteLayout
{
	public Leaderboard()
	{
		InitializeComponent();

        Load();
	}

	public async Task Load()
    {
        

        //var url = "http://rostislavlitovkin.pythonanywhere.com/validate";

        var url = "http://10.14.201.188:5000/leaderboardAddress";
        using var client = new HttpClient();

        var result = await client.GetAsync(url);

        var contentStream = await result.Content.ReadAsStringAsync();

        addressLabel.Text = contentStream.Substring(0, 6) + "..";


        var url2 = "http://10.14.201.188:5000/leaderboardScore";

        var result2 = await client.GetAsync(url2);

        var contentStream2 = await result2.Content.ReadAsStringAsync();

        scoreLabel.Text = contentStream2;
    }
}
