using Models.Requests;
using Models.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Controllers;
using Shared.Models.Responses;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using Core.Interfaces.Services;
using Shared.Models.Requests;
using Shared.Constants;

namespace API.Controllers.SPE
{
    [ApiExplorerSettings(GroupName = "SPE Module")]
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Authenticates a user and generate JWT Token",
            Description = "Authenticates a user and generate JWT Token",
            OperationId = "auth.login",
            Tags = new[] { "AuthEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> LoginAsync([FromBody] LoginRequest request)
        {
            var loginResponse = await _authService.LoginAsync(request);
            return HandleResult(loginResponse);
        }


        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Initiate Forgot Password process",
            Description = "This endpoint intiate a processes to reset a user password if forgotten",
            OperationId = "auth.forgotpassword",
            Tags = new[] { "PasswordEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<BaseResponse>> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request)
        {
            var forgotResponse = await _authService.ForgotPasswordAsync(request);
            return HandleResult(forgotResponse);
        }


        [AllowAnonymous]
        [SwaggerOperation(
        Summary = "Verify OTP and return reset password token",
        Description = "This endpoint verifies an OTP",
        OperationId = "auth.verifyotp",
        Tags = new[] { "PasswordEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("VerifyOtp")]
        public async Task<ActionResult<ApiResponse<string>>> VerifyResetOtpAsync([FromBody] VerifyOtpRequest request)
        {
            var verifyResponse = await _authService.VerifyResetOtpAsync(request);
            return HandleResult(verifyResponse);
        }


        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Set a new Password",
            Description = "This endpoint reset user password with new one",
            OperationId = "auth.resetpassword",
            Tags = new[] { "PasswordEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("ResetPassword")]
        public async Task<ActionResult<BaseResponse>> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            var resetResponse = await _authService.ResetPasswordAsync(request);
            return HandleResult(resetResponse);
        }


        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
        [SwaggerOperation(
            Summary = "ReSet a User Password By Admin",
            Description = "This endpoint reset user password with new one.It requires Admin priveleges.",
            OperationId = "auth.resetpasswordbyadmin",
            Tags = new[] { "PasswordEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("ResetPasswordByAdmin")]
        public async Task<ActionResult<BaseResponse>> ResetPasswordByAdminAsync([FromBody] ResetPasswordByAdminRequest request)
        {
            var resetResponse = await _authService.ResetPasswordByAdminAsync(request, User.Identity!.Name ?? string.Empty);
            return HandleResult(resetResponse);
        }

        //[SwaggerOperation(
        //    Summary = "Logout a user",
        //    Description = "This endpoint invalidates all refresh tokens and jwt tokens of a user",
        //    OperationId = "auth.logout",
        //    Tags = new[] { "AuthEndpoints" })
        //]
        //[Produces(MediaTypeNames.Application.Json)]
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        //[HttpPost("Logout")]
        //public async Task<ActionResult<BaseResponse>> LogoutAsync()
        //{
        //    var logoutResponse = await _authService.LogoutAsync(User.Identity?.Name ?? string.Empty);
        //    return Ok(logoutResponse);
        //}


        //[Authorize]
        //[SwaggerOperation(
        //    Summary = "Change user Password",
        //    Description = "This endpoint change user password with new one",
        //    OperationId = "auth.chnagepassword",
        //    Tags = new[] { "PasswordEndpoints" })
        //]
        //[Produces(MediaTypeNames.Application.Json)]
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        //[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        //[HttpPost("ChangePassword")]
        //public async Task<ActionResult<BaseResponse>> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
        //{
        //    var resetResponse = await _authService.ChangePasswordAsync(request, User.Identity?.Name ?? string.Empty);
        //    return Ok(resetResponse);
        //}

    }
}
