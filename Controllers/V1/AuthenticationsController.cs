using agency_portal_api.DTOs;
using agency_portal_api.DTOs.Enums;
using agency_portal_api.Services;
using agency_portal_api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace agency_portal_api.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationsController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GlobalResponse<AuthResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            return new ControllerResponse().ReturnResponse(await authenticationService.Login(model));
        }

    }
}
