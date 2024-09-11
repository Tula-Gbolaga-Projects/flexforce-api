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
        private readonly IJobDetailService jobDetailService;

        public AgenciesController(IAgencyService agencyService, IJobDetailService jobDetailService)
        {
            this.agencyService = agencyService;
            this.jobDetailService = jobDetailService;
        }

        // POST api/Agency
        [HttpPost("create")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GlobalResponse<GetAgencyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAgency(CreateAgencyDto model, CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await agencyService.Create(model, token));
        }

        [HttpPost("jobs/create")]
        [ProducesResponseType(typeof(GlobalResponse<GetJobDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateJobDetail(CreateJobDetailDto model, CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await jobDetailService.Create(model, HttpContext.User.FindFirstValue("AgencyId"), token));
        }

        [HttpGet("jobs/list-all")]
        [ProducesResponseType(typeof(GlobalResponse<GetJobDetailDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListAllJobDetails(CancellationToken token)
        {
            return Ok(ResponseBuilder.BuildResponse<object>(null, await jobDetailService.GetAll(HttpContext.User.FindFirstValue("AgencyId"), token)));
        }

        [HttpGet("jobs/{jobDetailId}")]
        [ProducesResponseType(typeof(GlobalResponse<GetJobDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetJobDetailById(string jobDetailId, CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await jobDetailService.GetById(jobDetailId, token));
        }
    }
}
