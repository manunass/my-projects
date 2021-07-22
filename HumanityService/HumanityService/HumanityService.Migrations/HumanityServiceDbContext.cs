using System;
using HumanityService.Stores;
using Microsoft.EntityFrameworkCore;

namespace HumanityService.Migrations
{
    /// <summary>
    /// * To add a migration, navigate to the migration project and run:
    ///     dotnet ef migrations add MyMigration
    /// 
    /// * To view resulting SQL script, run:
    ///     dotnet ef migrations script
    /// 
    /// * To apply a migration, run:
    ///     dotnet ef database update
    /// 
    /// To change a migration before applying it, simply delete the corresponding files from visual studio and run (you can also create another migration to apply the changes):
    ///     dotnet ef migration add MyMigration
    /// </summary>
    /// 


    public class LawIndexDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;


            //string connectionString = Environment.GetEnvironmentVariable("HumanityService_SqlDatabaseSettings__ConnectionString");
            string connectionString = "Server=DESKTOP-LSFHGPT;Database=HumanityServiceDb;Trusted_Connection=true";

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("SQL Server connection string not found");
            }
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasKey(e => e.Username);
            modelBuilder.Entity<UserEntity>().Property(e => e.Username).HasMaxLength(36);
            modelBuilder.Entity<UserEntity>().HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<UserEntity>().Property(e => e.Email).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(e => e.LastName).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(e => e.PhoneNumber).HasMaxLength(16).IsRequired();
            modelBuilder.Entity<UserEntity>().Property(e => e.Password).HasMaxLength(128).IsRequired();

            modelBuilder.Entity<NgoEntity>().HasKey(e => e.Username);
            modelBuilder.Entity<NgoEntity>().Property(e => e.Username).HasMaxLength(36);
            modelBuilder.Entity<NgoEntity>().HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<NgoEntity>().Property(e => e.Email).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<NgoEntity>().Property(e => e.Name).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<NgoEntity>().Property(e => e.RegistrationNumber).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<NgoEntity>().Property(e => e.PhoneNumber).HasMaxLength(16).IsRequired();
            modelBuilder.Entity<NgoEntity>().Property(e => e.Password).HasMaxLength(128).IsRequired();

            modelBuilder.Entity<LocationEntity>().HasKey(e => e.Username);

            modelBuilder.Entity<CampaignEntity>().HasKey(e => e.Id);


            modelBuilder.Entity<ProcessEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<ProcessEntity>().HasIndex(e => e.CampaignId);

            modelBuilder.Entity<ContributionEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<ContributionEntity>().HasIndex(e => e.ProcessId);
            modelBuilder.Entity<ContributionEntity>().HasIndex(e => e.DeliveryDemandId);

            modelBuilder.Entity<DeliveryDemandEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<DeliveryDemandEntity>().HasIndex(e => e.ProcessId);
        }
    }
}