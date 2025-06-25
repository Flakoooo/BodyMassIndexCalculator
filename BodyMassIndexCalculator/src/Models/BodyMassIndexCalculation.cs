namespace BodyMassIndexCalculator.src.Models
{
    public class BodyMassIndexCalculation
    {
        public DateTime CreatedAt { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public double BodyMassIndex { get; set; }
        public string? Recommendation { get; set; }
    }
}
