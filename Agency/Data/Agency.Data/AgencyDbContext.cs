
using Agency.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Agency.Web.Data
{
    public class AgencyDbContext : IdentityDbContext<AgencyUser, AgencyUserRole, string>
    {
        public AgencyDbContext(DbContextOptions<AgencyDbContext> options)
            : base(options)
        {
        }
    }
}
