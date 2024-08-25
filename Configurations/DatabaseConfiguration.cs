using agency_portal_api.Data;
using Microsoft.EntityFrameworkCore;

namespace agency_portal_api.Configurations
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, string connectionString)
        {
            return
            services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        }
    }
}
