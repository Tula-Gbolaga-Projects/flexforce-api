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
        private readonly IMailJetService mailJetService;
        private IMapper mapper;

        public JobDetailService(IRepository repository, IMapper mapper, IMailJetService mailJetService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.mailJetService = mailJetService;
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
                //email users under agency
                var connectedUsers = await repository.ListAll<ConnectedAgency>().Include(c => c.JobSeeker.User).Where(c => c.AgencyId ==  agencyid && c.ConnectedStatus == ConnectedAgencyStatusEnum.Onboarded).Select(c => new MailjetUserDetails()
                {
                    Email = c.JobSeeker.User.Email,
                    Name = c.JobSeeker.User.FirstName
                }).ToListAsync();

                await mailJetService.SendMail(connectedUsers.FirstOrDefault().Email, "A new job has been posted, kindly login to check it out", "New Job Posting", token, false, connectedUsers);

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
