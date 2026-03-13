using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace Strength_Log;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        bool hasProfile = Preferences.Get("HasUserProfile", false);

        if (hasProfile)
        {
            double savedHeight = Preferences.Get("Height", 0.0);
            double savedWeight = Preferences.Get("Weight", 0.0);
            string savedGender = Preferences.Get("Gender", string.Empty);

            if (savedHeight > 0)
                HeightEntry.Text = savedHeight.ToString();

            if (savedWeight > 0)
                WeightEntry.Text = savedWeight.ToString();

            if (!string.IsNullOrWhiteSpace(savedGender))
                GenderPicker.SelectedItem = savedGender;
        }
    }

    private async void OnContinueClicked(object? sender, EventArgs e)
    {
        string heightText = HeightEntry.Text ?? string.Empty;
        string weightText = WeightEntry.Text ?? string.Empty;
        string gender = GenderPicker.SelectedItem?.ToString() ?? string.Empty;
        //Prompt for incomplete information filling//
        if (string.IsNullOrWhiteSpace(heightText) ||
            string.IsNullOrWhiteSpace(weightText) ||
            string.IsNullOrWhiteSpace(gender))
        {
            await DisplayAlertAsync("Missing Information",
                "Please enter height, weight, and select gender.",
                "OK");
            return;
        }
        //Prompt for incorrect information filling//
        bool isHeightValid = double.TryParse(heightText, out double heightCm);
        bool isWeightValid = double.TryParse(weightText, out double weightKg);

        if (!isHeightValid || !isWeightValid || heightCm <= 0 || weightKg <= 0)
        {
            await DisplayAlertAsync("Invalid Input",
                "Please enter valid positive numbers for height and weight.",
                "OK");
            return;
        }
        //BMI calculation formula//
        double heightM = heightCm / 100.0;
        double bmi = weightKg / (heightM * heightM);

        BmiResultLabel.Text = $"Your BMI: {bmi:F1}";

        UserProfile.Height = heightCm;
        UserProfile.Weight = weightKg;
        UserProfile.Gender = gender;
        UserProfile.BMI = bmi;

        Preferences.Set("HasUserProfile", true);
        Preferences.Set("Height", heightCm);
        Preferences.Set("Weight", weightKg);
        Preferences.Set("Gender", gender);
        Preferences.Set("BMI", bmi);

        await Navigation.PushAsync(new DashboardPage());
    }
}