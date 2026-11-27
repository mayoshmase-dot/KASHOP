using KASHOP.BLL.Settings;
using Microsoft.AspNetCore.Localization;

namespace KASHOP.PL.Extenstions
{
    public static class LocalizationRegistration
    {
        public static IServiceCollection AddLocalizationConfig  (this IServiceCollection services)
        {

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture(SupportedLanguages.Default);
                options.AddSupportedCultures(SupportedLanguages.Languages);
                options.AddSupportedUICultures(SupportedLanguages.Languages);
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new AcceptLanguageHeaderRequestCultureProvider()
                };
            });

            return services;
        }
    }
}
