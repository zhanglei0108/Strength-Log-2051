namespace Strength_Log;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    //Go to the welcome page//
    private async void GoWelcomeClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new WelcomePage());
    }
}