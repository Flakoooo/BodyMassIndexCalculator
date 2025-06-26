using BodyMassIndexCalculator.src.Services;
using BodyMassIndexCalculator.src.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.IdentityModel.Tokens;

namespace BodyMassIndexCalculator.src.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly AuthService _authService;
        
        [ObservableProperty]
        private string? _errorText;

        [ObservableProperty]
        private bool _isErrorVisible;

        [ObservableProperty]
        private string? _email;

        [ObservableProperty]
        private string? _password;

        public LoginViewModel(INavigationService navigationService, AuthService authService)
        {
            _navigationService = navigationService;
            _authService = authService;
            ErrorText = string.Empty;
            IsErrorVisible = false;
            Email = string.Empty;
            Password = string.Empty;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorText = "Заполните все поля!";
                IsErrorVisible = true;
                return;
            }

            var (result, error) = await _authService.SignIn(Email, Password);
            if (result == null)
            {
                ErrorText = error;
                IsErrorVisible = true;
            }
            else
            {
                if (result.User?.Email == Email)
                {
                    IsErrorVisible = false;
                    await _navigationService.GoToMainTabsAsync();
                }
                else
                {
                    ErrorText = !error.IsNullOrEmpty() ? error : "Неверный email или пароль!";
                    IsErrorVisible = true;
                }
            }
        }

        [RelayCommand]
        private async Task GoToRegister() => await _navigationService.GoToRegisterAsync();
    }
}
