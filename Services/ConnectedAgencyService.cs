using agency_portal_api.DTOs;
using agency_portal_api.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace agency_portal_api.Services
{
    public interface IConnectedAgencyService
    {
        Task<List<SeekerConnectedAgencyDto>> ListAllSeekerAgencies(string userId, CancellationToken token);
    }

    public class ConnectedAgencyService : IConnectedAgencyService
    {
        private readonly IAgencyService agencyService;
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public ConnectedAgencyService(IAgencyService agencyService, IRepository repository, IMapper mapper)
        {
            this.agencyService = agencyService;
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<List<SeekerConnectedAgencyDto>> ListAllSeekerAgencies(string userId, CancellationToken token)
        {
            var agencies = await agencyService.ListAll().Include(c => c.Staff.Where(d => d.IsPrimary)).ThenInclude(c => c.User).Include(c => c.ConnectedSeekers).Where(c => c.Status == AgencyStatusEnum.Approved).ToListAsync(token);

            var mapped = mapper.Map<List<SeekerConnectedAgencyDto>>(agencies);

            return mapped;
        }
    }
}
