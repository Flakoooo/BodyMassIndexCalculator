using BodyMassIndexCalculator.src.Models;
using BodyMassIndexCalculator.src.Services;
using BodyMassIndexCalculator.src.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BodyMassIndexCalculator.src.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IAPI _api;
        private readonly AuthService _authService;
        private DateTime _lastRefreshTime = DateTime.MinValue;

        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _email;

        [ObservableProperty]
        private ObservableCollection<BodyMassIndexCalculation>? _bodyMassIndexCalculations;

        public ProfileViewModel(IAPI api, AuthService authService)
        {
            _api = api;
            _authService = authService;
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
            var user = _authService.CurrentSession?.User;
            if (user == null)
            {
                SetNullProfile();
                return;
            }

            var profile = await GetUserProfileAsync(user.Id);
            Name = profile != null ? $"{profile.FirstName} {profile.LastName}"
                                   : "null null";

            Email = user.Email ?? "null";
        }

        private async Task LoadCalculationsDataAsync()
        {
            var calculations = await GetUserCalculationsAsync();
            BodyMassIndexCalculations = new ObservableCollection<BodyMassIndexCalculation>(
                calculations.OrderByDescending(bmic => bmic.CreatedAt)
            );
        }

        private async Task<Profile?> GetUserProfileAsync(string? userId)
        {
            if (userId == null) return null;
            return await _api.GetProfileByUserId(Guid.Parse(userId));
        }

        private async Task<IEnumerable<BodyMassIndexCalculation>> GetUserCalculationsAsync()
        {
            var userId = _authService.CurrentSession?.User?.Id;
            return userId != null ? await _api.GetCalculationsByUserId(Guid.Parse(userId)) : [];
        }

        private void SetNullProfile()
        {
            Name = "null null";
            Email = "null";
        }
    }
}
