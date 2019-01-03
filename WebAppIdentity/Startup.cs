using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WebAppIdentity.EF;
using WebAppIdentity.Models;


namespace WebAppIdentity
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>(opts =>
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 4;
   
            });

            services.Configure<IdentityOptions>(o =>
            {
                o.SignIn.RequireConfirmedEmail = true;
            });

            services.ConfigureApplicationCookie(opts => {
                opts.LoginPath = "/Account/SignUp";
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes => {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            //CreateRolesAndAdminUser(serviceProvider);
        }

        //private void CreateRolesAndAdminUser(IServiceProvider serviceProvider)
        //{
        //    string[] roleNames = { "Admin", "User" };

        //    foreach (string roleName in roleNames)
        //    {
        //        CreateRole(serviceProvider, roleName);
        //    }
        //    string adminUserEmail = "admin@mail.ru";
        //    string adminPwd = "admin";
        //    AddUserToRole(serviceProvider, adminUserEmail, adminPwd, "Admin");
        //}

        //private void AddUserToRole(IServiceProvider serviceProvider, string userEmail, string userPwd, string roleName)
        //{
        //    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        //    Task<User> checkAppUser = userManager.FindByEmailAsync(userEmail);
        //    checkAppUser.Wait();

        //    User appUser = checkAppUser.Result;

        //    if (checkAppUser.Result == null)
        //    {
        //        User newAppUser = new User
        //        {
        //            Email = userEmail,
        //            UserName = userEmail
        //        };

        //        Task<IdentityResult> taskCreateAppUser = userManager.CreateAsync(newAppUser, userPwd);
        //        taskCreateAppUser.Wait();

        //        if (taskCreateAppUser.Result.Succeeded)  {
        //            appUser = newAppUser;
        //        }
        //    }

        //    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(appUser, roleName);
        //    newUserRole.Wait();
        //}

        //private void CreateRole(IServiceProvider serviceProvider, string roleName)
        //{
        //    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        //    Task<bool> roleExists = roleManager.RoleExistsAsync(roleName);
        //    roleExists.Wait();

        //    if (!roleExists.Result)
        //    {
        //        Task<IdentityResult> roleResult = roleManager.CreateAsync(new IdentityRole(roleName));
        //        roleResult.Wait();
        //    }
        //}
    }
}
