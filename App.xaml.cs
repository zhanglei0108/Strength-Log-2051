namespace Strength_Log;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        _ = DbHelper.Database.InitAsync();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new NavigationPage(new MainPage()));
    }
}