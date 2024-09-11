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
    public class JobSeekersController : ControllerBase
    {
        private readonly IJobSeekerService jobSeekerService;
        private readonly IAppliedJobService appliedJobService;
        private readonly IJobDetailService jobDetailService;
        private readonly IAgencyService agencyService;
        private readonly IConnectedAgencyService connectedAgencyService;

        public JobSeekersController(IJobSeekerService jobSeekerService, IAppliedJobService appliedJobService, 
            IJobDetailService jobDetailService, IAgencyService agencyService, IConnectedAgencyService connectedAgencyService)
        {
            this.jobSeekerService = jobSeekerService;
            this.appliedJobService = appliedJobService;
            this.jobDetailService = jobDetailService;
            this.agencyService = agencyService;
            this.connectedAgencyService = connectedAgencyService;
        }

        // POST api/JobSeeker
        [HttpPost("create")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GlobalResponse<GetJobSeekerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateJobSeeker(CreateJobSeekerDto model, CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await jobSeekerService.Create(model, token));
        }

        // GET api/JobSeeker/{jobSeekerId}
        [HttpGet("profile")]
        [ProducesResponseType(typeof(GlobalResponse<GetJobSeekerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetJobSeekerById(CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await jobSeekerService.GetById(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), token));
        }

        [HttpPost("jobs/apply")]
        [ProducesResponseType(typeof(GlobalResponse<GetAppliedJobDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApplyToJob(CreateAppliedJobDto model, CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await appliedJobService.ApplyToJob(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), model.JobDetailId, token));
        }

        [HttpGet("jobs/list-all")]
        [ProducesResponseType(typeof(GlobalResponse<GetJobDetailDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListJobs(string city, string industry, string role, string group, CancellationToken token)
        {
            return Ok(ResponseBuilder.BuildResponse<object>(null, await jobDetailService.GetPaginatedResult(token)));
        }

        [HttpGet("agencies/list-all")]
        [ProducesResponseType(typeof(GlobalResponse<SeekerConnectedAgencyDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AgenciesList(CancellationToken token)
        {
            return Ok(ResponseBuilder.BuildResponse<object>(null, await connectedAgencyService.ListAllSeekerAgencies(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),token)));
        }

        [HttpGet("agencies/{agencyId}")]
        [ProducesResponseType(typeof(GlobalResponse<GetAgencyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAgencyById(string agencyId, CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await agencyService.GetById(agencyId, token));
        }
    }
}
