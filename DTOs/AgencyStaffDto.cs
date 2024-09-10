using agency_portal_api.Entities;
using System.ComponentModel.DataAnnotations;

namespace agency_portal_api.DTOs
{
    public class CreateAgencyStaffDto : CreateUserDto
    {
        public string AgencyId { get; set; }
        public string ProfilePicture { get; set; }
    }

    public class GetAgencyStaffDto
    {
        public string Id { get; set; }
        public string AgencyId { get; set; }
        public string Agency { get; set; }
        public string ProfilePicture { get; set; }

        public string UserId { get; set; }
        public GetUserDto User { get; set; }

        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
