using BodyMassIndexCalculator.src.Services;

namespace BodyMassIndexCalculator
{
    public partial class AppShell : Shell
    {
        public ShellItem MainTabsShellItem => (ShellItem)FindByName("MainTabs");
        public ShellItem LoginPageShellItem => (ShellItem)FindByName("LoginPage");
        public ShellItem RegisterPageShellItem => (ShellItem)FindByName("RegisterPage");

        public AppShell(AuthService authService)
        {
            InitializeComponent();

            if (authService.CurrentSession != null)
            {
                CurrentItem = MainTabsShellItem;
            }
        }
    }
}
