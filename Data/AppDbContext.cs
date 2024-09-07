using agency_portal_api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace agency_portal_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Agency> Agencies { get; set; }
        public DbSet<AgencyStaff> AgencyStaff { get; set; }
        public DbSet<AppliedJob> AppliedJobs { get; set; }
        public DbSet<JobDetail> JobDetails { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ConnectedAgency> ConnectedAgencies { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
