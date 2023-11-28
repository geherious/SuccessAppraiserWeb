using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SuccessAppraiserWeb.Areas.Identity.models;
using SuccessAppraiserWeb.Data;
using SuccessAppraiserWeb.Data.Goal.Interfaces;
using SuccessAppraiserWeb.Data.Goal.Repositories;
using SuccessAppraiserWeb.Data.Identity;

namespace SuccessAppraiserWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            builder.Host.UseSerilog((ctx, lc) => lc
            .WriteTo.Console());


            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string connectionString;
            Console.WriteLine(environment);
            if (environment == "Development")
            {
                connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }
            else
            {
                connectionString = builder.Configuration.GetConnectionString("MySQLProdConnection") ?? throw new InvalidOperationException("Connection string 'MySQLProdConnection' not found.");
            }
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString,
                new MySqlServerVersion(new Version(8, 0, 33))));
            //options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>().AddUserManager<CustomUserManager>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddControllers();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789?!#-._@+";
                options.User.RequireUniqueEmail = true;
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(14);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            AddDataServices(builder.Services);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "area",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/");
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Seed();
            app.Run();
        }

        public static void AddDataServices(IServiceCollection services)
        {
            services.AddScoped<IGoalRepository, GoalRepository>();
            services.AddScoped<GoalTemplateRepository>();
            services.AddScoped<IGoalTemplateRepository, GoalTemplateCachedRepository>();
        }
    }
}