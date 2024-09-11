using agency_portal_api.DTOs;
using agency_portal_api.Services;
using agency_portal_api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace agency_portal_api.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class AdministratorsController : ControllerBase
    {
        private readonly IAgencyService agencyService;
        private readonly IJobSeekerService jobSeekerService;
        private readonly IUserService userService;

        public AdministratorsController(IAgencyService agencyService, IJobSeekerService jobSeekerService, IUserService userService)
        {
            this.agencyService = agencyService;
            this.jobSeekerService = jobSeekerService;
            this.userService = userService;
        }

        [HttpPost("users/create")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GlobalResponse<GetUserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApplyToJob(CreateUserDto model, CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await userService.CreateUser(model, null, token));
        }

        [HttpGet("agencies/list-all")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GlobalResponse<GetAgencyDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListAllAgencies(CancellationToken token)
        {
            return Ok(ResponseBuilder.BuildResponse<object>(null, await agencyService.GetPaginatedResult(token)));
        }

        [HttpGet("agencies/{agencyId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GlobalResponse<GetAgencyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAgencyById(string agencyId, CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await agencyService.GetById(agencyId, token));
        }

        [HttpPost("agencies/{agencyId}/activate")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GlobalResponse<GetAgencyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ActivateAgency([Required]string agencyId, CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await agencyService.ActivateAgency(agencyId, token));
        }

        // GET api/JobSeeker/ListAll
        [HttpGet("job-seeker/list-all")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GlobalResponse<GetJobSeekerDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListAllJobSeekers(int page, int perPage, CancellationToken token)
        {
            return Ok(ResponseBuilder.BuildResponse<object>(null, await jobSeekerService.GetPaginatedResult(token)));
        }

        // GET api/JobSeeker/{jobSeekerId}
        [HttpGet("job-seeker/{jobSeekerId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GlobalResponse<GetJobSeekerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetJobSeekerById(string jobSeekerId, CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await jobSeekerService.GetById(jobSeekerId, token));
        }
    }
}
