using KASHOP.BLL;
using KASHOP.BLL.Settings;
using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repositories;
using KASHOP.PL.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace KASHOP.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture(SupportedLanguages.Default);
                options.AddSupportedCultures(SupportedLanguages.Languages);
                options.AddSupportedUICultures(SupportedLanguages.Languages);
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new AcceptLanguageHeaderRequestCultureProvider()
                };
            });   

            builder.Services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
            builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISeedData, RoleSeedData>();
            builder.Services.AddScoped<ISeedData, RoleSeedData>();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredUniqueChars = 1;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            MapsterConfig.RegisterMappngs();

            var app = builder.Build();

            app.UseRequestLocalization(
                app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseStaticFiles();

            using (var scope = app.Services.CreateScope())
            {
                var seeders = scope.ServiceProvider.GetServices<ISeedData>();    

                foreach (var seeder in seeders)
                {
                    await seeder.DataSeed();
                }
            }
            app.Run();
        }
    }
}