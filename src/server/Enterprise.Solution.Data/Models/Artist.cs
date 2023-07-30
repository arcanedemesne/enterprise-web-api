using Enterprise.Solution.Data.Models.Base;

namespace Enterprise.Solution.Data.Models
{
    public class Artist : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public ICollection<Cover> Covers { get; set; }
        public ICollection<CoverAssignment> CoversAssignments { get; set; } = new HashSet<CoverAssignment>();
    }
}
