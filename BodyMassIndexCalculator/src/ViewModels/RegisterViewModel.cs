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
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                Password = string.Empty

            };
        }

        [RelayCommand]
        private async Task Register()
        {
            try
            {
                RegisterModel.ErrorText = string.Empty;

                bool firstNameEmpty = string.IsNullOrWhiteSpace(RegisterModel.FirstName);
                bool lastNameEmpty = string.IsNullOrWhiteSpace(RegisterModel.LastName);
                bool emailEmpty = string.IsNullOrWhiteSpace(RegisterModel.Email);
                bool passwordEmpty = string.IsNullOrWhiteSpace(RegisterModel.Password);

                if (firstNameEmpty || lastNameEmpty || emailEmpty || passwordEmpty)
                {
                    RegisterModel.ErrorText = (firstNameEmpty, lastNameEmpty, emailEmpty, passwordEmpty) switch
                    {
                        (false, true, true, true) => "Заполните поле имени!",
                        (true, false, true, true) => "Заполните поле фамилии!",
                        (true, true, false, true) => "Заполните поле почты!",
                        (true, true, true, false) => "Заполните поле пароля!",
                        _ => "Заполните все поля!"
                    };
                    return;
                }

                if (RegisterModel.Password != null && RegisterModel.Password.Length < 6)
                {
                    RegisterModel.ErrorText = "Пароль должен содержать минимум 6 символов";
                    return;
                }

                var (result, error) = await AuthService.SignUp(
                    RegisterModel.FirstName ?? "",
                    RegisterModel.LastName ?? "",
                    RegisterModel.Email ?? throw new ArgumentNullException(),
                    RegisterModel.Password ?? throw new ArgumentNullException());

                if (result != null) await GoToLogin();
                else
                {
                    RegisterModel.ErrorText = error;
                    return;
                }
            }
            catch (Exception)
            {
                RegisterModel.ErrorText = "Непредвиденная ошибка";
            }
        }

        [RelayCommand]
        private async Task GoToLogin() => await _navigationService.GoToLoginAsync();
    }
}
