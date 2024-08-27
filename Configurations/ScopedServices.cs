using agency_portal_api.Services;
using Microsoft.Extensions.DependencyInjection;
using agency_portal_api.Data;

namespace agency_portal_api.Configurations
{
    public static class ScopedServices
    {
        public static IServiceCollection AddScopedServices(this IServiceCollection services) => services
            .AddScoped<IMailJetService, MailJetService>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IAgencyStaffService, AgencyStaffService>()
            .AddScoped<IAgencyService, AgencyService>()
            .AddScoped<IJobSeekerService, JobSeekerService>()
            .AddScoped<IJobDetailService, JobDetailService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IDbTransactionService, DbTransactionService>()
            .AddScoped<IRepository, Repository>();
    }
}
