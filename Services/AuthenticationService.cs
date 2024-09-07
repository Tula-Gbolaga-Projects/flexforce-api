using agency_portal_api.DTOs.ServiceDtos;
using agency_portal_api.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using agency_portal_api.Entities;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using agency_portal_api.Options;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using agency_portal_api.DTOs.Enums;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http.Extensions;
using System.Web;
using agency_portal_api.DTOs.Consts;

namespace agency_portal_api.Services
{
    public interface IAuthenticationService
    {
        ValueTask<CustomResponse<AuthResponse>> Login(LoginRequestModel request);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper mapper;
        private readonly PasswordValidator passwordValidator;
        private readonly IUserService userService;

        private readonly JWT _jwt;

        public IRepository repository { get; set; }

        public AuthenticationService(IMapper mapper, IOptions<JWT> options, IRepository repository,
           PasswordValidator passwordValidator, IUserService userService,  IConfiguration configuration)
        {
            this.mapper = mapper;
            _jwt = options.Value;
            this.repository = repository;
            this.passwordValidator = passwordValidator;
            this.userService = userService;
        }

        public async ValueTask<CustomResponse<AuthResponse>> Login(LoginRequestModel request)
        {
            var existingUser = await userService.FindByEmail(request.UserName);
            if (existingUser is null)
            {
                return new CustomResponse<AuthResponse>(ServiceResponses.NotFound, null, "User account not found");
            }

            if(existingUser.RoleName == Roles.AgencyStaff)
            {
                var existingStaffAgency = await repository.ListAll<AgencyStaff>().Include(c => c.Agency).FirstOrDefaultAsync(c => c.UserId == existingUser.Id);
                if(existingStaffAgency is null)
                {
                    return new CustomResponse<AuthResponse>(ServiceResponses.NotFound, null, "Agency staff not found");
                }

                if(existingStaffAgency.Agency.Status == AgencyStatusEnum.Pending)
                {
                    return new CustomResponse<AuthResponse>(ServiceResponses.Failed, null, "Your agency has not been activated");
                }
            }
            
            var passwordCheck = await userService.CheckPassword(existingUser, request.Password);
            if (passwordCheck.Response == ServiceResponses.Success)
            {
                var result = await GenerateToken(existingUser);

                return new CustomResponse<AuthResponse>(ServiceResponses.Success, result, null);
            }

            return new CustomResponse<AuthResponse>(ServiceResponses.Failed, null, "Login failed for user");
        }

        private async Task<AuthResponse> GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.RoleName)
            };

            var token = CreateToken(claims);

            var createdToken = tokenHandler.CreateToken(token);

            var refreshToken = GenerateRefreshToken();

            _ = int.TryParse(_jwt.RefreshTokenValidityInDays, out int refreshTokenValidityInDays);

            user.RefreshTokenKey = refreshToken;
            
            user.RefreshTokenExpirytime = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);

            user.LastRefreshTime = DateTime.UtcNow;

            await userService.UpdateUser(user, default);

            return new AuthResponse()
            {
                ExpiresAt = createdToken.ValidTo,
                UserId = user.Id,
                Token = tokenHandler.WriteToken(createdToken),
                RefreshToken = refreshToken,
                RoleName = user.RoleName
            };
        }

        private async Task<AuthResponse> GenerateHQToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.RoleName)
            };

            var token = CreateToken(claims);

            var createdToken = tokenHandler.CreateToken(token);

            var refreshToken = GenerateRefreshToken();

            _ = int.TryParse(_jwt.RefreshTokenValidityInDays, out int refreshTokenValidityInDays);
            user.RefreshTokenKey = refreshToken;
            user.RefreshTokenExpirytime = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);

            return new AuthResponse()
            {
                ExpiresAt = createdToken.ValidTo,
                UserId = user.Id,
                Token = tokenHandler.WriteToken(createdToken),
                RefreshToken = refreshToken
            };
        }

        private SecurityTokenDescriptor CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey));
            _ = int.TryParse(_jwt.TokenValidityInMinutes, out int tokenValidityInMinutes);

            var jwtToken = new SecurityTokenDescriptor()
            {
                Audience = _jwt.Audience,
                Issuer = _jwt.Issuer,
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };

            return jwtToken;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken)
            {
                return null;
            }
            else if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;

        }

    }
}
