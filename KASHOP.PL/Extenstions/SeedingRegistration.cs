using KASHOP.PL.Utils;

namespace KASHOP.PL.Extenstions
{
    public static class SeedingRegistration
    {
        public static IServiceCollection AddSeeders(this IServiceCollection services)
        {
            services.AddScoped<ISeedData, RoleSeedData>();
            return services;
        }

    }
}
