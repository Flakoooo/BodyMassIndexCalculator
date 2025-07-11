using BodyMassIndexCalculator.src.Models;
using BodyMassIndexCalculator.src.Services.Interfaces;

namespace BodyMassIndexCalculator.src.Services
{
    public class API : IAPI
    {
        public async Task<IEnumerable<BodyMassIndexCalculation>> GetCalculationsByUserId(Guid userId)
        {
            var response = await SupabaseService.Client
                .From<BodyMassIndexCalculation>()
                .Select("created_at, height, weight, body_mass_index, recommendation")
                .Where(bmic => bmic.UserId == userId)
                .Get();

            return response.Models.AsEnumerable();
        }
        public async Task<BodyMassIndexCalculation?> CreateCalculation(Guid userId, int height, int weight, double bodyMassIndex, string recommendation)
        {
            var newCalculation = new BodyMassIndexCalculation
            {
                CreatedAt = DateTime.Now,
                UserId = userId,
                Height = height,
                Weight = weight,
                BodyMassIndex = bodyMassIndex,
                Recommendation = recommendation
            };

            var response = await SupabaseService.Client
                .From<BodyMassIndexCalculation>()
                .Insert(newCalculation);

            return response.Model;
        }
    }
}
