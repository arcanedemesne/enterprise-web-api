using System.Text.Json.Serialization;

namespace Enterprise.Solution.Data.Models
{
    public class CoverAssignment
    {
        [JsonIgnore]
        public Artist Artist { get; set; } = null!;
        public int ArtistId { get; set; }

        [JsonIgnore]
        public Cover Cover { get; set; } = null!;
        public int CoverId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
