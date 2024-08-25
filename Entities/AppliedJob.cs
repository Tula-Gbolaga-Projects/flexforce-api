using System.ComponentModel.DataAnnotations;

namespace agency_portal_api.Entities
{
    public class AppliedJob : DbEntity
    {
        [Required]
        public string JobSeekerId { get; set; }
        public JobSeeker JobSeeker { get; set; }

        [Required]
        public string JobDetailId { get; set; }
        public JobDetail JobDetail { get; set; }

        public AppliedJobStatusEnum Status { get; set; } = 0;
    }

    public enum AppliedJobStatusEnum
    {
        Pending, Accepted, Rejected
    }
}
