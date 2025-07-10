using BodyMassIndexCalculator.src.Services;
using BodyMassIndexCalculator.src.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BodyMassIndexCalculator.src.ViewModels
{
    public partial class RegisterModel : ObservableObject
    {
        [ObservableProperty]
        private string? _errorText;

        [ObservableProperty]
        private bool _isErrorVisible;

        [ObservableProperty]
        private string? _firstName;

        [ObservableProperty]
        private string? _lastName;

        [ObservableProperty]
        private string? _email;

        [ObservableProperty]
        private string? _password;
    }

    public partial class RegisterViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private RegisterModel _registerModel;

        public RegisterViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            RegisterModel = new RegisterModel
            {
                ErrorText = string.Empty,
                IsErrorVisible = false,
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                Password = string.Empty

            };
        }

        private void SetErrorText(string? error, bool visible)
        {
            RegisterModel.ErrorText = error;
            RegisterModel.IsErrorVisible = visible;
        }

        [RelayCommand]
        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(RegisterModel.FirstName) || 
                string.IsNullOrWhiteSpace(RegisterModel.LastName) || 
                string.IsNullOrWhiteSpace(RegisterModel.Email) || 
                string.IsNullOrWhiteSpace(RegisterModel.Password))
            {
                SetErrorText("Заполните все поля!", true);
                return;
            }

            if (RegisterModel.Password.Length < 6)
            {
                SetErrorText("Пароль должен содержать минимум 6 символов", true);
                return;
            }

            var (result, error) = await AuthService.SignUp(
                RegisterModel.FirstName, 
                RegisterModel.LastName, 
                RegisterModel.Email, 
                RegisterModel.Password);

            if (result != null) await _navigationService.GoToMainTabsAsync();
            else
            {
                SetErrorText(error, true);
                return;
            }
        }

        [RelayCommand]
        private async Task GoToLogin() => await _navigationService.GoToLoginAsync();
    }
}
