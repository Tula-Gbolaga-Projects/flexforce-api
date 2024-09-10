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
    public interface IJobSeekerService
    {
        Task<CustomResponse<GetJobSeekerDto>> Create(CreateJobSeekerDto jobSeeker, CancellationToken token);
        Task<CustomResponse<GetJobSeekerDto>> GetById(string jobSeekerId, CancellationToken token);
        Task<List<GetJobSeekerDto>> GetPaginatedResult(CancellationToken token);
        Task<CustomResponse<GetJobSeekerDto>> Profile(string jobSeekerId, CancellationToken token);
    }

    public class JobSeekerService : IJobSeekerService
    {
        private readonly IRepository repository;
        private IMapper mapper;
        private readonly IUserService userService;
        private readonly IMailJetService mailJetService;

        public JobSeekerService(IRepository repository, IMapper mapper, IUserService userService, IMailJetService mailJetService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.userService = userService;
            this.mailJetService = mailJetService;
        }

        public async Task<CustomResponse<GetJobSeekerDto>> Create(CreateJobSeekerDto model, CancellationToken token)
        {
            if (model == null)
            {
                return new ServiceError<GetJobSeekerDto>().NullError();
            }

            var jobSeeker = mapper.Map<JobSeeker>(model);

            var userResponse = await userService.CreateUser(model, Roles.JobSeeker, token);
            if (userResponse.Response != ServiceResponses.Success)
                return new CustomResponse<GetJobSeekerDto>() { Response = userResponse.Response, Message = userResponse.Message };

            jobSeeker.UserId = userResponse.Data.Id;
            jobSeeker.Id = userResponse.Data.Id;

            var result = await repository.AddAsync(jobSeeker, token);
            if (result)
            {
                await mailJetService.SendMail(userResponse.Data.Email, "You have successfully created your account", "Account Creation Successful", token, false);

                return await GetById(jobSeeker.Id, token);
            }

            return new ServiceError<GetJobSeekerDto>().CreateError();
        }

        public async Task<CustomResponse<GetJobSeekerDto>> GetById(string jobSeekerId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(jobSeekerId))
            {
                return new ServiceError<GetJobSeekerDto>().NullError();
            }

            var jobSeeker = await ListAll()
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == jobSeekerId);

            if (jobSeeker == null)
            {
                return new ServiceError<GetJobSeekerDto>().FindError();
            }

            var mapped = mapper.Map<GetJobSeekerDto>(jobSeeker);
            mapped.User = mapper.Map<GetUserDto>(jobSeeker.User);

            return new CustomResponse<GetJobSeekerDto>()
            {
                Response = ServiceResponses.Success,
                Data = mapped
            };
        }

        public async Task<List<GetJobSeekerDto>> GetPaginatedResult(CancellationToken token)
        {
            var query = await ListAll().ToListAsync(token);

            var paginatedResult = mapper.Map<List<GetJobSeekerDto>>(query);

            return paginatedResult;
        }

        public IQueryable<JobSeeker> ListAll()
        {
            return repository.ListAll<JobSeeker>();
        }


        public async Task<CustomResponse<GetJobSeekerDto>> Profile(string jobSeekerId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(jobSeekerId))
            {
                return new ServiceError<GetJobSeekerDto>().NullError();
            }

            var jobSeeker = await ListAll()
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == jobSeekerId);

            if (jobSeeker == null)
            {
                return new ServiceError<GetJobSeekerDto>().FindError();
            }

            var mapped = mapper.Map<GetJobSeekerDto>(jobSeeker);
            mapped.User = mapper.Map<GetUserDto>(jobSeeker.User);

            return new CustomResponse<GetJobSeekerDto>()
            {
                Response = ServiceResponses.Success,
                Data = mapped
            };
        }
    }
}
