using Microsoft.EntityFrameworkCore;
using SubFinder.Models;

namespace SubFinder.Data
{
    public class SubFinderContext : DbContext
    {
        public SubFinderContext(DbContextOptions<SubFinderContext> options) : base(options)
        {
        }

        public DbSet<Sentiment> Sentiment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sentiment>().ToTable("Sentiment");
         
        }

        public DbSet<SubFinder.Models.CustomRule> CustomRule { get; set; }
    }
}
