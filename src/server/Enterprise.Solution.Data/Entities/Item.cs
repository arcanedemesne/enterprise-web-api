using Microsoft.EntityFrameworkCore;

namespace Enterprise.Solution.Data.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public Item(string name)
        {
            Name = name;
        }
    }
}