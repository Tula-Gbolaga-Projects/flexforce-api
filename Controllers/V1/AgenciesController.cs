using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using agency_portal_api.DTOs;
using agency_portal_api.Entities;
using agency_portal_api.Services;
using agency_portal_api.Utilities;
using agency_portal_api.Controllers;

namespace agency_portal_api.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class AgenciesController : ControllerBase
    {
        private readonly IAgencyService agencyService;

        public AgenciesController(IAgencyService agencyService)
        {
            this.agencyService = agencyService;
        }

        // POST api/Agency
        [HttpPost("create")]
        [ProducesResponseType(typeof(GlobalResponse<GetAgencyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAgency(CreateAgencyDto model, CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await agencyService.Create(model, token));
        }

    }
}
