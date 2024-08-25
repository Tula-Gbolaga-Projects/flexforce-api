using System.ComponentModel.DataAnnotations;

namespace agency_portal_api.Entities
{
    public class AgencyStaff : DbEntity
    {
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string AgencyId { get; set; }
        public Agency Agency { get; set; }

        public string ProfilePicture { get; set; }
    }
}
