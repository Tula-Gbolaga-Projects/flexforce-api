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
    }

    public class AgencyService : IAgencyService
    {
        private readonly IRepository repository;
        private IMapper mapper;

        public AgencyService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<CustomResponse<GetAgencyDto>> Create(CreateAgencyDto model, CancellationToken token)
        {
            if (model == null)
            {
                return new ServiceError<GetAgencyDto>().NullError();
            }

            var agency = mapper.Map<Agency>(model);

            var result = await repository.AddAsync(agency, token);
            if (result)
            {
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

    }
}
