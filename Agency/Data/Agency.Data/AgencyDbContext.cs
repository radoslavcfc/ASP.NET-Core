
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Agency.Web.Data
{
    public class AgencyDbContext : IdentityDbContext
    {
        public AgencyDbContext(DbContextOptions<AgencyDbContext> options)
            : base(options)
        {
        }
    }
}
