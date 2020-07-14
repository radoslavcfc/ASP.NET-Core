using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using SeasonalWorkers.Data.Models;

namespace SeasonalWorkers.Data
{
    public class SeasonalWorkersDbContext : IdentityDbContext<User, UserRole, string>
    {
        public SeasonalWorkersDbContext(DbContextOptions<SeasonalWorkersDbContext> options)
            : base(options)
        {
        }
    }
}
