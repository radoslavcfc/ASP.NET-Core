using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeasonalWorkers.Data;
using SeasonalWorkers.Data.Models;

namespace SeasonalWorkers
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SeasonalWorkersDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, UserRole>()
               .AddRoles<UserRole>()
               .AddEntityFrameworkStores<SeasonalWorkersDbContext>()
               .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<SeasonalWorkersDbContext>();


            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("Home/StatusCode?code={0}");
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //Seeding the roles in the database when initializing the application
            //using (var serviceScope = app.ApplicationServices.CreateScope())
            //{
            //    using (var context = serviceScope.ServiceProvider.GetRequiredService<SeasonalWorkersDbContext>())
            //    {
            //        context.Database.EnsureCreated();

            //        if (!context.Roles.Any())
            //        {
            //            context.Roles.Add(new UserRole { Name = "Admin", NormalizedName = "ADMIN" });
            //            context.Roles.Add(new UserRole { Name = "User", NormalizedName = "USER" });
            //        }
            //        context.SaveChanges();
            //    }
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(ep =>
            {
                ep.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

                ep.MapRazorPages();
            });
        }
    }
    }

