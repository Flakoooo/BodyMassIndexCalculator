using BodyMassIndexCalculator.src.Models;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;

namespace BodyMassIndexCalculator.src.Services
{
    public class AuthService
    {
        private static Session? _currentSession;

        public event EventHandler? SessionUpdated;

        public Session? CurrentSession
        {
            get => _currentSession;
            private set
            {
                _currentSession = value;
                SessionUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task<(Session? Session, string? Error)> SignIn(string email, string password)
        {
            try
            {
                var response = await SupabaseService.Client.Auth.SignIn(email, password);
                CurrentSession = response;
                return (response, null);
            }
            catch (GotrueException gex)
            {
                string userFriendlyError = GetUserFriendlyError(gex);
                return (null, userFriendlyError);
            }
            catch (Exception)
            {
                return (null, "Произошла непредвиденная ошибка");
            }
        }

        public static async Task<(Session? Session, string? Error)> SignUp(string firstName, string lastName, string email, string password)
        {
            try
            {
                var signUpResponse = await SupabaseService.Client.Auth.SignUp(email, password);

                if (signUpResponse?.User?.Id == null)
                    return (null, "Не удалось создать аккаунт");

                var saveProfileResponse = await SaveUserProfile(Guid.Parse(signUpResponse.User.Id), firstName, lastName);

                if (saveProfileResponse != null)
                    return (null, "Не удалось создать профиль");

                return (signUpResponse, null);
            }
            catch (GotrueException gex)
            {
                string userFriendlyError = GetUserFriendlyError(gex);
                await Shell.Current.DisplayAlert("ошиПка",gex.Message,"OK");
                return (null, null);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("ошиПка", ex.Message, "OK");
                return (null, null);
            }
        }

        private static async Task<string?> SaveUserProfile(Guid userId, string firstName, string lastName)
        {
            await SupabaseService.Client
                .From<Profile>()
                .Insert(new Profile
                {
                    UserId = userId,
                    FirstName = firstName,
                    LastName = lastName
                });

            return null;
        }

        public void SignOut()
        {
            SupabaseService.Client.Auth.SignOut();
            CurrentSession = null;
        }

        private static string GetUserFriendlyError(GotrueException gex)
        {
            if (gex.StatusCode != 0)
            {
                return gex.StatusCode switch
                {
                    400 => "Некорректные данные",
                    401 => "Ошибка авторизации",
                    404 => "Ресурс не найден",
                    422 => "Данные не могут быть обработаны",
                    500 => "Ошибка на сервере",
                    _ => $"Произошла непредвиденная ошибка (код: {gex.StatusCode})"
                };
            }

            // Если статуса нет, анализируем сообщение
            return "Произошла непредвиденная ошибка";
        }
    }
}
