using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalWebsite.Auth;
using PersonalWebsite.GithubService;
using PersonalWebsite.Models;
using System;
using System.Threading.Tasks;

namespace PersonalWebsite
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
            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddAreaPageRoute("Projects", "/Project_Detail", "Projects/Detail/{id}");
                    options.Conventions.AddAreaPageRoute("Projects", "/Project_Edit", "Projects/Edit/{id}");
                });

            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("UserContext")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();


            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Login";
                options.AccessDeniedPath = $"/Identity/AccessDenied";
            });


            services.AddHttpClient<GithubClient>();
            services.AddHostedService<GithubUpdateService>();
            services.AddDbContext<GithubRepositoryContext>(options => options.UseSqlServer(Configuration.GetConnectionString("GithubRepositoryContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            CreateRolesAsync(serviceProvider).Wait();
        }

        private async Task CreateRolesAsync(IServiceProvider serviceProvider)
        {
            var adminData = new AdminData();
            Configuration.GetSection("AdminData").Bind(adminData);

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Member" };
            IdentityResult roleResult;

            foreach (string name in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(name);

                if (!roleExists)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(name));
                }
            }

            var adminUser = await userManager.FindByEmailAsync(adminData.Email);

            if (adminUser == null)
            {
                var powerUser = new ApplicationUser
                {
                    UserName = adminData.Username,
                    Email = adminData.Email,
                };

                var createPowerUser = await userManager.CreateAsync(powerUser, adminData.Password);

                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(powerUser, "Admin");
                }
            }
        }
    }

    class AdminData
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
