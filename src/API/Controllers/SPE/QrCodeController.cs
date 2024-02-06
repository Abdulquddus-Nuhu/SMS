using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Requests;
using Shared.Constants;
using Shared.Controllers;
using Shared.Models.Requests;
using Shared.Models.Responses;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace API.Controllers.SPE
{
    [ApiExplorerSettings(GroupName = "SPE Module")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class QrCodeController : BaseController
    {
        private readonly IQrCodeService _qrCodeService;

        public QrCodeController(IQrCodeService qrCodeService)
        {
            _qrCodeService = qrCodeService;
        }


        [Authorize(Roles = AuthConstants.Roles.SUPER_ADMIN + ", " + AuthConstants.Roles.PARENT)]
        [SwaggerOperation(
            Summary = "Generate a new QrCode By Parent Endpoint",
            Description = "This endpoint generates a new data for qrCode. For AuthorizedUser :- Self = 0, Other = 1. It requires Parent privilege",
            OperationId = "qrCode.create",
            Tags = new[] { "QrCodeEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<GenerateQrCodeResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("generate-qrcode")]
        public async Task<ActionResult<ApiResponse<GenerateQrCodeResponse>>> CreateQrCodeAsync(GenerateQrCodeRequest request)
        {
            request.UserEmail = User.Identity!.Name ?? string.Empty;

            var response = await _qrCodeService.CreateQrCodeAsync(request);
            return HandleResult(response);
        }

        [Authorize(Roles = AuthConstants.Roles.SUPER_ADMIN + ", " + AuthConstants.Roles.PARENT)]
        [SwaggerOperation(
        Summary = "Authorizes a QrCode By Parent Endpoint",
        Description = "This endpoint authorizes a qrCode. It requires Parent privilege",
        OperationId = "qrCode.edit",
        Tags = new[] { "QrCodeEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("authorize-qrcode")]
        public async Task<ActionResult<BaseResponse>> AuthorizeQrCodeAsync(AuthorizeQrCodeRequest request)
        {
            var response = await _qrCodeService.AuthorizeQrCode(request);
            return HandleResult(response);
        }
    }
}
