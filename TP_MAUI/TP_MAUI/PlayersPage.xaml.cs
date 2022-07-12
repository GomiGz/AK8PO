using System;
using TP_MAUI.Models;

namespace TP_MAUI;

public partial class PlayersPage : ContentPage
{

	public PlayersPage()
	{
        InitializeComponent();
    }

	protected override void OnAppearing()
    {
        PlayersList.ItemsSource = App.dbContext.GetPlayers();
        LevelPicker.ItemsSource = App.dbContext.GetLevels();
        LevelPicker.ItemDisplayBinding = new Binding("LevelName");
        base.OnAppearing();
	}

    public void PlayerSelected(object sender, SelectionChangedEventArgs e)
    {
        int playerId = (e.CurrentSelection.FirstOrDefault(0) as Player).PlayerId;
        var playerDetail = App.dbContext.GetPlayer(playerId);
        PlayerDetail.BindingContext = playerDetail;
        LevelPicker.SelectedIndex = playerDetail.LevelId;
        DeletePlayerButton.IsEnabled = true;
    }
    void ValueChanged(object sender, EventArgs e)
    {
        DeletePlayerButton.IsEnabled = false;
    }

    void AgeChanged(object sender, ValueChangedEventArgs e)
    {
        AgeLabel.Text = e.NewValue.ToString();
    }

    void SavePlayer(object sender, EventArgs args)
    {
        var player = new Player();
        player.FirstName = FirstNameEntry.Text;
        player.LastName = LastNameEntry.Text;
        player.Email = EmailEntry.Text;
        player.Age = Int32.Parse(AgeLabel.Text);
        player.LevelId = LevelPicker.SelectedIndex;

        App.dbContext.SavePlayer(player);
        PlayersList.ItemsSource = App.dbContext.GetPlayers();
        ClearTable();
    }

    void UpdatePlayer(object sender, EventArgs args)
    {
        var player = new Player();
        player.FirstName = FirstNameEntry.Text;
        player.LastName = LastNameEntry.Text;
        player.Email = EmailEntry.Text;
        player.Age = Int32.Parse(AgeLabel.Text);
        player.LevelId = LevelPicker.SelectedIndex;
        player.PlayerId = ((Player)PlayersList.SelectedItem).PlayerId;

        App.dbContext.UpdatePlayer(player);
        PlayersList.ItemsSource = App.dbContext.GetPlayers();
        ClearTable();
    }

    private void ClearTable()
    {
        FirstNameEntry.Text = "";
        LastNameEntry.Text = "";
        AgeLabel.Text = "";
        EmailEntry.Text = "";
        LevelPicker.SelectedIndex = 0;
    }

    void DeletePlayer(object sender, EventArgs args)
    {
        App.dbContext.DeletePlayer(((Player)PlayersList.SelectedItem).PlayerId);
        PlayersList.ItemsSource = App.dbContext.GetPlayers();
        ClearTable();
    }




}