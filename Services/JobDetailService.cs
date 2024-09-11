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
    public interface IJobDetailService
    {
        Task<CustomResponse<GetJobDetailDto>> Create(CreateJobDetailDto jobDetail, string agencyId, CancellationToken token);
        Task<CustomResponse<GetJobDetailDto>> GetById(string jobDetailId, CancellationToken token);
        Task<List<GetJobDetailDto>> GetAll(string agencyId, CancellationToken token);
        Task<List<GetJobDetailDto>> GetPaginatedResult(CancellationToken token);
    }

    public class JobDetailService : IJobDetailService
    {
        private readonly IRepository repository;
        private IMapper mapper;

        public JobDetailService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<CustomResponse<GetJobDetailDto>> Create(CreateJobDetailDto model, string agencyid, CancellationToken token)
        {
            if (model == null)
            {
                return new ServiceError<GetJobDetailDto>().NullError();
            }

            var jobDetail = mapper.Map<JobDetail>(model);
            jobDetail.AgencyId = agencyid;

            var result = await repository.AddAsync(jobDetail, token);
            if (result)
            {
                return await GetById(jobDetail.Id, token);
            }

            return new ServiceError<GetJobDetailDto>().CreateError();
        }

        public async Task<CustomResponse<GetJobDetailDto>> GetById(string jobDetailId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(jobDetailId))
            {
                return new ServiceError<GetJobDetailDto>().NullError();
            }

            var jobDetail = await ListAll()
                .FirstOrDefaultAsync(c => c.Id == jobDetailId);

            if (jobDetail == null)
            {
                return new ServiceError<GetJobDetailDto>().FindError();
            }

            return new CustomResponse<GetJobDetailDto>()
            {
                Response = ServiceResponses.Success,
                Data = mapper.Map<GetJobDetailDto>(jobDetail)
            };
        }

        public async Task<List<GetJobDetailDto>> GetPaginatedResult(CancellationToken token)
        {
            var query = await ListAll().Where(c => c.Publicity == JobPublicityEnum.Public).ToListAsync(token);

            var paginatedResult = mapper.Map<List<GetJobDetailDto>>(query);

            return paginatedResult;
        }

        public async Task<List<GetJobDetailDto>> GetAll(string agencyId, CancellationToken token)
        {
            var query = await ListAll().Where(c => c.AgencyId == agencyId).ToListAsync(token);

            var paginatedResult = mapper.Map<List<GetJobDetailDto>>(query);

            return paginatedResult;
        }

        public IQueryable<JobDetail> ListAll()
        {
            return repository.ListAll<JobDetail>();
        }

    }
}
