using Microsoft.Maui.Storage;

namespace Strength_Log;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }
    //Check the local storage information to distinguish between new and old users//
    protected override Window CreateWindow(IActivationState? activationState)
    {
        bool hasProfile = Preferences.Get("HasUserProfile", false);

        Page startPage;

        if (hasProfile)
        {
            startPage = new NavigationPage(new DashboardPage());
        }
        else
        {
            startPage = new NavigationPage(new MainPage());
        }

        return new Window(startPage);
    }
}