using Microsoft.EntityFrameworkCore;
using VerticalSlice.POC.DataAccess.Entities;

namespace VerticalSlice.POC.DataAccess
{
    public class VerticalSliceDbContext : DbContext
    {
        public VerticalSliceDbContext() { }
        public VerticalSliceDbContext(DbContextOptions<VerticalSliceDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
    }
}
