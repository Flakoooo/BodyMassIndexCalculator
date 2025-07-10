using BodyMassIndexCalculator.src.Models;
using BodyMassIndexCalculator.src.Services;
using BodyMassIndexCalculator.src.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BodyMassIndexCalculator.src.ViewModels
{
    public partial class ProfileModel : ObservableObject
    {
        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _email;

        [ObservableProperty]
        private ObservableCollection<BodyMassIndexCalculation>? _bodyMassIndexCalculations;
    }

    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IAPI _api;
        private DateTime _lastRefreshTime = DateTime.MinValue;

        [ObservableProperty]
        private ProfileModel _profileModel;

        public ProfileViewModel(IAPI api)
        {
            _api = api;
            ProfileModel = new ProfileModel
            {
                Name = string.Empty,
                Email = string.Empty,
                BodyMassIndexCalculations = []
            };
            _ = InitializeData();
        }

        [RelayCommand]
        public async Task RefreshData()
        {
            if ((DateTime.Now - _lastRefreshTime).TotalSeconds < 5)
                return;

            await InitializeData();
            _lastRefreshTime = DateTime.Now;
        }

        private async Task InitializeData()
        {
            await Task.WhenAll(
                LoadProfileDataAsync(),
                LoadCalculationsDataAsync()
            );
        }

        private async Task LoadProfileDataAsync()
        {
            var user = SupabaseService.Client.Auth.CurrentUser;
            if (user == null)
            {
                SetNullProfile();
                return;
            }

            var profile = await GetUserProfileAsync(user.Id);
            ProfileModel.Name = profile != null ? $"{profile.FirstName} {profile.LastName}"
                                   : "null null";

            ProfileModel.Email = user.Email ?? "null";

            /*
            var profile = await GetUserProfileAsync("1");
            ProfileModel.Name = profile != null ? $"{profile.FirstName} {profile.LastName}"
                                   : "null null";

            ProfileModel.Email = "email" ?? "null";
            */
        }

        private async Task<Profile?> GetUserProfileAsync(string? userId)
        {
            if (userId == null) return null;
            return await _api.GetProfileByUserId(Guid.Parse(userId));
        }

        private async Task LoadCalculationsDataAsync()
        {
            var calculations = await GetUserCalculationsAsync();
            ProfileModel.BodyMassIndexCalculations = new ObservableCollection<BodyMassIndexCalculation>(
                calculations.OrderByDescending(bmic => bmic.CreatedAt)
            );
        }

        private async Task<IEnumerable<BodyMassIndexCalculation>> GetUserCalculationsAsync()
        {
            var userId = SupabaseService.Client.Auth.CurrentUser?.Id;
            return userId != null ? await _api.GetCalculationsByUserId(Guid.Parse(userId)) : [];

            /*
            var userId = "1";
            return userId != null ? 
                [
                new BodyMassIndexCalculation 
                { 
                    UserId = Guid.Parse("1"), 
                    CreatedAt = DateTime.Now, 
                    Height = 185,
                    Weight = 77,
                    BodyMassIndex = 22.5,
                    Recommendation = "рекомендация 1"
                },
                new BodyMassIndexCalculation
                {
                    UserId = Guid.Parse("1"),
                    CreatedAt = DateTime.Now.AddDays(-1),
                    Height = 180,
                    Weight = 82,
                    BodyMassIndex = 24.3,
                    Recommendation = "рекомендация 2"
                },
                new BodyMassIndexCalculation
                {
                    UserId = Guid.Parse("1"),
                    CreatedAt = DateTime.Now.AddDays(-2),
                    Height = 190,
                    Weight = 56,
                    BodyMassIndex = 19.2,
                    Recommendation = "рекомендация 3"
                }
                ] : [];
            */
        }

        private void SetNullProfile()
        {
            ProfileModel.Name = "null null";
            ProfileModel.Email = "null";
        }
    }
}
