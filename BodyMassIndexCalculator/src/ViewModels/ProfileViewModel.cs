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
            var user = SupabaseService.Client.Auth.CurrentUser;
            if (user == null) SetUserData();
            else
            {
                var metadata = user.UserMetadata;
                string? firstName = metadata?["first_name"]?.ToString();
                string? lastName = metadata?["last_name"]?.ToString();
                SetUserData($"{firstName} {lastName}", user.Email ?? "null");
            }
            _ = LoadCalculationsDataAsync();
        }

        [RelayCommand]
        public async Task RefreshData()
        {
            if ((DateTime.Now - _lastRefreshTime).TotalSeconds < 5)
                return;

            await LoadCalculationsDataAsync();
            _lastRefreshTime = DateTime.Now;
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

        private void SetUserData(string fullName = "null null", string email = "null") 
            => (ProfileModel.Name, ProfileModel.Email) = (fullName, email);
    }
}
