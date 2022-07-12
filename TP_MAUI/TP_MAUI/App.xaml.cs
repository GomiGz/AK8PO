namespace TP_MAUI;

public partial class App : Application
{

    public static DbContext dbContext;
    public App(DbContext repo)
	{
		InitializeComponent();

		MainPage = new MainPage();
		dbContext = repo;
	}
}
