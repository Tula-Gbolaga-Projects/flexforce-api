namespace agency_portal_api.DTOs
{
    public class CreateJobSeekerDto : CreateUserDto
    {
        public string ProfilePicture { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AboutMe { get; set; }
        public string CV { get; set; }
        public string InternationalPass { get; set; }
        public string Brp { get; set; }
        public string NationalInsurance { get; set; }
        public string ShareCode { get; set; }
        public string RoleName { get; set; }
    }

    public class GetJobSeekerDto
    {
        public string Id { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AboutMe { get; set; }
        public string CV { get; set; }
        public string InternationalPass { get; set; }
        public string Brp { get; set; }
        public string NationalInsurance { get; set; }
        public string ShareCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; }
        public GetUserDto User { get; set; }
    }
}
