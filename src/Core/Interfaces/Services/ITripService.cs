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
        public Task<BaseResponse> CreateTripAsync(CreateTripRequest request, string driver);
        public Task<ApiResponse<List<TripResponse>>> TripListAsync();
    }
}
