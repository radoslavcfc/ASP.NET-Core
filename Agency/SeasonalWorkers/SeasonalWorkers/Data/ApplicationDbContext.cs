using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SeasonalWorkers.Data
{
    public class SeasonalWorkersDbContext : IdentityDbContext
    {
        public SeasonalWorkersDbContext(DbContextOptions<SeasonalWorkersDbContext> options)
            : base(options)
        {
        }
    }
}
