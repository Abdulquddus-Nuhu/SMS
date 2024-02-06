using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Models.Requests;
using Models.Responses;
using Shared.Models.Responses;

namespace Core.Services
{
    public class JobTitleService : IJobTitleService
    {
        private readonly IJobTitleRepository _jobTitleRepository;

        public JobTitleService(IJobTitleRepository jobTitleRepository)
        {
            _jobTitleRepository = jobTitleRepository;
        }

        public async Task<BaseResponse> CreateJobTitle(CreateJobTitleRequest request)
        {
            var jobTitle = new JobTitle()
            {
                Name = request.Name,
                
          
            };

            var result = await _jobTitleRepository.AddJobTitle(jobTitle);
            return result;
        }

        // Add additional methods as needed for job title-related operations

        public async Task<ApiResponse<List<JobTitleRespone>>> GetAllAsync()
        {
            var jobTitle = await _jobTitleRepository.GetAllAsync();
            return new ApiResponse<List<JobTitleRespone>>()
            {
                Data = await jobTitle.Select(x => new JobTitleRespone()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync()
            };
        }
    }


}
