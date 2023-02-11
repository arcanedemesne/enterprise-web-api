using Enterprise.Solution.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Solution.Data.DbContexts
{
    public class EnterpriseSolutionDbContext : DbContext
    {
        public EnterpriseSolutionDbContext(DbContextOptions<EnterpriseSolutionDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; } = null!;
        //public DbSet<Book> Books { get; set; }
        //public DbSet<Cover> Covers { get; set; }
        //public DbSet<Artist> Artists { get; set; }
        //public DbSet<AuthorByArtist> AuthorsByArtist { get; set; }

        public DbSet<Item> Items { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Author>().HasKey(a => a.Id);
            //modelBuilder.Entity<Author>().HasMany<Book>();
            //modelBuilder.Entity<Book>().HasKey(b => b.Id);
            //modelBuilder.Entity<Book>().HasOne<Cover>();
            //modelBuilder.Entity<Cover>().HasKey(a => a.Id);
            //modelBuilder.Entity<Cover>().HasMany<Artist>();
            //modelBuilder.Entity<Artist>().HasKey(b => b.Id);
            //modelBuilder.Entity<Artist>().HasMany<Cover>();


            //modelBuilder.Entity<AuthorByArtist>().HasNoKey()
            //    .ToView(nameof(AuthorsByArtist));


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

            var someBooks = new Book[]{
                new Book { Id = 1, AuthorId = 1, Title = "In God's Ear",
                    PublishDate = new DateTime(1989, 3, 1).ToUniversalTime() },
                new Book { Id = 2, AuthorId = 2, Title = "A Tale For the Time Being",
                    PublishDate = new DateTime(2013, 12, 31).ToUniversalTime() },
                new Book { Id = 3, AuthorId = 3, Title = "The Left Hand of Darkness",
                    PublishDate = new DateTime(1969, 3, 1).ToUniversalTime()} };
            modelBuilder.Entity<Book>().HasData(someBooks);

            //var someArtists = new Artist[]{
            //    new Artist {Id = 1, FirstName = "Pablo", LastName="Picasso"},
            //    new Artist {Id = 2, FirstName = "Dee", LastName="Bell"},
            //    new Artist {Id = 3, FirstName = "Katharine", LastName="Kuharic"} };
            //modelBuilder.Entity<Artist>().HasData(someArtists);

            //var someCovers = new Cover[]{
            //    new Cover {Id = 1, BookId = 3, DesignIdeas="How about a left hand in the dark?", DigitalOnly = false},
            //    new Cover {Id = 2, BookId = 2, DesignIdeas= "Should we put a clock?", DigitalOnly = true},
            //    new Cover {Id = 3, BookId = 1, DesignIdeas="A big ear in the clouds?", DigitalOnly = false}};
            //modelBuilder.Entity<Cover>().HasData(someCovers);
        }
    }
}