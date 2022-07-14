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
		TournamentsList.ItemsSource = App.dbContext.GetTournaments();
		SuccessLabel.Text = "";
		TournamentErrorLabel.Text = "";
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
		if (String.IsNullOrWhiteSpace(TournamentNameEntry.Text))
		{
            SuccessLabel.Text = "Nebyl zadán název";
            SuccessLabel.TextColor = Colors.Red;
            return;
        }
		if (players is null || players.Count == 0)
		{
            SuccessLabel.Text = "Nebyli vybráni hráèi";
			SuccessLabel.TextColor = Colors.Red;
			return;
        }
		var tournamentId = App.dbContext.InsertTournament(TournamentNameEntry.Text);
		
		var tournament = new Tournament() { TournamentId = tournamentId };
		tournament.GenerateMatches(players.Select(player => player.PlayerId).ToList());
		SuccessLabel.Text = "Turnaj spuštìn";
		SuccessLabel.TextColor = Colors.Green;
        players = null;
        TournamentsList.ItemsSource = App.dbContext.GetTournaments();
    }

	protected void CancelTournament(object sender, EventArgs e)
	{
		var item = TournamentsList.SelectedItem;
		if(item is null)
		{
			TournamentErrorLabel.Text = "Nebyl vybrán turnaj";
			return;
		}
		App.dbContext.DeleteTournament(((Tournament)item).TournamentId);
        TournamentsList.ItemsSource = App.dbContext.GetTournaments();
    }

    protected void ResetTournament(object sender, EventArgs e)
    {
        var item = TournamentsList.SelectedItem;
        if (item is null)
        {
            TournamentErrorLabel.Text = "Nebyl vybrán turnaj";
			TournamentErrorLabel.TextColor = Colors.Red;
            return;
        }
        App.dbContext.ResetTournament(((Tournament)item).TournamentId);
        TournamentErrorLabel.Text = "Turnaj restartován";
        TournamentErrorLabel.TextColor = Colors.Green;
    }
}