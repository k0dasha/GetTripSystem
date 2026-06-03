using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using static GetTripSystem.DAL;
using GetTripSystem.Entities;
using GetTripSystem.Configurations;


namespace GetTripSystem
{
    public class DAL
    {
        public class Context : DbContext
        {
            public Context(DbContextOptions<Context> options): base(options) {}
            public DbSet<User> Users { get; set; }
            public DbSet<Trip> Trips { get; set; }
            public DbSet<Registration> Registrations { get; set; }
            public DbSet<Picture> Pictures { get; set; }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.ApplyConfiguration(new UserConfiguration());
                modelBuilder.ApplyConfiguration(new TripConfiguration());
                modelBuilder.ApplyConfiguration(new RegistrationConfiguration());
                modelBuilder.ApplyConfiguration(new PictureConfiguration());

                base.OnModelCreating(modelBuilder);
            }
            public void ResetState()
            {
                ChangeTracker.Clear();
            }
        }
    }
}

