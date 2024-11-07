using Homies.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Homies.Data.Configuration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasOne(e => e.Type)
                .WithMany(t => t.Events)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
