using TP_MAUI.Models;


namespace TP_MAUI;

public partial class SpiderPage : ContentPage
{
	private Dictionary<Guid, Match> buttonMaping;
    private const int BUTTON_WIDTH = 150;
    private const int BUTTON_HEIGHT = 46;
    private const int BOX_HEIGHT = 50;

    public SpiderPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		TournamentsList.ItemsSource = App.dbContext.GetTournaments();
        SpiderBoard.Clear();
		base.OnAppearing();
	}

	protected void TournamentSelected(object sender, SelectionChangedEventArgs e)
	{
        DisplaySpider(((Tournament)e.CurrentSelection.First()).TournamentId);
	}

	protected void WinnerSelected(object sender, EventArgs e)
	{
		var buttonId = ((Button)sender).Id;
		if (buttonMaping.ContainsKey(buttonId))
		{
			App.dbContext.SetWinner(buttonMaping[buttonId]);
            DisplaySpider(buttonMaping[buttonId].TournamentId);
        }
	}

	protected void DisplaySpider(int tournamentId)
	{
        SpiderBoard.Clear();
        if (buttonMaping is null)
        {
            buttonMaping = new Dictionary<Guid, Match>();
        }
        buttonMaping.Clear();

        var matches = App.dbContext.GetMatchesOfTournament(tournamentId);
        var groupedMatches = matches.GroupBy(match => match.TournamentDepth).Select(grp => grp.OrderBy(match => match.TournamentLevel).ToList()).ToList();
        var currentDepth = 0;

        foreach (var depthMatches in groupedMatches)
        {
            VerticalStackLayout depthStack = new VerticalStackLayout();
            for (int i = 0; i < depthMatches.Count; i++)
            {
                VerticalStackLayout match = new VerticalStackLayout();
                var frame = new Frame
                {
                    CornerRadius = 0,
                    Padding = 4
                };

                var player0 = new Button
                {
                    Text = depthMatches[i].Player0Name,
                    Padding = 5,
                    WidthRequest = BUTTON_WIDTH,
                    HeightRequest = BUTTON_HEIGHT,
                    TextColor = Colors.Black
                };
                player0.Clicked += WinnerSelected;

                if (depthMatches[i].WinnerId is null)
                {
                    player0.BackgroundColor = Color.FromArgb("#b3ffff");
                }
                else if (depthMatches[i].Player0 == depthMatches[i].WinnerId)
                {
                    player0.BackgroundColor = Color.FromArgb("#ccffcc");
                }
                else
                {
                    player0.BackgroundColor = Color.FromArgb("#ffb3b3");
                }

                var player1 = new Button
                {
                    Text = depthMatches[i].Player1Name,
                    Padding = 5,
                    WidthRequest = BUTTON_WIDTH,
                    HeightRequest = BUTTON_HEIGHT,
                    TextColor = Colors.Black
                };
                player1.Clicked += WinnerSelected;

                if (depthMatches[i].WinnerId is null)
                {
                    player1.BackgroundColor = Color.FromArgb("#b3ffff");
                }
                else if (depthMatches[i].Player1 == depthMatches[i].WinnerId)
                {
                    player1.BackgroundColor = Color.FromArgb("#ccffcc");
                }
                else
                {
                    player1.BackgroundColor = Color.FromArgb("#ffb3b3");
                }


                match.Add(player0);
                match.Add(player1);

                //match.BackgroundColor = Color.FromArgb("#e6ffe6");
                frame.Content = match;

                if (depthMatches[i].Player0 is not null && depthMatches[i].Player1 is not null)
                {
                    var match0 = new Match(depthMatches[i]);
                    match0.WinnerId = match0.Player0;
                    var match1 = new Match(depthMatches[i]);
                    match1.WinnerId = match1.Player1;

                    buttonMaping.Add(player0.Id, match0);
                    buttonMaping.Add(player1.Id, match1);
                }

                var boxCount = Convert.ToInt32(Math.Pow(2, currentDepth) - 1);
                for (int j = 0; j < boxCount; j++)
                {
                    depthStack.Add(new BoxView
                    {
                        HeightRequest = BOX_HEIGHT,
                        Color = Colors.White
                    });
                }
                depthStack.Add(frame);
                for (int j = 0; j < boxCount; j++)
                {
                    depthStack.Add(new BoxView
                    {
                        HeightRequest = BOX_HEIGHT,
                        Color = Colors.White
                    });
                }
            }
            currentDepth++;
            SpiderBoard.Add(depthStack);
        }
    }
}