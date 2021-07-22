using GreenCoinService.DataContracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace GreenCoinService.Migrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GreenCoinServiceDbContext>
    {
        public GreenCoinServiceDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../GreenCoinService/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<GreenCoinServiceDbContext>();
            var connectionString = configuration.GetConnectionString("SqlDatabase");
            builder.UseSqlServer(connectionString);
            return new GreenCoinServiceDbContext(builder.Options);
        }
    }

    public class GreenCoinServiceDbContext: DbContext
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

        public GreenCoinServiceDbContext(DbContextOptions<GreenCoinServiceDbContext> options) : base(options)
        {
        }

        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<BagScanEntity> BagScans { get; set; }
        public DbSet<BatchEntity> Batches { get; set; }
        public DbSet<BusinessEntity> Businesses { get; set; }
        public DbSet<CodeEntity> Codes { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<MunicipalityEntity> Municipalities { get; set; }
        public DbSet<RecyclableEntity> Recyclables { get; set; }
        public DbSet<RequestEntity> Requests { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<WalletEntity> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MunicipalityEntity>().HasOne(e => e.Address).WithOne(e => e.Municipality).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<MunicipalityEntity>().HasMany(e => e.Employees).WithOne(e => e.Municipality).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<MunicipalityEntity>().HasMany(e => e.Users).WithOne(e => e.Municipality).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<MunicipalityEntity>().HasMany(e => e.Businesses).WithOne(e => e.Municipality).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<MunicipalityEntity>().HasMany(e => e.Batches).WithOne(e => e.Municipality).OnDelete(DeleteBehavior.ClientCascade);
            

            modelBuilder.Entity<UserEntity>().HasIndex(e => e.PhoneNumber).IsUnique();
            modelBuilder.Entity<UserEntity>().HasIndex(e => e.FirebaseUid).IsUnique();
            modelBuilder.Entity<UserEntity>().HasOne(e => e.Address).WithOne(e => e.User).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<UserEntity>().HasOne(e => e.Wallet).WithOne(e => e.User).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<UserEntity>().HasMany(e => e.Requests).WithOne(e => e.User).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<UserEntity>().HasMany(e => e.Codes).WithOne(e => e.User).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<UserEntity>().HasMany(e => e.BagScans).WithOne(e => e.User).OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<EmployeeEntity>().HasIndex(e => e.PhoneNumber).IsUnique();
            modelBuilder.Entity<EmployeeEntity>().HasIndex(e => e.FirebaseUid).IsUnique();
            modelBuilder.Entity<EmployeeEntity>().HasMany(e => e.BagScans).WithOne(e => e.Employee).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BusinessEntity>().HasIndex(e => e.PhoneNumber).IsUnique();
            modelBuilder.Entity<BusinessEntity>().HasIndex(e => e.FirebaseUid).IsUnique();
            modelBuilder.Entity<BusinessEntity>().HasOne(e => e.Address).WithOne(e => e.Business).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<BusinessEntity>().HasOne(e => e.Wallet).WithOne(e => e.Business).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<BusinessEntity>().HasMany(e => e.Requests).WithOne(e => e.Business).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<BusinessEntity>().HasMany(e => e.Codes).WithOne(e => e.Business).OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<WalletEntity>().HasMany(e => e.Transactions).WithOne(e => e.Wallet).OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<CodeEntity>().HasIndex(e => e.RecyclableId);
            modelBuilder.Entity<CodeEntity>().HasIndex(e => e.BusinessId);
            modelBuilder.Entity<CodeEntity>().HasIndex(e => e.UserId);

            modelBuilder.Entity<RecyclableEntity>().HasMany(e => e.Batches).WithOne(e => e.Recyclable).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<BatchEntity>().HasIndex(e => e.MunicipalityId);
            modelBuilder.Entity<BatchEntity>().HasIndex(e => e.RecyclableId);
            modelBuilder.Entity<BatchEntity>().HasIndex(e => e.Current);
            modelBuilder.Entity<BatchEntity>().HasMany(e => e.BagScans).WithOne(e => e.Batch).OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<BagScanEntity>().HasIndex(e => e.UserId);
            modelBuilder.Entity<BagScanEntity>().HasIndex(e => e.EmployeeId);
            modelBuilder.Entity<BagScanEntity>().HasIndex(e => e.BatchId);
            modelBuilder.Entity<BagScanEntity>().HasIndex(e => e.Processed);
        }
    }
}
