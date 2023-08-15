using Enterprise.Solution.Data.Models.Base;

namespace Enterprise.Solution.Data.Models
{
    public class Notification : BaseEntity
    {
        public string? Message { get; set; }
        public Guid AssignedTo {  get; set; }
    }
}
