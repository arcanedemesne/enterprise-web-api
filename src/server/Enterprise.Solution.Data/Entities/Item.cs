namespace Enterprise.Solution.Data.Entities
{
    public class Item : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}