using Core.Interfaces.Infrastructure;
using Core.Interfaces.Services;
using Core.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Models.Requests;
using Models.Responses;
using Shared.Models.Responses;
using static Shared.Constants.StringConstants;
using Infrastructure.Identity;
using Shared.Models.Requests;
using OtpNet;

namespace Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<Persona> _signInManager;
        private readonly UserManager<Persona> _userManager;
        private readonly ILogger<AuthService> _logger;
        private readonly HttpContext _httpContext;
        private readonly ITokenService _tokenService;
        private readonly IOtpGenerator _otpGenerator;
        private readonly IMediator _mediator;

        public AuthService(SignInManager<Persona> signInManager,
            UserManager<Persona> userManager,
            ILogger<AuthService> logger,
            IHttpContextAccessor httpContextAccessor,
            ITokenService tokenService,
            IOtpGenerator otpGenerator,
            IMediator mediator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext!;
            _tokenService = tokenService;
            _otpGenerator = otpGenerator;
            _mediator = mediator;
        }

        public async Task<BaseResponse> ChangePasswordAsync(ChangePasswordRequest request, string userName)
        {
            var response = new BaseResponse();
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
            {
                response.Message = "Invalid User";
                return response;
            }
            var changeResult = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            if (!changeResult.Succeeded)
            {
                response.Message = "Change password failed!";
                response.Status = false;
            }

            return response;
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var response = new ApiResponse<LoginResponse>() { Data = new LoginResponse(), Code = ResponseCodes.Status200OK };

            _logger.LogInformation("A user with email {0} is trying to login", request.Email);

            var loginResult = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, true);

            _logger.LogInformation("Login by a user {0} is {1}", request.Email, loginResult.Succeeded ? "successful" : "not successful");

            response.Data.Result = loginResult.Succeeded;
            response.Data.IsLockedOut = loginResult.IsLockedOut;
            response.Data.Email = request.Email;

            if (!loginResult.Succeeded)
            {
                response.Code = ResponseCodes.Status400BadRequest;
                response.Status = false;
                //response.Message = "Authentication failed!";
                response.Message = "The email or password you entered is incorrect. Please try again!";
                return response;
            }
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                _logger.LogWarning("Unable to find user {0} in the userManager store after a successfull Sign in", request.Email);

                response.Code = ResponseCodes.Status400BadRequest;
                response.Status = false;
                response.Message = "Authentication failed!";
                return response;
            }

            if (user.IsActive is false)
            {
                _logger.LogWarning("Login with email {0} was deleted not active in the userManager store after a successfull Sign in", request.Email);

                response.Code = ResponseCodes.Status404NotFound;
                response.Status = false;
                response.Message = $"Account not found for {request.Email}!";
                return response;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            response.Data.FullName = user.FullName;
            response.Data.PhoneNumber = user.PhoneNumber;
            response.Data.PhotoUrl = user.PhotoUrl;
            response.Data.Roles = userRoles;

            var tokenResult = _tokenService.GetToken(new PersonaResponse() { UserName = user.UserName, Email = user.Email, FirstName = user.FirstName, Id = user.Id, LastName = user.LastName, PhoneNumber = user.PhoneNumber, Roles = userRoles, });
            response.Data.Token = tokenResult.Token;
            response.Message = "Login Successfully!";
            response.Data.TokenExpiryDate = tokenResult.ExpiryDate;

            return response;

        }

        public async Task<BaseResponse> LogoutAsync(string userName)
        {
            var response = new BaseResponse();

            await _signInManager.SignOutAsync();

            _logger.LogInformation("Loggin out user {0}", userName);

            //await _tokenService.InvalidateAllTokens(userName);

            //TODO: Current token invalidation

            response.Message = "Successfully logout";

            return response;
        }

        public async Task<BaseResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var response = new BaseResponse() { Message = "Forgot Password OTP Has been sent to your email if your account exist!" };

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                response.Code = ResponseCodes.Status404NotFound;
                response.Message = "User Not Found";
                response.Status = false;
                return response;
            }

            _logger.LogInformation($"User {user.Email} has requested to reset his password");

            var otp = _otpGenerator.Generate(user.UserName, digitsCount: 6);

            _ = _mediator.Publish(new ForgotPasswordEvent(new PersonaResponse() { Email = user.Email, FirstName = user.FirstName, Id = user.Id, LastName = user.LastName, PhoneNumber = user.PhoneNumber, UserName = user.UserName }, otp));

            return response;
        }

        public async Task<ApiResponse<string>> VerifyResetOtpAsync(VerifyOtpRequest request)
        {
            var response = new ApiResponse<string>();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                _logger.LogWarning("User: {0} not found on userManager store", request.Email);
                response.Message = "Invalid OTP Supplied!";
                response.Code = ResponseCodes.Status400BadRequest;
                return response;
            }


            _logger.LogInformation("Verifying forgot password otp for user: {0}", request.Email);

            var isOtpValid = _otpGenerator.Verify(request.Email, request.Otp, digitsCount: 6);

            if (!isOtpValid)
            {
                _logger.LogInformation("Otp supplied by user: {0} to reset password is not valid", request.Email);
                response.Message = "Invalid OTP Supplied!";
                response.Code = ResponseCodes.Status400BadRequest;
                return response;
            }
            _logger.LogInformation("Otp supplied by user: {0} to reset password is valid", request.Email);

            var setPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            response.Data = setPasswordToken;

            return response;
        }

        public async Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = new BaseResponse();
            _logger.LogInformation("Resseting password for user {0}", request.Email);
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                response.Status = false;
                response.Code = ResponseCodes.Status404NotFound;
                _logger.LogWarning("User {0} not found in userManager store", request.Email);
                response.Message = "Invalid User!";
                return response;
            }
            var resetResult = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!resetResult.Succeeded)
            {
                response.Code = ResponseCodes.Status500InternalServerError;
                _logger.LogWarning("Reseting password for user {0} is not successful", request.Email);
                response.Message = string.Join(",", resetResult.Errors.Select(a => a.Description).ToArray());
                _logger.LogWarning(response.Message);
            }

            _logger.LogInformation("Reseting password for user {0} is successful", request.Email);

            return response;
        }

        public async Task<BaseResponse> ResetPasswordByAdminAsync(ResetPasswordByAdminRequest request, string modifier)
        {
            var response = new BaseResponse();
            _logger.LogInformation("Resseting password for user {0}, by Admin {1}", request.Email, modifier);
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                response.Status = false;
                response.Code = ResponseCodes.Status404NotFound;
                _logger.LogWarning("User {0} not found in userManager store", request.Email);
                response.Message = "Invalid User!";
                return response;
            }

            // Generate a password reset token
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetResult = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!resetResult.Succeeded)
            {
                response.Code = ResponseCodes.Status500InternalServerError;
                _logger.LogWarning("Reseting password for user {0} is not successful", request.Email);
                response.Message = string.Join(",", resetResult.Errors.Select(a => a.Description).ToArray());
                _logger.LogWarning(response.Message);
            }

            _ = _mediator.Publish(new ResetPasswordByAdminEvent(new PersonaResponse() { Email = user.Email, FirstName = user.FirstName, Id = user.Id, LastName = user.LastName, PhoneNumber = user.PhoneNumber, UserName = user.UserName }, request.NewPassword));


            _logger.LogInformation("Reseting password for user {0} is successful", request.Email);

            return response;
        }


    }

}
