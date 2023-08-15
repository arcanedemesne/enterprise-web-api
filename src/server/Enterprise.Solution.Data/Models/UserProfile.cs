using System.ComponentModel.DataAnnotations.Schema;
using Enterprise.Solution.Data.Models.Base;

namespace Enterprise.Solution.Data.Models
{
    public class UserProfile : BaseEntity
    {
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string Description { get; set; }

        public string Status { get; set; }

        public string ProfilePicUri { get; set; }
    }
}
