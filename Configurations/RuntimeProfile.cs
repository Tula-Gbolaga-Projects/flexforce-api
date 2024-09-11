using AutoMapper;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using agency_portal_api.DTOs;
using agency_portal_api.Entities;
using AutoMapper.Execution;

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
            CreateMap<JobSeeker, GetJobSeekerProfileDto>()
                .ForMember(dest => dest.User, option => option
                .MapFrom(src => new GetUserDto() 
                { 
                    Id = src.UserId,
                    DateCreated = src.User.DateCreated,
                    FirstName = src.User.FirstName,
                    LastName = src.User.LastName,
                    Email = src.User.Email, 
                    PhoneNumber = src.User.PhoneNumber,
                    IsActive = src.User.IsActive
                }));
            #endregion

            #region Agency Staff mappings
            CreateMap<CreateAgencyStaffDto, User>();
            CreateMap<CreateAgencyStaffDto, AgencyStaff>();
            CreateMap<AgencyStaff, GetAgencyStaffDto>()
                .ForMember(dest => dest.User, option => option
                .MapFrom(src => new GetUserDto()
                {
                    Id = src.UserId,
                    DateCreated = src.User.DateCreated,
                    FirstName = src.User.FirstName,
                    LastName = src.User.LastName,
                    Email = src.User.Email,
                    PhoneNumber = src.User.PhoneNumber,
                    IsActive = src.User.IsActive
                }));
            #endregion

            #region Agency mappings
            CreateMap<CreateAgencyDto, Agency>();
            CreateMap<Agency, GetAgencyDto>();
            #endregion

            #region Job Detail mappings
            CreateMap<CreateJobDetailDto, JobDetail>();
            CreateMap<JobDetail, GetJobDetailDto>();
            #endregion

            #region Applied Job mappings
            CreateMap<AppliedJob, GetAppliedJobDto>()
                .ForMember(dest => dest.JobDetail, option => option
                .MapFrom(src => src.JobDetail.Name))
                .ForMember(dest => dest.StartDate, option => option
                .MapFrom(src => src.JobDetail.StartDate))
                .ForMember(dest => dest.EndDate, option => option
                .MapFrom(src => src.JobDetail.EndDate))
                .ForMember(dest => dest.Status, option => option
                .MapFrom(src => src.Status))
                .ForMember(dest => dest.Agency, option => option
                .MapFrom(src => src.JobDetail.Agency.Name))
                .ForMember(dest => dest.PayRate, option => option
                .MapFrom(src => src.JobDetail.PayRate))
                .ForMember(dest => dest.Industry, option => option
                .MapFrom(src => src.JobDetail.Industry))
                .ForMember(dest => dest.Location, option => option
                .MapFrom(src => src.JobDetail.Location));
            #endregion

            #region Seeker Connected Agency mappings
            CreateMap<Agency, SeekerConnectedAgencyDto>()
                .ForMember(dest => dest.AgencyName, option => option
                .MapFrom(src => src.Name))
                .ForMember(dest => dest.Agencyid, option => option
                .MapFrom(src => src.Id))
                .ForMember(dest => dest.Brief, option => option
                .MapFrom(src => src.Description))
                .ForMember(dest => dest.Email, option => option
                .MapFrom(src => src.Email))
                .ForMember(dest => dest.Address, option => option
                .MapFrom(src => src.Address))
                .ForMember(dest => dest.Status, option => option
                .MapFrom<GetConnectedAgencyStatusResolver>())
                .ForMember(dest => dest.PrimaryStaff, option => option
                .MapFrom(src => src.Staff == null ? null : $"{src.Staff.FirstOrDefault().User.FirstName} {src.Staff.FirstOrDefault().User.LastName}"))
                .ForMember(dest => dest.PrimaryStaffEmail, option => option
                .MapFrom(src => src.Staff == null ? null : $"{src.Staff.FirstOrDefault().User.Email}"));
            #endregion
        }
    }

    public class GetConnectedAgencyStatusResolver : IValueResolver<Agency, SeekerConnectedAgencyDto, string>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetConnectedAgencyStatusResolver(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(Agency source, SeekerConnectedAgencyDto destination, string destMember, ResolutionContext context)
        {
            var connectedAgency = source.ConnectedSeekers.Where(c => c.JobSeekerId == httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
            if(connectedAgency == null )
            {
                return ConnectedAgencyStatusEnum.NotOnboarded.ToString();
            }

            return connectedAgency.ConnectedStatus.ToString();
        }
    }
}
