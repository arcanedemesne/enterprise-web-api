using Enterprise.Solution.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Solution.Data.DbContexts
{
    public class EnterpriseSolutionDbContext : DbContext
    {
        public EnterpriseSolutionDbContext(DbContextOptions<EnterpriseSolutionDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Book> Books { get; set; }
        public DbSet<Cover> Covers { get; set; }
        public DbSet<Artist> Artists { get; set; }
        //public DbSet<AuthorByArtist> AuthorsByArtist { get; set; }

        public DbSet<Item> Items { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // SEED DATA: Will move to script later on
            var authorList = new Author[] {
                new Author { Id = 1, FirstName = "Rhoda", LastName = "Lerman" },
                new Author { Id = 2, FirstName = "Ruth", LastName = "Ozeki" },
                new Author { Id = 3, FirstName = "Sofia", LastName = "Segovia" },
                new Author { Id = 4, FirstName = "Ursula K.", LastName = "LeGuin" },
                new Author { Id = 5, FirstName = "Hugh", LastName = "Howey" },
                new Author { Id = 6, FirstName = "Isabelle", LastName = "Allende" }
            };
            modelBuilder.Entity<Author>().HasData(authorList);

            var someBooks = new Book[] {
                new Book { Id = 1, AuthorId = 1, Title = "In God's Ear",
                    PublishDate = new DateTime(1989, 3, 1).ToUniversalTime() },
                new Book { Id = 2, AuthorId = 2, Title = "A Tale For the Time Being",
                    PublishDate = new DateTime(2013, 12, 31).ToUniversalTime() },
                new Book { Id = 3, AuthorId = 3, Title = "The Left Hand of Darkness",
                    PublishDate = new DateTime(1969, 3, 1).ToUniversalTime()}
            };
            modelBuilder.Entity<Book>().HasData(someBooks);

            var someCovers = new Cover[] {
                new Cover { Id = 1, BookId = 3, DesignIdeas = "How about a left hand in the dark?", DigitalOnly = false },
                new Cover { Id = 2, BookId = 2, DesignIdeas = "Should we put a clock?", DigitalOnly = true },
                new Cover { Id = 3, BookId = 1, DesignIdeas = "A big ear in the clouds?", DigitalOnly = false }
            };
            modelBuilder.Entity<Cover>().HasData(someCovers);

            var someArtists = new Artist[] {
                new Artist { Id = 1, FirstName = "Pablo", LastName = "Picasso" },
                new Artist { Id = 2, FirstName = "Dee", LastName = "Bell" },
                new Artist { Id = 3, FirstName = "Katharine", LastName = "Kuharic" } };
            modelBuilder.Entity<Artist>().HasData(someArtists);

            var someCoverAssigments = new CoverAssignment[]
            {
                new CoverAssignment { ArtistId = 1, CoverId = 1 },
                new CoverAssignment { ArtistId = 2, CoverId = 1 },
                new CoverAssignment { ArtistId = 2, CoverId = 2 },
                new CoverAssignment { ArtistId = 3, CoverId = 2 },
                new CoverAssignment { ArtistId = 3, CoverId = 3 },
            };

            modelBuilder.Entity<Artist>()
            .HasMany(a => a.Covers)
            .WithMany(c => c.Artists)
            .UsingEntity<CoverAssignment>(
                j => j
                    .HasOne(a => a.Cover)
                    .WithMany(c => c.CoverAssignments)
                    .HasForeignKey(ca => ca.CoverId),
                j => j
                    .HasOne(c => c.Artist)
                    .WithMany(a => a.CoversAssignments)
                    .HasForeignKey(ca => ca.ArtistId),
                j =>
                {
                    j.Property(ca => ca.DateCreated).HasDefaultValueSql("CURRENT_TIMESTAMP");
                    j.HasKey(t => new { t.ArtistId, t.CoverId });
                    j.HasData(someCoverAssigments);
                });
        }
    }
}