using System.Reflection.Emit;
using Markerstudy.Lancaster.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Markerstudy.Lancaster.Persistence.Configurations
{
    public class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            //builder.Entity<File>(
            //    entity =>
            //    {
            //        entity.HasKey(e => e.Id);
            //    });
        }
    }
}
