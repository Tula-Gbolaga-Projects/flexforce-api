using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AutoMapper;
using agency_portal_api.DTOs;
using agency_portal_api.DTOs.ServiceDtos;
using agency_portal_api.Services;
using agency_portal_api.Entities;
using agency_portal_api.DTOs.Enums;
using agency_portal_api.DTOs.Consts;

namespace agency_portal_api.Services
{
    public interface IAgencyStaffService
    {
        Task<CustomResponse<GetAgencyStaffDto>> Create(CreateAgencyStaffDto agencyStaff, bool primaryStaff, CancellationToken token);
        Task<CustomResponse<GetAgencyStaffDto>> GetById(string agencyStaffId, CancellationToken token);
        Task<List<GetAgencyStaffDto>> GetPaginatedResult(CancellationToken token);
    }

    public class AgencyStaffService : IAgencyStaffService
    {
        private readonly IRepository repository;
        private IMapper mapper;
        private readonly IUserService userService;

        public AgencyStaffService(IRepository repository, IMapper mapper, IUserService userService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.userService = userService;
        }

        public async Task<CustomResponse<GetAgencyStaffDto>> Create(CreateAgencyStaffDto model, bool primaryStaff, CancellationToken token)
        {
            if (model == null)
            {
                return new ServiceError<GetAgencyStaffDto>().NullError();
            }

            var agencyStaff = mapper.Map<AgencyStaff>(model);
            agencyStaff.IsPrimary = primaryStaff;

            var userResponse = await userService.CreateUser(model, Roles.AgencyStaff, token);
            if(userResponse.Response != ServiceResponses.Success)
                return new CustomResponse<GetAgencyStaffDto>() { Response = userResponse.Response , Message = userResponse.Message};

            agencyStaff.UserId = userResponse.Data.Id;
            agencyStaff.Id = userResponse.Data.Id;

            var result = await repository.AddAsync(agencyStaff, token);
            if (result)
            {
                return await GetById(agencyStaff.Id, token);
            }

            return new ServiceError<GetAgencyStaffDto>().CreateError();
        }

        public async Task<CustomResponse<GetAgencyStaffDto>> GetById(string agencyStaffId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(agencyStaffId))
            {
                return new ServiceError<GetAgencyStaffDto>().NullError();
            }

            var agencyStaff = await ListAll()
                .FirstOrDefaultAsync(c => c.Id == agencyStaffId);

            if (agencyStaff == null)
            {
                return new ServiceError<GetAgencyStaffDto>().FindError();
            }

            var mapped = mapper.Map<GetAgencyStaffDto>(agencyStaff);
            mapped.User = mapper.Map<GetUserDto>(agencyStaff.User);

            return new CustomResponse<GetAgencyStaffDto>()
            {
                Response = ServiceResponses.Success,
                Data = mapped
            };
        }

        public async Task<List<GetAgencyStaffDto>> GetPaginatedResult(CancellationToken token)
        {
            var query = await ListAll().ToListAsync(token);

            var paginatedResult = mapper.Map<List<GetAgencyStaffDto>>(query);

            return paginatedResult;
        }

        public IQueryable<AgencyStaff> ListAll()
        {
            return repository.ListAll<AgencyStaff>();
        }

    }
}
