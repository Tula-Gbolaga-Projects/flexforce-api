﻿using agency_portal_api.DTOs;
using agency_portal_api.DTOs.ServiceDtos;
using agency_portal_api.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace agency_portal_api.Services
{
    public interface IAppliedJobService
    {
        Task<List<GetAppliedJobDto>> GetAppliedJobs(string jobSeekerId, CancellationToken cancellationToken);
        Task<CustomResponse<GetAppliedJobDto>> ApplyToJob(string jobSeekerId, string jobDetailId, CancellationToken cancellationToken);
    }

    public class AppliedJobService : IAppliedJobService
    {
        public readonly IRepository repository;
        public readonly IMapper mapper;

        public AppliedJobService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<CustomResponse<GetAppliedJobDto>> ApplyToJob(string jobSeekerId, string jobDetailId, CancellationToken cancellationToken)
        {
            var existingApplication = await ListAll().AnyAsync(c => c.JobDetailId == jobDetailId && c.JobSeekerId == jobSeekerId);
            if (existingApplication)
            {
                return new CustomResponse<GetAppliedJobDto>()
                {
                    Response = DTOs.Enums.ServiceResponses.BadRequest,
                    Message = "You have already applied to this job"
                };
            }

            var jobAPplication = new AppliedJob()
            {
                JobSeekerId = jobSeekerId,
                JobDetailId = jobDetailId,
                Status = AppliedJobStatusEnum.Pending
            };

            var createdResponse = await repository.AddAsync(jobAPplication, cancellationToken);
            if (createdResponse)
            {
                return await GetById(jobAPplication.Id, cancellationToken);
            }

            return new CustomResponse<GetAppliedJobDto>()
            {
                Response = DTOs.Enums.ServiceResponses.Failed,
                Message = "Unable to apply"
            };
        }

        public async Task<CustomResponse<GetAppliedJobDto>> GetById(string appliedJobId, CancellationToken cancellationToken)
        {
            var jobApplication = await ListAll().Include(c => c.JobDetail.Agency).FirstOrDefaultAsync(c => c.Id == appliedJobId, cancellationToken);
            if (jobApplication == null)
            {
                return new CustomResponse<GetAppliedJobDto>()
                {
                    Response = DTOs.Enums.ServiceResponses.NotFound,
                    Message = "Job Application not found"
                };
            }

            return new CustomResponse<GetAppliedJobDto>()
            {
                Response = DTOs.Enums.ServiceResponses.Success,
                Data = mapper.Map<GetAppliedJobDto>(jobApplication)
            };
        }

        public async Task<List<GetAppliedJobDto>> GetAppliedJobs(string jobSeekerId, CancellationToken cancellationToken)
        {
            var appliedJobs = await ListAll().Include(c => c.JobDetail.Agency).Where(c => c.JobSeekerId == jobSeekerId).ToListAsync(cancellationToken);

            return mapper.Map<List<GetAppliedJobDto>>(appliedJobs);
        }

        IQueryable<AppliedJob> ListAll()
        {
            return repository.ListAll<AppliedJob>();
        }
    }
}
