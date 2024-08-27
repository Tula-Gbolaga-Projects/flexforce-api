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

        public JobSeekersController(IJobSeekerService jobSeekerService)
        {
            this.jobSeekerService = jobSeekerService;
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

        // GET api/JobSeeker/ListAll
        [HttpGet("list-all")]
        [ProducesResponseType(typeof(GlobalResponse<GetJobSeekerDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListAllJobSeekers(int page, int perPage, CancellationToken token)
        {
            return Ok(ResponseBuilder.BuildResponse<object>(null, await jobSeekerService.GetPaginatedResult(token)));
        }

        // GET api/JobSeeker/{jobSeekerId}
        [HttpGet("{jobSeekerId}")]
        [ProducesResponseType(typeof(GlobalResponse<GetJobSeekerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetJobSeekerById(string jobSeekerId, CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await jobSeekerService.GetById(jobSeekerId, token));
        }
    }
}
