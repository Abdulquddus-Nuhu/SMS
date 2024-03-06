using Core.Interfaces.Services;
using Core.Services;
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
        private readonly ITripService _tripService;

        public QrCodeController(IQrCodeService qrCodeService, ITripService tripService)
        {
            _qrCodeService = qrCodeService;
            _tripService = tripService;
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
        Description = "This endpoint authorizes a qrCode. For AuthorizedUser :- Self = 0, Other = 1. It requires Parent privilege",
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

        [Authorize(Roles = AuthConstants.Roles.SUPER_ADMIN + ", " + AuthConstants.Roles.PARENT)]
        [SwaggerOperation(
         Summary = "Get List Of Students And QrCode Status For the Day For A Parent Dashboard Endpoint",
         Description = "It requires Parent privilege",
         OperationId = "childrenQrcode.get",
         Tags = new[] { "QrCodeEndpoints" })
         ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<StudentWithQrCodeResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("students")]
        public async Task<ActionResult<ApiResponse<List<StudentWithQrCodeResponse>>>> GetParentStudentsAsync()
        {
            var response = await _qrCodeService.GetParentStudentsAsync(User.Identity!.Name ?? string.Empty);
            return HandleResult(response);
        }

        //[Authorize(Roles = AuthConstants.Roles.BUS_DRIVER + ", " + AuthConstants.Roles.SUPER_ADMIN)]
        [SwaggerOperation(
        Summary = "Create a new trip Endpoint",
        Description = "Requires Bus Driver or Super Admin privileges. For TripType :- PickUp = 0, DropOff = 1. ",
        OperationId = "trip.create",
        Tags = new[] { "QrCodeEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-trip")]
        public async Task<ActionResult<BaseResponse>> CreateTripAsync(CreateTripRequest request)
        {
            var response = await _tripService.CreateTripAsync(request, User.Identity!.Name ?? string.Empty);
            return HandleResult(response);
        }
        
        //[Authorize(Roles = AuthConstants.Roles.BUS_DRIVER + ", " + AuthConstants.Roles.SUPER_ADMIN)]
        [SwaggerOperation(
        Summary = "Gets the list of Endpoint",
        Description = "Requires Bus Driver or Admin privileges",
        OperationId = "trip.list",
        Tags = new[] { "QrCodeEndpoints" })
        //Tags = new[] { "TripEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<TripResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("trip-list")]
        public async Task<ActionResult<ApiResponse<List<TripResponse>>>> TripListAsync()
        {
            var response = await _tripService.TripListAsync();
            return HandleResult(response);
        }
    }
}
