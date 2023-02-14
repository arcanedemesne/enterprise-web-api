namespace Enterprise.Solution.Data.Entities
{
    public class CoverAssignment
    {
        public Artist Artist { get; set; } = null!;
        public int ArtistId { get; set; }

        public Cover Cover { get; set; } = null!;
        public int CoverId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
