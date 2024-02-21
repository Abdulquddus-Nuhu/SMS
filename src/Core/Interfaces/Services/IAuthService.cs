using Models.Requests;
using Models.Responses;
using Shared.Models.Requests;
using Shared.Models.Responses;

namespace Core.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
        public Task<BaseResponse> ChangePasswordAsync(ChangePasswordRequest request, string userName);
        public Task<BaseResponse> LogoutAsync(string userName);
        public Task<BaseResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        public Task<ApiResponse<string>> VerifyResetOtpAsync(VerifyOtpRequest request);
        public Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequest request);
        public Task<BaseResponse> ResetPasswordByAdminAsync(ResetPasswordByAdminRequest request, string modifier);
    }
}
