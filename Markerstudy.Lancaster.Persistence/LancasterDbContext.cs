using System;
using System.Threading;
using System.Threading.Tasks;
using Markerstudy.Lancaster.Domain.Common;
using Markerstudy.Lancaster.Domain.Models;
using Markerstudy.Lancaster.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Markerstudy.Lancaster.Persistence
{
    public class LancasterDbContext : DbContext
    {
        public LancasterDbContext(DbContextOptions<LancasterDbContext> options)
            : base(options)
        {
        }

        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LancasterDbContext).Assembly);            
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        break;
                    default:
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
