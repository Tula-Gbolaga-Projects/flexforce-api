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
        private readonly IAppliedJobService appliedJobService;

        public AgenciesController(IAgencyService agencyService, IJobDetailService jobDetailService, IAppliedJobService appliedJobService)
        {
            this.agencyService = agencyService;
            this.jobDetailService = jobDetailService;
            this.appliedJobService = appliedJobService;
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

        [HttpGet("jobs/{jobDetailId}/applications")]
        [ProducesResponseType(typeof(GlobalResponse<GetAppliedJobDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListJobs(string jobDetailId, CancellationToken token)
        {
            return Ok(ResponseBuilder.BuildResponse<object>(null, await appliedJobService.GetAppliedJobs(jobDetailId, token)));
        }

        [HttpPatch("jobs/applications/{jobApplicationId}/approve")]
        [ProducesResponseType(typeof(GlobalResponse<GetAppliedJobDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApproveJob(string jobApplicationId, CancellationToken token)
        {
            return Ok(ResponseBuilder.BuildResponse<object>(null, await appliedJobService.UpdateApplication(jobApplicationId, AppliedJobStatusEnum.Accepted, token)));
        }

        [HttpPatch("jobs/applications/{jobApplicationId}/reject")]
        [ProducesResponseType(typeof(GlobalResponse<GetAppliedJobDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RejectJob(string jobApplicationId, CancellationToken token)
        {
            return Ok(ResponseBuilder.BuildResponse<object>(null, await appliedJobService.UpdateApplication(jobApplicationId, AppliedJobStatusEnum.Rejected, token)));
        }
    }
}
