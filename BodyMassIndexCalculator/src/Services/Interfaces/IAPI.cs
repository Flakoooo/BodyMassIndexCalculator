using BodyMassIndexCalculator.src.Models;
using Supabase.Gotrue;

namespace BodyMassIndexCalculator.src.Services.Interfaces
{
    public interface IAPI
    {
        public Task<IEnumerable<BodyMassIndexCalculation>> GetCalculationsByUserId(Guid userId);
        public Task<BodyMassIndexCalculation?> CreateCalculation(Guid userId, int height, int weight, double bodyMassIndex, string recommendation);
    }
}
