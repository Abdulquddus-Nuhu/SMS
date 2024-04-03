using Models.Requests;
using Shared.Models.Requests;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IQrCodeService
    {
        public Task<ApiResponse<List<StudentInSchoolResponse>>> GetTodaysQrCodeAsync(string email);
        public Task<ApiResponse<GenerateQrCodeResponse>> CreateQrCodeAsync(GenerateQrCodeRequest request);
        public Task<BaseResponse> AuthorizeQrCode(AuthorizeQrCodeRequest request);
        public Task<ApiResponse<List<StudentWithQrCodeResponse>>> GetParentStudentsAsync(string email);
        public Task<ApiResponse<List<GenerateQrCodeResponse>>> GenerateQrCodesForTripAsync(Guid tripId, string busDriverEmail);
        public Task<ApiResponse<ScanQrCodeResponse>> ScanQrCodeAsync(string qrCodeData, string user);
    }
}
