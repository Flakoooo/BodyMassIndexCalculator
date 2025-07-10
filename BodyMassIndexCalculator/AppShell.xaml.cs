using BodyMassIndexCalculator.src.Services;

namespace BodyMassIndexCalculator
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            SetNavBarIsVisible(this, false);

            if (SupabaseService.Client.Auth.CurrentUser != null)
                CurrentItem = (ShellItem)FindByName("MainTabs");
        }
    }
}
