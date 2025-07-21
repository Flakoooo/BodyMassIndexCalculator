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
                Email = string.Empty,
                Password = string.Empty
            };
        }

        [RelayCommand]
        private async Task Login()
        {
            LoginModel.ErrorText = string.Empty;

            bool emailEmpty = string.IsNullOrWhiteSpace(LoginModel.Email);
            bool passwordEmpty = string.IsNullOrWhiteSpace(LoginModel.Password);

            if (emailEmpty || passwordEmpty)
            {
                LoginModel.ErrorText = (emailEmpty, passwordEmpty) switch
                {
                    (true, true) => "Заполните все поля!",
                    (false, true) => "Заполните почту!",
                    (true, false) => "Заполните пароль!",
                    _ => string.Empty
                };
                return;
            }

            var (result, error) = await AuthService.SignIn(LoginModel.Email ?? "", LoginModel.Password ?? "");
            if (result == null) LoginModel.ErrorText = error;
            else
            {
                if (result.User?.Email == LoginModel.Email) await _navigationService.GoToMainTabsAsync();
                else LoginModel.ErrorText = !error.IsNullOrEmpty() ? error : "Неверный email или пароль!";
            }
        }

        [RelayCommand]
        private async Task GoToRegister() => await _navigationService.GoToRegisterAsync();

        //private async Task GoToRegister() => await Task.CompletedTask;
    }
}
