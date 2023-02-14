namespace Enterprise.Solution.Data.Entities
{
    public class Artist : BaseEntity
    {
        public string FirstName { get; set;}
        public string LastName { get; set;}

        public ICollection<Cover> Covers { get; set; }
        public ICollection<CoverAssignment> CoversAssignments { get; set; } = new HashSet<CoverAssignment>();
    }
}
