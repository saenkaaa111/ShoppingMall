using Microsoft.EntityFrameworkCore;

namespace ShoppingMall.Data
{
    public class VisitorsContext : DbContext
    {
        public VisitorsContext() { }
        public VisitorsContext(DbContextOptions<VisitorsContext> options) : base(options) { }
        public DbSet<Visitor> Visitors { get; set; }
                
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Visitor>().ToTable("visitor");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("DefaultConnection"));
            }
        }
    }
}
