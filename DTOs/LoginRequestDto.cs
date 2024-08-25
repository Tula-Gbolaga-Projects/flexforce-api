using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace agency_portal_api.DTOs
{
    public class LoginRequestModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public UserTypeEnum UserType { get; set; }
    }

    public enum UserTypeEnum
    {
        Agency, JobSeeker, Admin
    }
}
