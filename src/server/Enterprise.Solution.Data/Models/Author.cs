using Enterprise.Solution.Data.Models.Base;
using System.Text.Json.Serialization;

namespace Enterprise.Solution.Data.Models
{
    public class Author : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        [JsonIgnore]
        public ICollection<Book> Books { get; set; }
    }
}
