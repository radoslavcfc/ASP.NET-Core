
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Panda.Data;
using Panda.Domain;
using Panda.Services;
using System.Linq;

namespace Panda.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PandaDbContext>(options =>
                options
                .UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<PandaUser, PandaUserRole>()
                .AddRoles<PandaUserRole>()
                .AddEntityFrameworkStores<PandaDbContext>()
                .AddDefaultTokenProviders();
            services.AddTransient<IPackagesService, PackagesService>();
            services.AddTransient<IReceiptsService, ReceiptsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded =context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddControllers();
            services.AddMvc();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                //For Test purposes I have eased the requirements.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;

                options.User.RequireUniqueEmail = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                using(var context = serviceScope.ServiceProvider.GetRequiredService<PandaDbContext>())
                {
                    context.Database.EnsureCreated();

                    if (!context.Roles.Any())
                    {
                        context.Roles.Add(new PandaUserRole { Name = "Admin", NormalizedName = "ADMIN" });
                        context.Roles.Add(new PandaUserRole { Name = "User", NormalizedName = "USER"});
                    }

                    //if(!context.PackageStatus.Any())
                    //{
                    //    context.PackageStatus.Add(new PackageStatus { Name = "Pending" });
                    //    context.PackageStatus.Add(new PackageStatus { Name = "Shipped" });
                    //    context.PackageStatus.Add(new PackageStatus { Name = "Delivered" });
                    //    context.PackageStatus.Add(new PackageStatus { Name = "Acquired" });
                    //}

                    context.SaveChanges();
                }
            }
       
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(ep =>
            {
                ep.MapControllerRoute(name:"usual", pattern: "{controller=Home}/{action=Index}/{id?}");
                ep.MapRazorPages();
            });
        }
    }
}
