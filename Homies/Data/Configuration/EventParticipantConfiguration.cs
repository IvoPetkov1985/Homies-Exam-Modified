using Homies.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Homies.Data.Configuration
{
    public class EventParticipantConfiguration : IEntityTypeConfiguration<EventParticipant>
    {
        public void Configure(EntityTypeBuilder<EventParticipant> builder)
        {
            builder.HasKey(ep => new
            {
                ep.HelperId,
                ep.EventId
            });

            builder.HasOne(ep => ep.Helper)
                .WithMany()
                .HasForeignKey(ep => ep.HelperId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
