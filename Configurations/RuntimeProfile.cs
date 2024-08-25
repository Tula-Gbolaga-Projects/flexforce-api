using AutoMapper;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using agency_portal_api.DTOs;
using agency_portal_api.Entities;

namespace agency_portal_api.Configurations
{
    public  class RuntimeProfile : Profile
    {
        public RuntimeProfile() 
        {
            #region User mappings
            CreateMap<CreateUserDto, User>();
            CreateMap<User, GetUserDto>();
            #endregion

            #region Job Seeker mappings
            CreateMap<CreateJobSeekerDto, User>();
            CreateMap<CreateJobSeekerDto, JobSeeker>();
            CreateMap<JobSeeker, GetJobSeekerDto>();
            #endregion

            #region Agency Staff mappings
            CreateMap<CreateAgencyStaffDto, User>();
            CreateMap<CreateAgencyStaffDto, AgencyStaff>();
            CreateMap<AgencyStaff, GetAgencyStaffDto>();
            #endregion

            #region Agency mappings
            CreateMap<CreateAgencyDto, Agency>();
            CreateMap<Agency, GetAgencyDto>();
            #endregion

            #region Job Detail mappings
            CreateMap<CreateJobDetailDto, JobDetail>();
            CreateMap<JobDetail, GetJobDetailDto>();
            #endregion
        }
    }
}
