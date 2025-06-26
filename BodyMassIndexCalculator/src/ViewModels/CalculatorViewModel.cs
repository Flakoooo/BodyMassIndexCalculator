using BodyMassIndexCalculator.src.Services;
using BodyMassIndexCalculator.src.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BodyMassIndexCalculator.src.ViewModels
{
    public partial class CalculatorViewModel : ObservableObject
    {
        private readonly AuthService _authService;
        private readonly IAPI _api;


        [ObservableProperty]
        private string? _errorText;

        [ObservableProperty]
        private bool _isErrorVisible;

        [ObservableProperty]
        private bool _isResultVisible;

        [ObservableProperty]
        private string? _result;

        [ObservableProperty]
        private string? _recommendation;

        [ObservableProperty]
        private string? _height;

        [ObservableProperty]
        private string? _weight;

        public CalculatorViewModel(AuthService authService, IAPI api) 
        {
            _authService = authService;
            _api = api;
            IsErrorVisible = false;
            ErrorText = string.Empty;
            IsResultVisible = false;
            Result = string.Empty;
            Recommendation = string.Empty;
            Height = string.Empty;
            Weight = string.Empty;
        }

        [RelayCommand]
        private async Task Calculate()
        {
            if (string.IsNullOrWhiteSpace(Height) || string.IsNullOrWhiteSpace(Weight))
            {
                if (string.IsNullOrWhiteSpace(Height) && string.IsNullOrWhiteSpace(Weight))
                    ErrorText = "Заполните все поля!!";
                else if (string.IsNullOrWhiteSpace(Height) && !string.IsNullOrWhiteSpace(Weight))
                    ErrorText = "Поле Рост не заполнено!";
                else if (!string.IsNullOrWhiteSpace(Height) && string.IsNullOrWhiteSpace(Weight))
                    ErrorText = "Поле Вес не заполнено!";

                IsErrorVisible = true;
                return;
            }

            if (int.TryParse(Height, out int height) && int.TryParse(Weight, out int weight))
            {
                double index = Math.Round(weight / Math.Pow((double)height / 100, 2), 2);
                Result = index.ToString();
                Recommendation = GetRecommendation(index);

                var id = _authService.CurrentSession?.User?.Id;
                if (id != null)
                {
                    await _api.CreateCalculation(Guid.Parse(id), height, weight, index, Recommendation);
                }

                IsResultVisible = true;
            }
            return;
        }

        private static string GetRecommendation(double index)
        {
            if (index <= 16.0)
                return "Выраженный дефицит массы тела. Советуем набрать вес для здоровья.";
            else if (index > 16.0 && index <= 18.5)
                return "Недостаточная масса тела. Рекомендуется увеличить массу тела.";
            else if (index > 18.5 && index <= 24.99)
                return "Норма. Ваш вес в здоровом диапазоне — поддерживайте его!";
            else if (index > 24.99 && index <= 30.0)
                return "Избыточная масса тела или предожирение. Желательно снизить вес для улучшения самочувствия.";
            else if (index > 30.0 && index <= 35.0)
                return "Ожирение. Рекомендуется уменьшить вес под контролем специалиста.";
            else if (index > 35.0 && index <= 40.0)
                return "Ожирение резкое. Необходимо снижение веса с медицинской поддержкой.";
            else if (index > 40.0)
                return "Очень резкое ожирение. Требуется срочная коррекция веса под наблюдением врача.";

            return "";
        }
    }
}
