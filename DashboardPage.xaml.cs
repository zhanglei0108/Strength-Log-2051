using Microsoft.Maui.Storage;

namespace Strength_Log;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        double height = Preferences.Get("Height", 0.0);
        double weight = Preferences.Get("Weight", 0.0);
        string gender = Preferences.Get("Gender", "Unknown");
        double bmi = Preferences.Get("BMI", 0.0);

        GenderLabel.Text = $"Gender: {gender}";
        HeightLabel.Text = $"Height: {height:F0} cm";
        WeightLabel.Text = $"Weight: {weight:F1} kg";
        BmiLabel.Text = $"BMI: {bmi:F1}";
        BmiStatusLabel.Text = $"BMI Status: {GetBmiStatus(bmi)}";

        BmiCommentLabel.Text = GetBmiComment(bmi);
        MotivationLabel.Text = GetMotivationText(bmi);

        SetProfileImage(gender);
    }

    private void SetProfileImage(string gender)
    {
        string normalizedGender = gender?.Trim().ToLower() ?? string.Empty;

        if (normalizedGender == "female")
        {
            ProfileImage.Source = "female.png";
        }
        else
        {
            ProfileImage.Source = "male.png";
        }
    }
    //Different situations of BMI//
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
    //Log in different copy content in the middle copy area according to different situations//
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
    //Encouragement phrases//
    private string GetMotivationText(double bmi)
    {
        return "Stay consistent, trust the process, and every workout will bring you one step closer to your goal.";
    }
    //This task is still under development....//
    private async void OnAddWorkoutClicked(object? sender, EventArgs e)
    {
        await DisplayAlertAsync("Coming Soon",
             "The workout recording feature will be added next.",
             "OK");
    }

    private async void OnEditProfileClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new WelcomePage());
    }
    //The specific function of the button - Edit//
    private async void OnResetDataClicked(object? sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync(
            "Reset Data",
            "Are you sure you want to clear all saved profile data?",
            "Yes",
            "No");

        if (confirm)
        {
            Preferences.Clear();
            Application.Current!.Windows[0].Page = new NavigationPage(new MainPage());
        }
    }
}