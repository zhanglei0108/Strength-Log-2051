using System.Collections.Generic;
using System.Linq;

namespace Strength_Log;

public partial class WorkoutHistoryPage : ContentPage
{
    private List<WorkoutRecord> allWorkouts = new();

    public WorkoutHistoryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        allWorkouts = await DbHelper.Database.GetAllWorkoutsAsync();
        WorkoutCollectionView.ItemsSource = allWorkouts;
    }

    private async void OnDeleteWorkoutClicked(object sender, EventArgs e)
    {
        Button? button = sender as Button;
        WorkoutRecord? selectedWorkout = button?.CommandParameter as WorkoutRecord;

        if (selectedWorkout == null)
            return;

        bool confirm = await DisplayAlertAsync(
            "Delete Workout",
            "Are you sure you want to delete this workout record?",
            "Yes",
            "No");

        if (!confirm)
            return;

        await DbHelper.Database.DeleteWorkoutAsync(selectedWorkout);

        allWorkouts = await DbHelper.Database.GetAllWorkoutsAsync();
        WorkoutCollectionView.ItemsSource = allWorkouts;

        NoResultLabel.IsVisible = false;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string keyword = e.NewTextValue?.Trim().ToLower() ?? "";

        if (string.IsNullOrWhiteSpace(keyword))
        {
            WorkoutCollectionView.ItemsSource = allWorkouts;
            NoResultLabel.IsVisible = false;
        }
        else
        {
            var filteredWorkouts = allWorkouts
                 .Where(x => !string.IsNullOrWhiteSpace(x.DateText) &&
                              x.DateText.ToLower().Contains(keyword))
                 .ToList();

            WorkoutCollectionView.ItemsSource = filteredWorkouts;
            NoResultLabel.IsVisible = filteredWorkouts.Count == 0;
        }
    }

}