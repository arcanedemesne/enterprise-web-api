using System.ComponentModel.DataAnnotations.Schema;

namespace Enterprise.Solution.Data.Entities
{
    public class Cover : BaseEntity
    {
        public string DesignIdeas { get; set; }
        public bool DigitalOnly { get; set; }

        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public ICollection<Artist> Artists { get; set; }
        public ICollection<CoverAssignment> CoverAssignments { get; set; } = new HashSet<CoverAssignment>();
    }
}