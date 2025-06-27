using BodyMassIndexCalculator.src.Services;
using BodyMassIndexCalculator.src.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BodyMassIndexCalculator.src.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly AuthService _authService;

        [ObservableProperty]
        private string? _errorText;

        [ObservableProperty]
        private bool _isErrorVisible;

        [ObservableProperty]
        private string _firstName;

        [ObservableProperty] 
        private string _lastName;
        
        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        public RegisterViewModel(INavigationService navigationService, AuthService authService)
        {
            _navigationService = navigationService;
            _authService = authService;
            ErrorText = string.Empty;
            IsErrorVisible = false;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }

        [RelayCommand]
        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(FirstName) || 
                string.IsNullOrWhiteSpace(LastName) || 
                string.IsNullOrWhiteSpace(Email) || 
                string.IsNullOrWhiteSpace(Password))
            {
                ErrorText = "Заполните все поля!";
                IsErrorVisible = true;
                return;
            }

            if (Password.Length < 6)
            {
                ErrorText = "Пароль должен содержать минимум 6 символов";
                IsErrorVisible = true;
                return;
            }

            var (result, error) = await _authService.SignUp(FirstName, LastName, Email, Password);

            if (result != null)
                await _navigationService.GoToMainTabsAsync();
            else
            {
                ErrorText = error;
                IsErrorVisible = true;
                return;
            }
        }

        [RelayCommand]
        private async Task GoToLogin() => await _navigationService.GoToLoginAsync();
    }
}
