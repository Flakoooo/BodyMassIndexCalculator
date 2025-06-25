namespace BodyMassIndexCalculator
{
    public partial class AppShell : Shell
    {
        public ShellItem MainTabsShellItem => (ShellItem)FindByName("MainTabs");
        public ShellItem LoginPageShellItem => (ShellItem)FindByName("LoginPage");

        public AppShell()
        {
            InitializeComponent();

            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
            //isLoggedIn = false;

            if (isLoggedIn)
                CurrentItem = MainTabsShellItem;
        }
    }
}
