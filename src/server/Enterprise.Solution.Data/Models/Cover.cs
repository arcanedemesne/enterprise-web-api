using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Enterprise.Solution.Data.Models.Base;

namespace Enterprise.Solution.Data.Models
{
    public class Cover : BaseEntity
    {
        public string DesignIdeas { get; set; }
        public bool DigitalOnly { get; set; }

        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        [JsonIgnore]
        public ICollection<Artist> Artists { get; set; }
        [JsonIgnore]
        public ICollection<CoverAssignment> CoverAssignments { get; set; } = new HashSet<CoverAssignment>();
    }
}