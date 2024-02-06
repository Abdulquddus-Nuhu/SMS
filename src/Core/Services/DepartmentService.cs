using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Models.Requests;
using Models.Responses;
using Shared.Models.Responses;

namespace Core.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<BaseResponse> CreateDepartment(CreateDepartmentRequest request, string creator)
        {
            var department = new Department()
            {
                Id = Guid.NewGuid(),
                Name = request.DepartmentName,
                CreatedBy = creator,
                Created = DateTime.UtcNow,
            };

            return await _departmentRepository.AddDepartment(department);
        }

        public async Task<ApiResponse<List<DepartmentResponse>>> GetAllAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();

            return new ApiResponse<List<DepartmentResponse>>()
            {
                Data = await departments.Select(x => new DepartmentResponse()
                {
                    Id = x.Id,
                    DeparmentName = x.Name
                }).ToListAsync()
            };
        }
    }
}
