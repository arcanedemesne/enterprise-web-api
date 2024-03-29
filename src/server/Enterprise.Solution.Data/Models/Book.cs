﻿using System.ComponentModel.DataAnnotations.Schema;
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

        public Author Author { get; set; } = null!;
        
        public Cover Cover { get; set; } = null!;
    }
}
