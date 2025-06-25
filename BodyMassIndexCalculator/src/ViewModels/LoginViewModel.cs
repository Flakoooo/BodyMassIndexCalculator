using BodyMassIndexCalculator.src.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BodyMassIndexCalculator.src.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        
        [ObservableProperty]
        private string? _errorText;

        [ObservableProperty]
        private bool _isErrorVisible;


        [ObservableProperty]
        private string? _email;

        [ObservableProperty]
        private string? _password;

        /*
        [ObservableProperty]
        private string _registerFirstName;

        [ObservableProperty]
        private string _registerLastName;

        [ObservableProperty]
        private string _registerEmail;

        [ObservableProperty]
        private string _registerPassword;
        */
        public LoginViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
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

            if (Email == "user@example.com" && Password == "12345")
            {
                IsErrorVisible = false;
                Preferences.Set("IsLoggedIn", true);

                await _navigationService.GoToMainTabsAsync();
            }
            else
            {
                ErrorText = "Неверный email или пароль!";
                IsErrorVisible = true;
            }
        }
    }
}
