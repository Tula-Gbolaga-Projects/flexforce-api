using agency_portal_api.Entities;
using System.ComponentModel.DataAnnotations;

namespace agency_portal_api.DTOs
{
    public class CreateAppliedJobDto
    {
        public string JobDetailId { get; set; }
    }

    public class GetAppliedJobDto
    {
        public string JobDetailId { get; set; }
        public string JobDetail { get; set; }

        public string Agency { get; set; }
        public string Location { get; set; }
        public string Industry { get; set; }
        public double PayRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } 
    }
}
