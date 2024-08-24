using System.ComponentModel.DataAnnotations;

namespace agency_portal_api.Entities
{
    public class DbEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
    }
}
