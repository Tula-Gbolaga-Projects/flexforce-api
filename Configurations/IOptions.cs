using agency_portal_api.DTOs;
using agency_portal_api.DTOs.ServiceDtos;

namespace agency_portal_api.Configurations
{
    public static class IOptions
    {
        public static IServiceCollection ConfigureAppSetting(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<JWT>(configuration.GetSection("JWT"));
        }
    }
}
