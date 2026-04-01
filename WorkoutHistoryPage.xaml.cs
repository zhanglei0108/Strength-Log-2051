namespace Strength_Log;

public partial class WorkoutHistoryPage : ContentPage
{
    public WorkoutHistoryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var workouts = await DbHelper.Database.GetAllWorkoutsAsync();
        WorkoutCollectionView.ItemsSource = workouts;
    }
}