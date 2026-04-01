namespace Strength_Log;

public partial class MainPage : ContentPage
{
    private bool _hasCheckedProfile = false;

    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_hasCheckedProfile)
            return;

        _hasCheckedProfile = true;

        var profile = await DbHelper.Database.GetLatestUserProfileAsync();

        if (profile is not null)
        {
            Application.Current!.Windows[0].Page = new NavigationPage(new DashboardPage());
        }
    }

    //Go to the welcome page//
    private async void GoWelcomeClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new WelcomePage());
    }
}