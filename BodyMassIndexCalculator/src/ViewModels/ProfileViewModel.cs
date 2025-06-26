using BodyMassIndexCalculator.src.Models;
using BodyMassIndexCalculator.src.Services;
using BodyMassIndexCalculator.src.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BodyMassIndexCalculator.src.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IAPI _api;
        private readonly AuthService _authService;
        
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
            InitializeProfile();
            InitializeCalculations();
        }

        private async void InitializeProfile()
        {
            var user = _authService.CurrentSession?.User;
            if (user != null)
            {
                var id = user.Id;
                if (id != null)
                {
                    var profile = await _api.GetProfileByUserId(Guid.Parse(id));
                    if (profile != null)
                        Name = $"{profile.FirstName} {profile.LastName}";
                    else
                        Name = "null null";
                }
                else
                    Name = "null null";

                Email = user.Email;
            }
            else
            {
                Name = "null null";
                Email = "null";
            }
        }

        private async void InitializeCalculations()
        {
            IEnumerable<BodyMassIndexCalculation> calculations = [];
            var id = _authService.CurrentSession?.User?.Id;
            if (id != null)
                calculations = await _api.GetCalculationsByUserId(Guid.Parse(id));

            BodyMassIndexCalculations = new ObservableCollection<BodyMassIndexCalculation>(calculations.OrderBy(bmic => bmic.CreatedAt).Reverse());
        }
    }
}
