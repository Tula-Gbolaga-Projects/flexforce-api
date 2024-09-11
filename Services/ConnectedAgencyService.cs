using agency_portal_api.DTOs;
using agency_portal_api.DTOs.ServiceDtos;
using agency_portal_api.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace agency_portal_api.Services
{
    public interface IConnectedAgencyService
    {
        Task<List<SeekerConnectedAgencyDto>> ListAllSeekerAgencies(string userId, CancellationToken token);
        Task<List<AgencyConnectedSeekerDto>> ListAllAgenciesSeeker(string agencyId, CancellationToken token);
        Task<CustomResponse<string>> ConnectToAgency(string agencyId, string userId, CancellationToken token);
        Task<CustomResponse<string>> InviteSeeker(string agencyId, string userId, CancellationToken token);
        Task<CustomResponse<string>> UpdateConnectedSeeker(string userId, string agencyId, ConnectedAgencyStatusEnum connectedAgencyStatus, CancellationToken token);
    }

    public class ConnectedAgencyService : IConnectedAgencyService
    {
        private readonly IAgencyService agencyService;
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly IJobSeekerService jobSeekerService;
        private readonly IMailJetService mailJetService;    

        public ConnectedAgencyService(IAgencyService agencyService, IRepository repository, IMapper mapper, IJobSeekerService jobSeekerService, IMailJetService mailJetService)
        {
            this.agencyService = agencyService;
            this.repository = repository;
            this.mapper = mapper;
            this.jobSeekerService = jobSeekerService;
            this.mailJetService = mailJetService;
        }

        public async Task<List<SeekerConnectedAgencyDto>> ListAllSeekerAgencies(string userId, CancellationToken token)
        {
            var agencies = await agencyService.ListAll().Include(c => c.Staff.Where(d => d.IsPrimary)).ThenInclude(c => c.User).Include(c => c.ConnectedSeekers).Where(c => c.Status == AgencyStatusEnum.Approved).ToListAsync(token);

            var mapped = mapper.Map<List<SeekerConnectedAgencyDto>>(agencies);

            return mapped;
        }

        public IQueryable<ConnectedAgency> ListAll()
        {
            return repository.ListAll<ConnectedAgency>();
        }

        public async Task<List<AgencyConnectedSeekerDto>> ListAllAgenciesSeeker(string agencyId, CancellationToken token)
        {
            var agencies = await jobSeekerService.ListAll().Include(c => c.User).Include(c => c.ConnectedAgencies).ToListAsync(token);

            var mapped = mapper.Map<List<AgencyConnectedSeekerDto>>(agencies);

            return mapped;
        }

        public async Task<CustomResponse<string>> ConnectToAgency(string agencyId, string userId, CancellationToken token)
        {
            var existingConnection = await ListAll().AnyAsync(c => c.AgencyId == agencyId && c.JobSeekerId == userId, token);
            if (existingConnection)
            {
                return new CustomResponse<string>()
                {
                    Message = "An existing connection already requested",
                    Response = DTOs.Enums.ServiceResponses.Failed
                };
            }

            var newConnection = new ConnectedAgency()
            {
                AgencyId = agencyId,
                JobSeekerId = userId,
                ConnectedStatus = ConnectedAgencyStatusEnum.RequestSent
            };

            var createdResponse =  await repository.AddAsync(newConnection);

            if (createdResponse)
            {
                return new CustomResponse<string>()
                {
                    Data = "Request sent successfully",
                    Response = DTOs.Enums.ServiceResponses.Success
                };
            }

            return new CustomResponse<string>()
            {
                Message = "Unable to connect",
                Response = DTOs.Enums.ServiceResponses.Failed
            };
        }

        public async Task<CustomResponse<string>> InviteSeeker(string agencyId, string userId, CancellationToken token)
        {
            var existingConnection = await ListAll().AnyAsync(c => c.AgencyId == agencyId && c.JobSeekerId == userId, token);
            if (existingConnection)
            {
                return new CustomResponse<string>()
                {
                    Message = "Job seeker already invited",
                    Response = DTOs.Enums.ServiceResponses.Failed
                };
            }

            var newConnection = new ConnectedAgency()
            {
                AgencyId = agencyId,
                JobSeekerId = userId,
                ConnectedStatus = ConnectedAgencyStatusEnum.Invited
            };

            var createdResponse = await repository.AddAsync(newConnection);

            if (createdResponse)
            {
                var existingUser = await repository.ListAll<User>().FirstOrDefaultAsync(c => c.Id == userId);
                if (existingUser != null)
                    await mailJetService.SendMail(existingUser.Email, "You have been invited to be connected with an angency, kindly login to view details", "Invitation Requested", token, false);

                return new CustomResponse<string>()
                {
                    Data = "Request sent successfully",
                    Response = DTOs.Enums.ServiceResponses.Success
                };
            }

            return new CustomResponse<string>()
            {
                Message = "Unable to connect",
                Response = DTOs.Enums.ServiceResponses.Failed
            };
        }

        public async Task<CustomResponse<string>> UpdateConnectedSeeker(string userId, string agencyId, ConnectedAgencyStatusEnum connectedAgencyStatus, CancellationToken token)
        {
            var existingConnection = await ListAll().FirstOrDefaultAsync(c => c.AgencyId == agencyId && c.JobSeekerId == userId, token);
            if (existingConnection  == null)
            {
                return new CustomResponse<string>()
                {
                    Message = "Connection not found",
                    Response = DTOs.Enums.ServiceResponses.Failed
                };
            }

            existingConnection.ConnectedStatus = connectedAgencyStatus;
            existingConnection.DateModified = DateTime.UtcNow;
            var createdResponse = await repository.ModifyAsync(existingConnection);

            if (createdResponse)
            {
                if (connectedAgencyStatus == ConnectedAgencyStatusEnum.Onboarded)
                {
                    var existingUser = await repository.ListAll<User>().FirstOrDefaultAsync(c => c.Id == userId);
                    if (existingUser != null)
                        await mailJetService.SendMail(existingUser.Email, "You have been approved to join an angency, kindly login to view details", "Onboard Successful", token, false);
                }

                return new CustomResponse<string>()
                {
                    Data = "Request sent successfully",
                    Response = DTOs.Enums.ServiceResponses.Success
                };
            }

            return new CustomResponse<string>()
            {
                Message = "Unable to update",
                Response = DTOs.Enums.ServiceResponses.Failed
            };
        }
    }
}
