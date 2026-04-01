namespace Strength_Log;
using Microsoft.Maui.Devices;

public partial class AddWorkoutPage : ContentPage
{
    public AddWorkoutPage()
    {
        InitializeComponent();
        WorkoutDatePicker.Date = DateTime.Today;
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        string exerciseType = ExerciseEntry.Text?.Trim() ?? string.Empty;
        string setsText = SetsEntry.Text?.Trim() ?? string.Empty;
        string repsText = RepsEntry.Text?.Trim() ?? string.Empty;
        string weightText = WeightEntry.Text?.Trim() ?? string.Empty;
        string dateText = $"{WorkoutDatePicker.Date:yyyy-MM-dd}";

        if (string.IsNullOrWhiteSpace(exerciseType) ||
            string.IsNullOrWhiteSpace(setsText) ||
            string.IsNullOrWhiteSpace(repsText) ||
            string.IsNullOrWhiteSpace(weightText))
        {
            await DisplayAlertAsync("Missing Information",
                "Please complete all workout fields.",
                "OK");
            return;
        }

        bool isSetsValid = int.TryParse(setsText, out int sets);
        bool isRepsValid = int.TryParse(repsText, out int reps);
        bool isWeightValid = double.TryParse(weightText, out double weight);

        if (!isSetsValid || !isRepsValid || !isWeightValid ||
            sets <= 0 || reps <= 0 || weight < 0)
        {
            await DisplayAlertAsync("Invalid Input",
                "Please enter valid positive numbers for sets, reps, and weight.",
                "OK");
            return;
        }

        var workout = new WorkoutRecord
        {
            ExerciseType = exerciseType,
            Sets = sets,
            Reps = reps,
            Weight = weight,
            DateText = dateText
        };

        await DbHelper.Database.SaveWorkoutAsync(workout);

        try
        {
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200));
        }
        catch
        {       
        }

        await DisplayAlertAsync("Success",
            "Workout record saved successfully.",
            "OK");

        await Navigation.PopModalAsync();
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}