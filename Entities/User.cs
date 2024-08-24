using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace agency_portal_api.Entities
{
    public class User : DbEntity
    {
        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        public string RoleName { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string NormalizedUserName { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string Email { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string NormalizedEmail { get; set; }

        [Column(TypeName = "bytea")]
        public byte[] PasswordHashed { get; set; }

        [Column(TypeName = "bytea")]
        public byte[] PasswordByte { get; set; }
        public string SecurityStamp { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public bool LockoutEnabled { get; set; }
        public string? RefreshTokenKey { get; set; }
        public DateTime? RefreshTokenExpirytime { get; set; }
        public DateTime? LastRefreshTime { get; set; }
    }
}
