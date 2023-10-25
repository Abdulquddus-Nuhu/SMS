using System;
using Module.Access.Models.Request;
using Module.Access.Models.Response;

namespace Module.Access.Services.Interfaces
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

