using agency_portal_api.Data;
using agency_portal_api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using agency_portal_api.Options;
using PasswordOptions = agency_portal_api.Options.PasswordOptions;

namespace agency_portal_api.Configurations
{
    public static class BaseIdentity
    {
        public static IServiceCollection ConfigurePasswordOptions(this IServiceCollection services)
        {
            PasswordOptions _passwordOptions = new()
            {
                RequiredLength = 8,
                RequireDigit = true,
                RequireLowercase = false,
                RequireUppercase = false,
                RequireNonAlphanumeric = false
            };
           return  services.AddSingleton(new PasswordValidator(_passwordOptions));
        }
    }
}

