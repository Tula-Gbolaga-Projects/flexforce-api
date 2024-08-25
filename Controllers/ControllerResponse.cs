using agency_portal_api.DTOs.Enums;
using agency_portal_api.DTOs.ServiceDtos;
using agency_portal_api.Utilities;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace agency_portal_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerResponse : ControllerBase
    {
        private readonly IMapper mapper;

        public ControllerResponse()
        {
        }

        public ControllerResponse(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public IActionResult ReturnResponse<T>(CustomResponse<T> customResponse)
        {
            switch (customResponse.Response)
            {
                case ServiceResponses.BadRequest:
                    ModelState.AddModelError($"{customResponse.Response}", customResponse.Message);
                    return BadRequest(ResponseBuilder.BuildResponse<object>(ModelState, null));

                case ServiceResponses.NotFound:
                    ModelState.AddModelError($"{customResponse.Response}", customResponse.Message);
                    return NotFound(ResponseBuilder.BuildResponse<object>(ModelState, null));

                case ServiceResponses.Failed:
                    ModelState.AddModelError($"{customResponse.Response}", customResponse.Message);
                    return UnprocessableEntity(ResponseBuilder.BuildResponse<object>(ModelState, null));

                case ServiceResponses.Success:
                    return Ok(ResponseBuilder.BuildResponse<object>(null, customResponse.Data == null ? customResponse.Response : customResponse.Data));

                default:
                    ModelState.AddModelError($"{customResponse.Response}", customResponse.Message);
                    return UnprocessableEntity(ResponseBuilder.BuildResponse<object>(ModelState, null));
            }
        }
    }
}
