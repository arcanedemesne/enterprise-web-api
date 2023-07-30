using System.ComponentModel.DataAnnotations.Schema;
using Enterprise.Solution.Data.Models.Base;

namespace Enterprise.Solution.Data.Models
{
    public class Cover : BaseEntity
    {
        public string DesignIdeas { get; set; }
        public bool DigitalOnly { get; set; }
        public string ImageUri { get; set; }

        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public ICollection<Artist> Artists { get; set; }
        public ICollection<CoverAssignment> CoverAssignments { get; set; } = new HashSet<CoverAssignment>();
    }
}