using ChineseAuction.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace ChineseAuction.Data
{
    public class ChinesActionDbContext : DbContext
    {
        public ChinesActionDbContext(DbContextOptions<ChinesActionDbContext> options) : base(options) { }
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Donor> Donors => Set<Donor>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Gift> Gifts => Set<Gift>();
        public DbSet<Purchase> Purchases => Set<Purchase>();
        public DbSet<Package> Packages => Set<Package>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // מפות שמות הטבלאות במסד הנתונים
            modelBuilder.Entity<Category>().ToTable("Categories").HasIndex(u=>u.Name)
                .IsUnique();
            modelBuilder.Entity<Donor>().ToTable("Donors").HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<User>().ToTable("Users").HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<Gift>().ToTable("Gifts");
            modelBuilder.Entity<Purchase>().ToTable("Purchases");
            modelBuilder.Entity<Package>().ToTable("Packages");
            base.OnModelCreating(modelBuilder);
        }
    }
}
