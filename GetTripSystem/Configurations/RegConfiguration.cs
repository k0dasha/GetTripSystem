using GetTripSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetTripSystem.Configurations
{
    public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne<Trip>()
                .WithMany()
                .HasForeignKey(r => r.TripID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
