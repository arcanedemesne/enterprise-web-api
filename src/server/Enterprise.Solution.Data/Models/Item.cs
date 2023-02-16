using Enterprise.Solution.Data.Models.Base;

namespace Enterprise.Solution.Data.Models
{
    public class Item : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}