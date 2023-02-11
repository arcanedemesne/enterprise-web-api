using System.ComponentModel.DataAnnotations.Schema;

namespace Enterprise.Solution.Data.Entities
{
    public class Book : BaseEntity
    {
        public string? Title { get; set; }

        public DateTime PublishDate { get; set; }
        public decimal BasePrice { get; set; }

        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        //[ForeignKey(nameof(AuthorId))]
        //public Author Author { get; set; } = null!

        //public Cover Cover { get; set; }
    }
}
