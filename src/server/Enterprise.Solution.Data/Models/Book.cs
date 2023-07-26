using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Enterprise.Solution.Data.Models.Base;

namespace Enterprise.Solution.Data.Models
{
    public class Book : BaseEntity
    {
        public string Title { get; set; } = null!;

        public DateTime PublishDate { get; set; }
        public decimal BasePrice { get; set; }

        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        [JsonIgnore]
        public Author Author { get; set; } = null!;
        
        [JsonIgnore]
        public Cover Cover { get; set; } = null!;
    }
}
