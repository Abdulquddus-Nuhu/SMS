using Access.API.Models.Requests;
using Access.API.Models.Responses;
using Access.API.Services.Implementation;
using Access.API.Services.Interfaces;
using Access.Models.Requests;
using Access.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Controllers;
using Shared.Models.Responses;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Access.API.Controllers
{
    [ApiExplorerSettings(GroupName = "Access")]
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    public class ConfigSPEController : BaseController
    {
        private readonly IGradeService _gradeService;
        private readonly ICampusService _campusService;

        public ConfigSPEController(IGradeService gradeService, ICampusService campusService)
        {
            _gradeService = gradeService;
            _campusService = campusService;
        }


        //[Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
              Summary = "Create a new Campus Endpoint",
              Description = "This endpoint creates a new campus. It requires Admin privilege",
              OperationId = "campus.create",
              Tags = new[] { "SPE-Configurations-Endpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-campus")]
        public async Task<ActionResult<BaseResponse>> CreateCampusAsync(CreateCampusRequest request)
        {
            var response = await _campusService.CreateCampus(request);
            return HandleResult(response);
        }


        //[Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
        Summary = "Get List Of Campus Endpoint",
        Description = "This endpoint gets the list of campus. It requires Admin privilege",
        OperationId = "campuses.get",
        Tags = new[] { "SPE-Configurations-Endpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<CampusResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("campus-list")]
        public async Task<ActionResult<ApiResponse<List<CampusResponse>>>> GetCampusListAsync()
        {
            var response = await _campusService.GetAllAsync();
            return HandleResult(response);
        }



        //[Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
              Summary = "Create a new Grade Endpoint",
              Description = "This endpoint creates a new grade. It requires Admin privilege",
              OperationId = "grade.create",
              Tags = new[] { "SPE-Configurations-Endpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-grade")]
        public async Task<ActionResult<BaseResponse>> CreateGradeAsync(CreateGradeRequest request)
        {
            var response = await _gradeService.CreateGrade(request);
            return HandleResult(response);
        }


        //[Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
        Summary = "Get List Of Grade Endpoint",
        Description = "This endpoint gets the list of grade. It requires Admin privilege",
        OperationId = "grades.get",
        Tags = new[] { "SPE-Configurations-Endpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<GradeResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("grade-list")]
        public async Task<ActionResult<ApiResponse<List<GradeResponse>>>> GetGradesAsync()
        {
            var response = await _gradeService.GetAllAsync();
            return HandleResult(response);
        }
    }
}
