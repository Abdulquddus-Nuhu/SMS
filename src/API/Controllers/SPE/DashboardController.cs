using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Requests;
using Models.Responses;
using Shared.Constants;
using Shared.Controllers;
using Shared.Models.Responses;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace API.Controllers.SPE
{
    [ApiExplorerSettings(GroupName = "SPE Module")]
    //[Route("api/[controller]")]
    [Route("api/dashboard")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashboardController : BaseController
    {
        private readonly IQrCodeService _qrCodeService;

        public DashboardController(IQrCodeService qrCodeService)
        {
            _qrCodeService = qrCodeService;
        }


        [Authorize(Roles = AuthConstants.Roles.SUPER_ADMIN + ", " + AuthConstants.Roles.PARENT)]
        [SwaggerOperation(
        Summary = "Get List Of Students For A Parent Dashboard Endpoint",
        Description = "This endpoint gets the list of students for a Parent to be displayed in dashboard. It requires Parent privilege",
        OperationId = "parentDashboard.get",
        Tags = new[] { "DashboardEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<StudentInSchoolResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("parent-mobile")]
        public async Task<ActionResult<ApiResponse<List<StudentInSchoolResponse>>>> GetParentStudentsAsync()
        {
            var response = await _qrCodeService.GetTodaysQrCodeAsync(User.Identity!.Name ?? string.Empty);
            return HandleResult(response);
        }

    }
}
