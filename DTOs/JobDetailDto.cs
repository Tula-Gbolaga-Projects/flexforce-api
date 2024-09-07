using agency_portal_api.Entities;
using System.ComponentModel.DataAnnotations;

namespace agency_portal_api.DTOs
{
    public class CreateJobDetailDto
    {
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
        public string Location { get; set; }
        public JobPublicityEnum Publicity { get; set; }
    }

    public class GetJobDetailDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public double PayRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Industry { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public string Location { get; set; }
        public string Publicity { get; set; }
        public string DressCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
