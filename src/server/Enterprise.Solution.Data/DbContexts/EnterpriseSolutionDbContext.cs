using Enterprise.Solution.Data.Models;
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
        public DbSet<EmailSubscription> EmailSubscriptions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            // SEED DATA: Will move to script later on
            var authorList = new Author[] {
                new Author { Id =  1, FirstName = "Rhoda", LastName = "Lerman" },
                new Author { Id =  2, FirstName = "Ruth", LastName = "Ozeki" },
                new Author { Id =  3, FirstName = "Sofia", LastName = "Segovia" },
                new Author { Id =  4, FirstName = "Ursula K.", LastName = "LeGuin" },
                new Author { Id =  5, FirstName = "Isabelle", LastName = "Allende" },
                new Author { Id =  6, FirstName = "Sparkleflight", LastName = "Twirldew" },
                new Author { Id =  7, FirstName = "Cedarbeam", LastName = "Sugarshy" },
                new Author { Id =  8, FirstName = "Starglow", LastName = "Beetlefrost" },
                new Author { Id =  9, FirstName = "Yemima", LastName = "Shulamith" },
                new Author { Id = 10, FirstName = "Orli", LastName = "Hallel" },
                new Author { Id = 11, FirstName = "Shirin", LastName = "Mina" },
                new Author { Id = 12, FirstName = "Golnar", LastName = "Fereshteh" },
                new Author { Id = 13, FirstName = "Negin", LastName = "Parastu" },
            };
            modelBuilder.Entity<Author>().HasData(authorList);

            var someBooks = new Book[] {
                new Book { Id =  1, AuthorId =  1, Title = "In God's Ear",
                    BasePrice =  5.25m, PublishDate = new DateTime(1989, 3, 1).ToUniversalTime() },
                new Book { Id =  2, AuthorId =  2, Title = "A Tale For the Time Being",
                    BasePrice =  7.99m, PublishDate = new DateTime(2013, 12, 31).ToUniversalTime() },
                new Book { Id =  3, AuthorId =  3, Title = "The Left Hand of Darkness",
                    BasePrice = 15.59m, PublishDate = new DateTime(1969, 3, 1).ToUniversalTime()},
                new Book { Id =  4, AuthorId =  3, Title = "The Thorned Sage",
                    BasePrice =  6.66m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id =  5, AuthorId =  4, Title = "Enter the Lie",
                    BasePrice =  4.36m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id =  6, AuthorId =  4, Title = "The Edge of Legend",
                    BasePrice =  9.34m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id =  7, AuthorId =  5, Title = "Prince of Shadows",
                    BasePrice =  6.66m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id =  8, AuthorId =  6, Title = "Plague of Silence",
                    BasePrice = 13.49m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id =  9, AuthorId =  6, Title = "Mystery of the Pock-Marked Librarian",
                    BasePrice = 11.56m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id = 10, AuthorId =  7, Title = "Rules and Roses",
                    BasePrice =  8.75m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id = 11, AuthorId =  7, Title = "Sign of the split Violin",
                    BasePrice = 13.31m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id = 12, AuthorId =  7, Title = "Year of the Maid",
                    BasePrice =  6.79m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id = 13, AuthorId =  8, Title = "Enemies Bound by Fear",
                    BasePrice = 12.47m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id = 14, AuthorId =  8, Title = "2938: Betelgeuese",
                    BasePrice = 29.38m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id = 15, AuthorId =  9, Title = "Trap the Knight",
                    BasePrice = 11.96m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id = 16, AuthorId = 10, Title = "Crime of teh Floridian",
                    BasePrice = 0.01m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id = 17, AuthorId = 11, Title = "Nebula Sinking",
                    BasePrice = 13.56m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id = 18, AuthorId = 12, Title = "Clue of the Hollow Pyramid",
                    BasePrice = 19.99m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
                new Book { Id = 19, AuthorId = 13, Title = "Death of the Three-Inch Lodger",
                    BasePrice = 4.57m, PublishDate = new DateTime(1969, 1, 1).ToUniversalTime()},
            };
            modelBuilder.Entity<Book>().HasData(someBooks);

            var someCovers = new Cover[] {
                new Cover { Id =  3, BookId =  1, DesignIdeas = "A big ear in the clouds?",
                    DigitalOnly = false, ImageUri = "https://media.gettyimages.com/photos/voice-recognition-technology-in-the-cloud-picture-id1176901715" },
                new Cover { Id =  2, BookId =  2, DesignIdeas = "Should we put a clock in a person?",
                    DigitalOnly = true,  ImageUri = "https://elitenp.com/wp-content/uploads/2020/11/time-clock-man-silhouette-business-5439652-1024x682.jpg" },
                new Cover { Id =  1, BookId =  3, DesignIdeas = "How about a left hand in the dark?",
                    DigitalOnly = false, ImageUri = "https://thumbs.dreamstime.com/b/left-hand-print-4788544.jpg" },
                new Cover { Id =  4, BookId =  4, DesignIdeas = "What if the devil was good?",
                    DigitalOnly = false, ImageUri = "https://www.hollywoodoutbreak.com/wp-content/uploads/2018/10/Hellboy_-_First_Look_Image_rgb.jpg" },
                new Cover { Id =  5, BookId =  5, DesignIdeas = "You can't handle the truth!",
                    DigitalOnly = false, ImageUri = "https://i2.wp.com/listverse.com/wp-content/uploads/2012/11/You-cant-handle-the-truth.jpg" },
                new Cover { Id =  6, BookId =  6, DesignIdeas = "What is almost a legend?",
                    DigitalOnly = false, ImageUri = "https://i.pinimg.com/736x/9c/a1/84/9ca184bf2ddb6a3f2a666fb74e521558--emmett-brown-doc-brown.jpg" },
                new Cover { Id =  7, BookId =  7, DesignIdeas = "What if god was bad?",
                    DigitalOnly = false, ImageUri = "https://2.bp.blogspot.com/-m9PzOVscJLY/WLc6pwsZcDI/AAAAAAAAAbk/qO3Om1T5eZgzA_Ig4X-vKLCXjedE-VRRQCLcB/s1600/evil%2Bgod.jpg" },
                new Cover { Id =  8, BookId =  8, DesignIdeas = "A sickness that you can't hear.",
                    DigitalOnly = false, ImageUri = "https://i.pinimg.com/originals/ed/0f/fb/ed0ffb3250f411ac100cf14cedbc9aa3.jpg" },
                new Cover { Id =  9, BookId =  9, DesignIdeas = "Someone stole a librarian...",
                    DigitalOnly = false, ImageUri = "https://m.media-amazon.com/images/M/MV5BYmJmNDM2MzItMTVlNC00ZTRkLTkzMGItN2RlZmIyY2I1MGIyL2ltYWdlXkEyXkFqcGdeQXVyNDcxNzU3MTE@._V1_SY500_CR0,0,767,500_AL_.jpg" },
                new Cover { Id =  10, BookId = 10, DesignIdeas = "Laws and flowers.",
                    DigitalOnly = false, ImageUri = "https://thumbs.dreamstime.com/z/book-roses-old-closed-rose-petal-bed-background-55441413.jpg" },
                new Cover { Id =  11, BookId = 11, DesignIdeas = "What if you saw an instrument ripped in half?",
                    DigitalOnly = false, ImageUri = "https://www.tooveys.com/lots/382988/2560.jpg" },
                new Cover { Id =  12, BookId = 12, DesignIdeas = "A maid cleans for a whole year.",
                    DigitalOnly = false, ImageUri = "https://i.pinimg.com/originals/34/43/ec/3443ecd426b69c2e8917d5b23573ab90.jpg" },
                new Cover { Id =  13, BookId = 13, DesignIdeas = "Rope made of fear?",
                    DigitalOnly = false, ImageUri = "https://image.shutterstock.com/shutterstock/photos/1136096/display_1500/stock-photo-triathlete-bound-my-ropes-1136096.jpg" },
                new Cover { Id =  14, BookId = 14, DesignIdeas = "A Sci-Fi love story.",
                    DigitalOnly = false, ImageUri = "https://i.pinimg.com/originals/e8/6a/06/e86a06e48d8493950a20ba5a8b83197f.jpg" },
                new Cover { Id =  15, BookId = 15, DesignIdeas = "It's a metal dude who ensares darkness.",
                    DigitalOnly = false, ImageUri = "https://hdqwalls.com/wallpapers/the-dark-knight-paint-jw.jpg" },
                new Cover { Id =  16, BookId = 16, DesignIdeas = "Florida Man! 'nough said.",
                    DigitalOnly = false, ImageUri = "https://www.mystateline.com/wp-content/uploads/sites/17/2019/10/Florida-man-Marion-County-Viral.jpg?w=1280" },
                new Cover { Id =  17, BookId = 17, DesignIdeas = "It's a sinking cloud in space.",
                    DigitalOnly = false, ImageUri = "https://en.wikipedia.org/wiki/Nebula#/media/File:Hs-2009-25-e-full.jpg" },
                new Cover { Id =  18, BookId = 18, DesignIdeas = "Someone killed the little guy?",
                    DigitalOnly = false, ImageUri = "https://mymodernmet.com/wp/wp-content/uploads/archive/At0FmCtX6MEUCMkPN9nO_1082012468.jpeg" },
            };
            modelBuilder.Entity<Cover>().HasData(someCovers);

            var someArtists = new Artist[] {
                new Artist { Id = 1, FirstName = "Pablo", LastName = "Picasso" },
                new Artist { Id = 2, FirstName = "Dee", LastName = "Bell" },
                new Artist { Id = 3, FirstName = "Katharine", LastName = "Kuharic" },
                new Artist { Id = 4, FirstName = "Oliver", LastName = "Hardy" },
                new Artist { Id = 5, FirstName = "Ford", LastName = "Chasey" },
                new Artist { Id = 6, FirstName = "Martin", LastName = "Montgomery" },
                new Artist { Id = 7, FirstName = "Verda", LastName = "Francis" },
            };
            modelBuilder.Entity<Artist>().HasData(someArtists);

            var someCoverAssigments = new CoverAssignment[]
            {
                new CoverAssignment { ArtistId = 1, CoverId = 1 },
                new CoverAssignment { ArtistId = 2, CoverId = 1 },
                new CoverAssignment { ArtistId = 2, CoverId = 2 },
                new CoverAssignment { ArtistId = 3, CoverId = 2 },
                new CoverAssignment { ArtistId = 3, CoverId = 3 },
                new CoverAssignment { ArtistId = 3, CoverId = 4 },
                new CoverAssignment { ArtistId = 4, CoverId = 4 },
                new CoverAssignment { ArtistId = 4, CoverId = 5 },
                new CoverAssignment { ArtistId = 5, CoverId = 5 },
                new CoverAssignment { ArtistId = 5, CoverId = 6 },
                new CoverAssignment { ArtistId = 5, CoverId = 7 },
                new CoverAssignment { ArtistId = 5, CoverId = 8 },
                new CoverAssignment { ArtistId = 6, CoverId = 9 },
                new CoverAssignment { ArtistId = 6, CoverId = 10 },
                new CoverAssignment { ArtistId = 6, CoverId = 11 },
                new CoverAssignment { ArtistId = 7, CoverId = 12 },
                new CoverAssignment { ArtistId = 7, CoverId = 13 },
                new CoverAssignment { ArtistId = 7, CoverId = 14 },
                new CoverAssignment { ArtistId = 7, CoverId = 15 },
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

            var emailSubscriptionList = new EmailSubscription[] {
                new EmailSubscription { Id = 1, FirstName = "Rhoda", LastName = "Lerman", EmailAddress = "Rhoda.Lerman@domain.local" },
                new EmailSubscription { Id = 2, FirstName = "Ruth", LastName = "Ozeki", EmailAddress = "Ruth.Ozeki@domain.local" },
                new EmailSubscription { Id = 3, FirstName = "Sofia", LastName = "Segovia", EmailAddress = "Sofia.Segovia@domain.local" },
                new EmailSubscription { Id = 4, FirstName = "Ursula K.", LastName = "LeGuin", EmailAddress = "Ursula.K.LeGuin@domain.local" },
                new EmailSubscription { Id = 5, FirstName = "Hugh", LastName = "Howey", EmailAddress = "Hugh.Howey@domain.local" },
                new EmailSubscription { Id = 6, FirstName = "Isabelle", LastName = "Allende", EmailAddress = "Isabelle.Allende@domain.local" }
            };
            modelBuilder.Entity<EmailSubscription>().HasData(emailSubscriptionList);
        }
    }
}