using Microsoft.EntityFrameworkCore;
using NewsBiasChecker.Models;

namespace NewsBiasChecker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Roundup> Roundups => Set<Roundup>();
        public DbSet<Story> Stories => Set<Story>();

        public DbSet<NewsStory> NewsStories => Set<NewsStory>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            // Unique constraint so you don’t import the same roundup twice (same Title + Date)
            b.Entity<Roundup>()
             .HasIndex(r => new { r.Title, r.Date })
             .IsUnique();

            // Ensure only one story per Side for each Roundup
            b.Entity<Story>()
             .HasIndex(s => new { s.RoundupId, s.Side })
             .IsUnique();

            b.Entity<NewsStory>()
             .HasIndex(n => n.Url)
             .IsUnique(false);

            base.OnModelCreating(b);
        }
    }
}

