using Microsoft.Extensions.Configuration;
using Supabase;
using System.Reflection;

namespace BodyMassIndexCalculator.src.Services
{
    public static class SupabaseService
    {
        private const string ConfigFileName = "appsettings.json";

        private static Stream LoadConfigFile()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                var appdata = FileSystem.AppDataDirectory;
                var configPath = Path.Combine(appdata, ConfigFileName);
                if (File.Exists(configPath))
                    return File.OpenRead(configPath);
                return Stream.Null;
            }

            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream($"BodyMassIndexCalculator.{ConfigFileName}") ?? Stream.Null;
        }

        private static string GetSupabaseUrl()
        {
            using var stream = LoadConfigFile();
            if (stream == Stream.Null)
                throw new FileNotFoundException("Конфигурационный файл не найден");

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            return config["Supabase:Url"] ?? "";
        }

        private static string GetSupabaseKey()
        {
            using var stream = LoadConfigFile();
            if (stream == Stream.Null)
                throw new FileNotFoundException("Конфигурационный файл не найден");

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            return config["Supabase:AnonKey"] ?? "";
        }

        public static Client Client { get; } = new Client(GetSupabaseUrl(), GetSupabaseKey());

        public static async Task Initialize() => await Client.InitializeAsync();
    }
}
