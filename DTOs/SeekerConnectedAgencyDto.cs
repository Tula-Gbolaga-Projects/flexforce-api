namespace agency_portal_api.DTOs
{
    public class SeekerConnectedAgencyDto
    {
        public string AgencyName { get; set; }
        public string Agencyid { get; set; }
        public string Brief { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PrimaryStaff { get; set; }
        public string PrimaryStaffEmail { get; set; }
        public string Status { get; set; }
    }

    public class AgencyConnectedSeekerDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public int TotalHoursWorked { get; set; }
    }
}
