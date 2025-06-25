using BodyMassIndexCalculator.src.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BodyMassIndexCalculator.src.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private ObservableCollection<BodyMassIndexCalculation> _bodyMassIndexCalculations;

        public ProfileViewModel()
        {
            Name = "Имя Фамилия";
            Email = "user@example.com";

            IEnumerable<BodyMassIndexCalculation> bodyMassIndexCalculations = 
                [
                    new BodyMassIndexCalculation { CreatedAt = DateTime.ParseExact("25.06.2025 15:30:00", "dd.MM.yyyy HH:mm:ss", null), Height = 185, Weight = 77, BodyMassIndex = 22.5, Recommendation = "Норма. Ваш вес в здоровом диапазоне — поддерживайте его!" },
                    new BodyMassIndexCalculation { CreatedAt = DateTime.ParseExact("24.06.2025 14:27:35", "dd.MM.yyyy HH:mm:ss", null), Height = 178, Weight = 80, BodyMassIndex = 25.25, Recommendation = "Избыточная масса тела или предожирение. Желательно снизить вес для улучшения самочувствия." },
                    new BodyMassIndexCalculation { CreatedAt = DateTime.ParseExact("23.06.2025 14:27:35", "dd.MM.yyyy HH:mm:ss", null), Height = 184, Weight = 56, BodyMassIndex = 16.54, Recommendation = "Недостаточная масса тела. Рекомендуется увеличить массу тела." },
                ];
            BodyMassIndexCalculations = new ObservableCollection<BodyMassIndexCalculation>(bodyMassIndexCalculations);
        }
    }
}
