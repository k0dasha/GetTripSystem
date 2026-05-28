using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetTripSystem.Configurations
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.CreatorID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
