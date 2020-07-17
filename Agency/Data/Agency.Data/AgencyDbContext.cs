using Agency.Data.Infrastructure;
using Agency.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Agency.Web.Data
{
    public class AgencyDbContext : IdentityDbContext<AgencyUser, AgencyUserRole, string>
    {
        public AgencyDbContext(DbContextOptions<AgencyDbContext> options)
            : base(options)
        {
        }

        DbSet<Address> Addresses { get; set; }
        DbSet<ContactInfo> ContactInfos { get; set; }
        DbSet<Names> Names { get; set; }
        DbSet<Nationality> Nationalities { get; set; }
        DbSet<NewWorkerInfo> NewWorkerInfos { get; set; }
        DbSet<ReturneeInfo> ReturneeInfos { get; set; }
        DbSet<Worker> Workers { get; set; }

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
