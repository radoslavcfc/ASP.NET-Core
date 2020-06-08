
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Panda.Data;
using Panda.Domain;
using Panda.Services;
using System.IO;
using System.Linq;


namespace PandaWeb
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
                .UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"),
                                b => b.MigrationsAssembly("PandaWeb")));
            services.AddIdentity<PandaUser, PandaUserRole>()
                .AddRoles<PandaUserRole>()
                .AddEntityFrameworkStores<PandaDbContext>()
                .AddDefaultTokenProviders();
            services.AddTransient<IPackagesService, PackagesService>();
            services.AddTransient<IReceiptsService, ReceiptsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IAddressesService, AddressesService>();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded =context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddAuthentication("CookieAuthentication")
                 .AddCookie("CookieAuthentication", config =>
                 {
                     config.Cookie.Name = "UserLoginCookie";
                     config.LoginPath = "/Login/UserLogin";
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
             app.UseExceptionHandler("/Home/Error");
            //if (env.IsDevelopment())
            //{
            //   // app.UseStatusCodePagesWithRedirects("Home/StatusCode?code={0}");
            //    app.UseExceptionHandler("/Addresses/Index");
            //}
            // app.UseStatusCodePagesWithRedirects("Home/StatusCode?code={0}");

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseDatabaseErrorPage();
            //    app.UseExceptionHandler("/Error");
            //}
            //else
            //{
            //    app.UseStatusCodePagesWithRedirects("Home/StatusCode?code={0}");
            //    app.UseExceptionHandler("/Error");
            //    app.UseHsts();
            //}
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                using(var context = serviceScope.ServiceProvider.GetRequiredService<PandaDbContext>())
                {
                    context.Database.EnsureCreated();

                    if (!context.Roles.Any())
                    {
                        context.Roles.Add(new PandaUserRole { Name = "Admin", NormalizedName = "ADMIN" });
                        context.Roles.Add(new PandaUserRole { Name = "User", NormalizedName = "USER"});
                    }
                    context.SaveChanges();
                }
            }
       
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseCookiePolicy();
            
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(ep =>
            {
                ep.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}") ;
               
                ep.MapRazorPages();
            });
        }
    }
}
