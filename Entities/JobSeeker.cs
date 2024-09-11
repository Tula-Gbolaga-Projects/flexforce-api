using System.ComponentModel.DataAnnotations;

namespace agency_portal_api.Entities
{
    public class JobSeeker : DbEntity
    {
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        public string ProfilePicture { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AboutMe { get; set; }
        public string CV { get; set; }
        public string InternationalPass { get; set; }
        public string Brp { get; set; }
        public string NationalInsurance { get; set; }
        public string ShareCode { get; set; }
        public List<ConnectedAgency> ConnectedAgencies { get; set; }
    }
}
