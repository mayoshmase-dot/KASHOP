using KASHOP.DAL.Data;
using KASHOP.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KASHOP.DAL.Extensions
{
    public static class DataAccessRegistration
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IGenericRepository<>) , typeof(GenericRepository<>));
            services.AddHttpContextAccessor();

            return services;
        }
        
    }
}
