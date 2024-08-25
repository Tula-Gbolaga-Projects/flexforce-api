using agency_portal_api.DTOs.Enums;

namespace agency_portal_api.DTOs.ServiceDtos
{
    public class CustomResponse<T>
    {
        public ServiceResponses Response { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        public CustomResponse()
        {
        }

        public CustomResponse(ServiceResponses response, T data, string message)
        {
            Response = response;
            Data = data;
            Message = message;
        }

        public CustomResponse(ServiceResponses response, string message)
        {
            Response = response;
            Message = message;
        }
    }
    public class ServiceError<T>
    {
        public CustomResponse<T> AuthError()
        {
            return new CustomResponse<T>()
            {
                Message = "You cannot perform this action",
                Response = ServiceResponses.Unauthorized
            };
        }
        public CustomResponse<T> NullError()
        {
            return new CustomResponse<T>()
            {
                Message = "Value cannot be null",
                Response = ServiceResponses.BadRequest
            };
        }
        public CustomResponse<T> UpdateError()
        {
            return new CustomResponse<T>()
            {
                Message = "Unable to update",
                Response = ServiceResponses.Failed
            };
        }

        public CustomResponse<T> DeleteError()
        {
            return new CustomResponse<T>()
            {
                Message = "Unable to delete",
                Response = ServiceResponses.Failed
            };
        }

        public CustomResponse<T> DeactivateError()
        {
            return new CustomResponse<T>()
            {
                Message = "Unable to deactivate",
                Response = ServiceResponses.Failed
            };
        }

        public CustomResponse<T> FindError()
        {
            return new CustomResponse<T>()
            {
                Message = "Not found",
                Response = ServiceResponses.NotFound
            };
        }

        public CustomResponse<T> CreateError()
        {
            return new CustomResponse<T>()
            {
                Message = "Unable to create",
                Response = ServiceResponses.Failed
            };
        }
    }

}
