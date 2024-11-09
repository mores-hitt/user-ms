using Microsoft.EntityFrameworkCore;
using user_ms.Src.Models;

namespace user_ms.Src.Data
{
    public class DataContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Career> Careers { get; set; }

        public DbSet<UserProgress> UserProgresses { get; set; }

        public DbSet<SubjectRelationship> SubjectRelationships { get; set; }

        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
        }
        
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseModel && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseModel)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                }
            }
        }
    }

}
