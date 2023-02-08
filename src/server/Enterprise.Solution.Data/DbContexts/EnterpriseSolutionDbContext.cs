using Enterprise.Solution.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Solution.Data.DbContexts
{
    public class EnterpriseSolutionDbContext : DbContext
    {
        public EnterpriseSolutionDbContext(DbContextOptions<EnterpriseSolutionDbContext> options) : base(options) { }


        public DbSet<Item> Items { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}