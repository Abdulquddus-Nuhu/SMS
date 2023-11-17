using Access.Models.Requests;
using Access.Models.Responses;
using Shared.Models.Responses;

namespace Access.API.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
        public Task<BaseResponse> ChangePasswordAsync(ChangePasswordRequest request, string userName);
        public Task<BaseResponse> LogoutAsync(string userName);
        public Task<BaseResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        public Task<ApiResponse<string>> VerifyResetOtpAsync(VerifyOtpRequest request);
        public Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
