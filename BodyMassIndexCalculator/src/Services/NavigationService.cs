namespace BodyMassIndexCalculator.src.Services
{
    public class NavigationService : INavigationService
    {
        public async Task GoToLoginAsync() => await Shell.Current.GoToAsync("//LoginPage");
        public async Task GoToMainTabsAsync() => await Shell.Current.GoToAsync("//MainTabs");
    }
}
