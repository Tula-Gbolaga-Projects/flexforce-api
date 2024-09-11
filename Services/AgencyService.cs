using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AutoMapper;
using agency_portal_api.DTOs;
using agency_portal_api.DTOs.ServiceDtos;
using agency_portal_api.Services;
using agency_portal_api.Entities;
using agency_portal_api.DTOs.Enums;

namespace agency_portal_api.Services
{
    public interface IAgencyService
    {
        Task<CustomResponse<GetAgencyDto>> Create(CreateAgencyDto agency, CancellationToken token);
        Task<CustomResponse<GetAgencyDto>> GetById(string agencyId, CancellationToken token);
        Task<List<GetAgencyDto>> GetPaginatedResult(CancellationToken token);
        Task<CustomResponse<GetAgencyDto>> ActivateAgency(string agencyId, CancellationToken token);
        IQueryable<Agency> ListAll();
    }

    public class AgencyService : IAgencyService
    {
        private readonly IRepository repository;
        private readonly IAgencyStaffService agencyStaffService;
        private IMapper mapper;

        public AgencyService(IRepository repository, IMapper mapper, IAgencyStaffService agencyStaffService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.agencyStaffService = agencyStaffService;
        }

        public async Task<CustomResponse<GetAgencyDto>> Create(CreateAgencyDto model, CancellationToken token)
        {
            if (model == null)
            {
                return new ServiceError<GetAgencyDto>().NullError();
            }

            var agency = mapper.Map<Agency>(model);
            agency.Status = AgencyStatusEnum.Pending;

            var result = await repository.AddAsync(agency, token);
            if (result)
            {
                var agencyStaffDto = new CreateAgencyStaffDto()
                {
                    AgencyId = agency.Id,
                    Email = model.AgencyStaff.Email,
                    FirstName = model.AgencyStaff.FirstName,
                    LastName = model.AgencyStaff.LastName,
                    Password = model.AgencyStaff.Password,  
                    PhoneNumber = model.AgencyStaff.PhoneNumber
                };

                var createdStaffResponse = await agencyStaffService.Create(agencyStaffDto, token);
                if(createdStaffResponse.Response != ServiceResponses.Success)
                {
                    await Delete(agency.Id, token);

                    return new CustomResponse<GetAgencyDto>()
                    {
                        Response = createdStaffResponse.Response,
                        Message = createdStaffResponse.Message
                    };
                }

                return await GetById(agency.Id, token);
            }

            return new ServiceError<GetAgencyDto>().CreateError();
        }

        public async Task<CustomResponse<GetAgencyDto>> GetById(string agencyId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(agencyId))
            {
                return new ServiceError<GetAgencyDto>().NullError();
            }

            var agency = await ListAll()
                .FirstOrDefaultAsync(c => c.Id == agencyId);

            if (agency == null)
            {
                return new ServiceError<GetAgencyDto>().FindError();
            }

            return new CustomResponse<GetAgencyDto>()
            {
                Response = ServiceResponses.Success,
                Data = mapper.Map<GetAgencyDto>(agency)
            };
        }

        public async Task<List<GetAgencyDto>> GetPaginatedResult(CancellationToken token)
        {
            var query = await ListAll().ToListAsync(token);

            var paginatedResult = mapper.Map<List<GetAgencyDto>>(query);

            return paginatedResult;
        }

        public IQueryable<Agency> ListAll()
        {
            return repository.ListAll<Agency>();
        }

        private async Task<bool> Delete(string agencyId, CancellationToken token)
        {
            var agency = await ListAll().FirstOrDefaultAsync(c => c.Id == agencyId);
            if (agency == null)
                return false;

            return await repository.DeleteAsync(agency);
        }

        public async Task<CustomResponse<GetAgencyDto>> ActivateAgency(string agencyId, CancellationToken token)
        {
            var agency = await ListAll().FirstOrDefaultAsync(c => c.Id == agencyId);

            if (agency == null)
            {
                return new ServiceError<GetAgencyDto>().FindError();
            }

            agency.Status = AgencyStatusEnum.Approved;
            agency.DateModified = DateTime.UtcNow;
            agency.DateApproved = DateTime.UtcNow;

            var updateResponse = await repository.ModifyAsync(agency);
            if (updateResponse)
            {
                return await GetById(agencyId, token);
            }

            return new ServiceError<GetAgencyDto>().UpdateError();
        }
    }
}
