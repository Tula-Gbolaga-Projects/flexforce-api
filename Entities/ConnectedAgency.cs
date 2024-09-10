using System.ComponentModel.DataAnnotations;

namespace agency_portal_api.Entities
{
    public class ConnectedAgency : DbEntity
    {
        [Required]
        public string JobSeekerId { get; set; }
        public JobSeeker JobSeeker { get; set; }

        [Required]
        public string AgencyId { get; set; }
        public Agency Agency { get; set; }

        public ConnectedAgencyStatusEnum ConnectedStatus { get; set; }
    }

    public enum ConnectedAgencyStatusEnum
    {
        NotOnboarded, RequestSent, Onboarded, Invited
    }
}
