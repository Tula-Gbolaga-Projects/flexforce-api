using agency_portal_api.Entities;

namespace agency_portal_api.DTOs
{
    public class CreateAgencyDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }
        public CreateUserDto AgencyStaff { get; set; }
    }

    public class GetAgencyDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
    }
}
