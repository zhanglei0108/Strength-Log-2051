namespace Strength_Log;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var profile = await DbHelper.Database.GetLatestUserProfileAsync();

        double height = 0.0;
        double weight = 0.0;
        string gender = "Unknown";
        double bmi = 0.0;

        if (profile is not null)
        {
            height = profile.Height;
            weight = profile.Weight;
            gender = profile.Gender;
            bmi = profile.BMI;
        }

        GenderLabel.Text = $"Gender: {gender}";
        HeightLabel.Text = $"Height: {height:F0} cm";
        WeightLabel.Text = $"Weight: {weight:F1} kg";
        BmiLabel.Text = $"BMI: {bmi:F1}";
        BmiStatusLabel.Text = $"BMI Status: {GetBmiStatus(bmi)}";

        BmiCommentLabel.Text = GetBmiComment(bmi);
        MotivationLabel.Text = GetMotivationText(bmi);

        SetProfileImage(gender);

        // Show latest workout
        var latestWorkout = await DbHelper.Database.GetLatestWorkoutAsync();

        if (latestWorkout is null)
        {
            WorkoutPlaceholderLabel.Text = "No workout records yet. Start adding your first workout.";
        }
        else
        {
            WorkoutPlaceholderLabel.Text =
                $"Exercise: {latestWorkout.ExerciseType}\n" +
                $"Sets: {latestWorkout.Sets}   Reps: {latestWorkout.Reps}\n" +
                $"Weight: {latestWorkout.Weight:F1} kg\n" +
                $"Date: {latestWorkout.DateText}";
        }
    }

    private void SetProfileImage(string gender)
    {
        string normalizedGender = gender?.Trim().ToLower() ?? string.Empty;

        if (normalizedGender == "female")
        {
            ProfileImage.Source = "female.jpg";
        }
        else
        {
            ProfileImage.Source = "male.jpg";
        }
    }

    private string GetBmiStatus(double bmi)
    {
        if (bmi <= 0)
            return "Not Available";
        else if (bmi < 18.5)
            return "Underweight";
        else if (bmi < 25)
            return "Normal";
        else if (bmi < 30)
            return "Overweight";
        else
            return "Obese";
    }
    private async void OnViewHistoryClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new WorkoutHistoryPage());
    }

    private string GetBmiComment(double bmi)
    {
        if (bmi <= 0)
            return "Your BMI data is not available yet.";
        else if (bmi < 18.5)
            return "Your current BMI is below the healthy range. A balanced diet and strength training may help.";
        else if (bmi < 25)
            return "Your current BMI is within a healthy range. Keep maintaining your good habits.";
        else if (bmi < 30)
            return "Your BMI is slightly above the healthy range. Consistent training and nutrition control can help.";
        else
            return "Your BMI is above the healthy range. A gradual and sustainable fitness plan is recommended.";
    }

    private string GetMotivationText(double bmi)
    {
        return "Stay consistent, trust the process, and every workout will bring you one step closer to your goal.";
    }

    private async void OnAddWorkoutClicked(object? sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NavigationPage(new AddWorkoutPage()));
    }

    private async void OnEditProfileClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new WelcomePage());
    }

    private async void OnResetDataClicked(object? sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync(
            "Reset Data",
            "Are you sure you want to clear all saved profile data?",
            "Yes",
            "No");

        if (confirm)
        {
            await DbHelper.Database.ClearAllDataAsync();
            Application.Current!.Windows[0].Page = new NavigationPage(new MainPage());
        }
    }
}