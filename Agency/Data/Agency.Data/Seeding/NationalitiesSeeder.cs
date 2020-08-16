using Agency.Data.Models;
using Agency.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agency.Data.Seeding
{
    public class NationalitiesSeeder : ISeeder
    {
        public async Task SeedAsync(AgencyDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Nationalities.Any())
            {
                return;
            }

            var nationalities = new List<string>
            {
               "British","Bulgarian","Romanian"
            };

            foreach (var nationality in nationalities)
            {
                await dbContext.Nationalities.AddAsync(new Nationality
                {
                    NationalityCountry = nationality
                }) ;
            }
        }
    }
}
