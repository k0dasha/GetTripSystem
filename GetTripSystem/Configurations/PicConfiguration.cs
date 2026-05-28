using GetTripSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetTripSystem.Configurations
{
    public class PictureConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne<Trip>()
                .WithMany()
                .HasForeignKey(p => p.TripID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
