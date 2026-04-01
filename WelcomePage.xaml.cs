using Microsoft.Maui.Controls;

namespace Strength_Log;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var profile = await DbHelper.Database.GetLatestUserProfileAsync();

        if (profile is not null)
        {
            if (profile.Height > 0)
                HeightEntry.Text = profile.Height.ToString();

            if (profile.Weight > 0)
                WeightEntry.Text = profile.Weight.ToString();

            if (!string.IsNullOrWhiteSpace(profile.Gender))
                GenderPicker.SelectedItem = profile.Gender;
        }
    }

    private async void OnContinueClicked(object? sender, EventArgs e)
    {
        string heightText = HeightEntry.Text ?? string.Empty;
        string weightText = WeightEntry.Text ?? string.Empty;
        string gender = GenderPicker.SelectedItem?.ToString() ?? string.Empty;

        // Prompt for incomplete information filling
        if (string.IsNullOrWhiteSpace(heightText) ||
            string.IsNullOrWhiteSpace(weightText) ||
            string.IsNullOrWhiteSpace(gender))
        {
            await DisplayAlertAsync("Missing Information",
                "Please enter height, weight, and select gender.",
                "OK");
            return;
        }

        // Prompt for incorrect information filling
        bool isHeightValid = double.TryParse(heightText, out double heightCm);
        bool isWeightValid = double.TryParse(weightText, out double weightKg);

        if (!isHeightValid || !isWeightValid || heightCm <= 0 || weightKg <= 0)
        {
            await DisplayAlertAsync("Invalid Input",
                "Please enter valid positive numbers for height and weight.",
                "OK");
            return;
        }

        // BMI calculation formula
        double heightM = heightCm / 100.0;
        double bmi = weightKg / (heightM * heightM);


        // Save to database only
        var existingProfile = await DbHelper.Database.GetLatestUserProfileAsync();

        if (existingProfile is not null)
        {
            existingProfile.Height = heightCm;
            existingProfile.Weight = weightKg;
            existingProfile.Gender = gender;
            existingProfile.BMI = bmi;

            await DbHelper.Database.SaveUserProfileAsync(existingProfile);
        }
        else
        {
            var newProfile = new UserProfileRecord
            {
                Height = heightCm,
                Weight = weightKg,
                Gender = gender,
                BMI = bmi
            };

            await DbHelper.Database.SaveUserProfileAsync(newProfile);
        }

        await Navigation.PushAsync(new DashboardPage());
    }
}