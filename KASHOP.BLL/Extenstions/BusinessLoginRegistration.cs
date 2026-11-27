using KASHOP.BLL.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP.BLL.Extenstions
{
    public static class BusinessLoginRegistration
    {
        public static IServiceCollection AddBusinessLoginLayer(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<IEmailService, EmailService>();
            MapsterConfig.RegisterMappngs();
            return services;
        }
    }
}
