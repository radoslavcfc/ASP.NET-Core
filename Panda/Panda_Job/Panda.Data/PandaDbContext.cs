﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Panda.Domain;

namespace Panda.Data
{
    public class PandaDbContext : IdentityDbContext<PandaUser, PandaUserRole, string>
    {
        public DbSet<Package> Packages { get; set; }

        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Flat> Flats { get; set; }
        public PandaDbContext(DbContextOptions<PandaDbContext> options) : base(options)
        {
           
        }
        public PandaDbContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PandaUser>()
                .HasKey(user => user.Id);

            //Used EF Fluent API to define relations with the customized user. 
            //An address property has been added later with migration direclty
            
           /* builder.Entity<PandaUser>()
                .HasMany(user => user.Packages)
                .WithOne(package => package.Recipient)
                .HasForeignKey(package => package.RecipientId);

            builder.Entity<PandaUser>()
                .HasMany(user => user.Receipts)
                .WithOne(receipt => receipt.Recipient)                
                .HasForeignKey(receipt => receipt.RecipientId);

            builder.Entity<Receipt>()
                .HasOne(receipt => receipt.Package)
                .WithOne()                
                .OnDelete(DeleteBehavior.Restrict);*/

            //Explicitly stated type to avoid the SQL error
            builder.Entity<Receipt>()
               .Property("Fee")
               .HasColumnType("Decimal");

            base.OnModelCreating(builder);
        }
    }
}
