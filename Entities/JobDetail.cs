using System.ComponentModel.DataAnnotations;

namespace agency_portal_api.Entities
{
    public class JobDetail : DbEntity
    {
        [Required]
        public string AgencyId { get; set; }
        public Agency Agency { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Name { get; set; }
        public double PayRate { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Industry { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public string DressCode { get; set; }
    }
}
