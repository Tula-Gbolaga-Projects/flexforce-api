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
    public class JobDetailsController : ControllerBase
    {
        private readonly IJobDetailService jobDetailService;

        public JobDetailsController(IJobDetailService jobDetailService)
        {
            this.jobDetailService = jobDetailService;
        }

        // POST api/JobDetail
        [HttpPost("create")]
        [ProducesResponseType(typeof(GlobalResponse<GetJobDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateJobDetail(CreateJobDetailDto model, CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await jobDetailService.Create(model, token));
        }

        // GET api/JobDetail/ListAll
        [HttpGet("list-all")]
        [ProducesResponseType(typeof(GlobalResponse<GetJobDetailDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListAllJobDetails(int page, int perPage, CancellationToken token)
        {
            return Ok(ResponseBuilder.BuildResponse<object>(null, await jobDetailService.GetPaginatedResult(token)));
        }

        // GET api/JobDetail/{jobDetailId}
        [HttpGet("{jobDetailId}")]
        [ProducesResponseType(typeof(GlobalResponse<GetJobDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GlobalResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetJobDetailById(string jobDetailId, CancellationToken token)
        {
            return new ControllerResponse().ReturnResponse(await jobDetailService.GetById(jobDetailId, token));
        }
    }
}
