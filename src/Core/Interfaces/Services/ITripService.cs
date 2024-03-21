using Models.Responses;
using Shared.Models.Requests;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface ITripService
    {
        public Task<ApiResponse<Guid?>> CreateTripAsync(CreateTripRequest request, string driver);
        public Task<ApiResponse<List<TripResponse>>> TripListAsync();
        public Task<BaseResponse> AddStudentToTripAsync(AddStudentToTripRequest request);
        public Task<ApiResponse<IEnumerable<StudentResponse>>> GetNotOnboardedStudentAsync(Guid tripId, string busDriverEmail);
        public Task<ApiResponse<IEnumerable<StudentResponse>>> GetOnboardedStudentAsync(Guid tripId, string busDriverEmail);
        public Task<BaseResponse> RemoveStudentFromTripAsync(RemoveStudentFromTripRequest request);
    }
}
