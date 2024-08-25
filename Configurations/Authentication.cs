using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using agency_portal_api.DTOs;

namespace agency_portal_api.Configurations
{
    public static class Authentication
    {
        public static AuthenticationBuilder ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JWT jwt = configuration.GetSection("JWT").Get<JWT>();


            return services.AddAuthentication()
              .AddCookie(options =>
              {
                  options.SlidingExpiration = true;
              })
              .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
              {
                  options.RequireHttpsMetadata = false;
                  options.SaveToken = true;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt.SigningKey)),
                      ValidateAudience = true,
                      ValidateIssuer = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = jwt.Issuer,
                      ValidAudience = jwt.Audience
                  };
              });
        }
    }
}
