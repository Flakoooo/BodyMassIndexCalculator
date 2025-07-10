using BodyMassIndexCalculator.src.Services;
using BodyMassIndexCalculator.src.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.IdentityModel.Tokens;

namespace BodyMassIndexCalculator.src.ViewModels
{
    public partial class LoginModel : ObservableObject
    {
        [ObservableProperty]
        private string? _errorText;

        [ObservableProperty]
        private bool _isErrorVisible;

        [ObservableProperty]
        private string? _email;

        [ObservableProperty]
        private string? _password;
    }

    public partial class LoginViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private LoginModel _loginModel;

        public LoginViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            LoginModel = new LoginModel
            {
                ErrorText = string.Empty,
                IsErrorVisible = false,
                Email = string.Empty,
                Password = string.Empty
            };
        }

        private void SetErrorText(string? error, bool visible)
        {
            LoginModel.ErrorText = error;
            LoginModel.IsErrorVisible = visible;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(LoginModel.Email) || 
                string.IsNullOrWhiteSpace(LoginModel.Password))
            {
                SetErrorText("Заполните все поля!", true);
                return;
            }

            var (result, error) = await AuthService.SignIn(LoginModel.Email, LoginModel.Password);
            if (result == null) SetErrorText(error, true);
            else
            {
                if (result.User?.Email == LoginModel.Email)
                {
                    LoginModel.IsErrorVisible = false;
                    await _navigationService.GoToMainTabsAsync();
                }
                else
                    SetErrorText(!error.IsNullOrEmpty() ? error : "Неверный email или пароль!", true);
            }
        }

        [RelayCommand]
        private async Task GoToRegister() => await _navigationService.GoToRegisterAsync();
    }
}
