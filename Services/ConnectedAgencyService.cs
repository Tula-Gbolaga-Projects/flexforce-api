using agency_portal_api.DTOs;

namespace agency_portal_api.Services
{
    public interface IConnectedAgencyService
    {
        Task<List<SeekerConnectedAgencyDto>> ListAllSeekerAgencies(CancellationToken token);
    }

    public class ConnectedAgencyService : IConnectedAgencyService
    {
        private readonly IAgencyService agencyService;
        private readonly IRepository repository;

        public ConnectedAgencyService(IAgencyService agencyService, IRepository repository)
        {
            this.agencyService = agencyService;
            this.repository = repository;
        }

        public Task<List<SeekerConnectedAgencyDto>> ListAllSeekerAgencies(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
