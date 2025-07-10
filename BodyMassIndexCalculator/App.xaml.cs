using BodyMassIndexCalculator.src.Services;

namespace BodyMassIndexCalculator
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Task.Run(SupabaseService.Initialize).Wait();
        }
        protected override Window CreateWindow(IActivationState? activationState) => new(new AppShell());
    }
}