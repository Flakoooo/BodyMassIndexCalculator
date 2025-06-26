namespace BodyMassIndexCalculator.src.Services.Interfaces
{
    public interface INavigationService
    {
        public Task GoToLoginAsync();
        public Task GoToMainTabsAsync();
        public Task GoToRegisterAsync();
    }
}
