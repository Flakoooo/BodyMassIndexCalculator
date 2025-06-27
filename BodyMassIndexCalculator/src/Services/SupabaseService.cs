using Supabase;

namespace BodyMassIndexCalculator.src.Services
{
    public static class SupabaseService
    {
        private static readonly string SupabaseUrl = "https://seivcpxkjsesvasrrawq.supabase.co";
        private static readonly string SupabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNlaXZjcHhranNlc3Zhc3JyYXdxIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTA5MzMxNzMsImV4cCI6MjA2NjUwOTE3M30.pgaHPlPBqP5dvgcU4ZnL6d3Xvolw6bRhJqitMiVoqoc";

        public static Client Client { get; } = new Client(SupabaseUrl, SupabaseKey);

        public static async Task Initialize() => await Client.InitializeAsync();
    }
}
