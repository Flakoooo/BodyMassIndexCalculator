using BodyMassIndexCalculator.src.Services;
using BodyMassIndexCalculator.src.Services.Interfaces;
using BodyMassIndexCalculator.src.ViewModels;
using Microsoft.Extensions.Logging;

namespace BodyMassIndexCalculator
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>();

            builder.Services
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<IAPI, API>()
                .AddTransient<LoginViewModel>()
                .AddTransient<RegisterViewModel>()
                .AddTransient<CalculatorViewModel>()
                .AddTransient<ProfileViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
