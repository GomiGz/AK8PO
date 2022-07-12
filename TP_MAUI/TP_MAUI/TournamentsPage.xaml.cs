using TP_MAUI.Models;

namespace TP_MAUI;

public partial class TournamentsPage : ContentPage
{

	private List<Player> players;
	public TournamentsPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		UpperPicker.ItemsSource = App.dbContext.GetLevels();
		UpperPicker.ItemDisplayBinding = new Binding("LevelName");
        LowerPicker.ItemsSource = App.dbContext.GetLevels();
        LowerPicker.ItemDisplayBinding = new Binding("LevelName");
        base.OnAppearing();
	}

	private void FindPlayers(object source, EventArgs e)
	{
		try
		{
			Int32.Parse(UpperAgeLimitEntry.Text);
			Int32.Parse(LowerAgeLimitEntry.Text);
		}
		catch
		{
			return;
		}
		players = App.dbContext.FindPlayers(
                                            Int32.Parse(UpperAgeLimitEntry.Text),
                                            Int32.Parse(LowerAgeLimitEntry.Text),
                                            UpperPicker.SelectedIndex,
                                            LowerPicker.SelectedIndex);
		PlayersList.ItemsSource = players;
        SuccessLabel.Text = "";
    }

    public void StartTournament(object sender, EventArgs e)
    {
		if (players is null || players.Count == 0)
		{
            SuccessLabel.Text = "Nebyli vybr�ni hr��i";
			SuccessLabel.TextColor = Colors.Red;
			return;
        }
		var tournamentId = App.dbContext.InsertTournament(TournamentNameEntry.Text);
		
		var tournament = new Tournament() { TournamentId = tournamentId };
		tournament.GenerateMatches(players.Select(player => player.PlayerId).ToList());
		SuccessLabel.Text = "Turnaj spu�t�n";
		SuccessLabel.TextColor = Colors.Green;
        players = null;
    }

}