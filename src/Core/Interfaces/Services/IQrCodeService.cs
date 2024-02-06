using Models.Requests;
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
    }
}
