using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BodyMassIndexCalculator.src.Models
{
    [Table("profiles")]
    public class Profile : BaseModel
    {
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }
    }
}
