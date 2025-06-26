using BodyMassIndexCalculator.src.Services;

namespace BodyMassIndexCalculator
{
    public partial class App : Application
    {
        private readonly AuthService _authService;
        public App(AuthService authService)
        {
            _authService = authService;
            InitializeComponent();

            Task.Run(SupabaseService.Initialize).Wait();
        }
        protected override Window CreateWindow(IActivationState? activationState) => new(new AppShell(_authService));
    }
}