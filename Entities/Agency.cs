namespace agency_portal_api.Entities
{
    public class Agency : DbEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }
        public AgencyStatusEnum Status { get; set; }
        public DateTime? DateApproved { get; set; }
    }

    public enum AgencyStatusEnum
    {
        Pending, Approved
    }
}
