using Newtonsoft.Json;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BodyMassIndexCalculator.src.Models
{
    [Table("body_mass_index_calculations")]
    public class BodyMassIndexCalculation : BaseModel
    {
        [PrimaryKey("user_id", false)]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("height")]
        public int Height { get; set; }

        [Column("weight")]
        public int Weight { get; set; }

        [Column("body_mass_index")]
        public double BodyMassIndex { get; set; }

        [Column("recommendation")]
        public string? Recommendation { get; set; }
    }
}
