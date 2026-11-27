using KASHOP.BLL.Extenstions;
using KASHOP.DAL.Extensions;
using KASHOP.PL.Extenstions;
using KASHOP.PL.Utils;
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

            builder.Services.AddDataAccessLayer(builder.Configuration);
            builder.Services.AddBusinessLoginLayer();
            builder.Services.AddAuthonticationConfig(builder.Configuration);
            builder.Services.AddLocalizationConfig();
            builder.Services.AddSeeders();


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