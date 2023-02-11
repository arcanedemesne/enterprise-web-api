using Microsoft.EntityFrameworkCore;

namespace Enterprise.Solution.Data.Entities
{
    [PrimaryKey("Id")]
    public abstract class BaseEntity
    {
        public int Id { get; internal set; }
    }
}