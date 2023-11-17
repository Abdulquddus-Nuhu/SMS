using Access.API.Services.Interfaces;
using Access.Models.Requests;
using Access.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Controllers;
using Shared.Models.Responses;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Access.API.Controllers
{
    [ApiExplorerSettings(GroupName = "Access")]
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaController : BaseController
    {
        private readonly IPersonaService _personaService;

        public PersonaController(IPersonaService personaService)
        {
            _personaService = personaService;
        }


        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
              Summary = "Create a new Parent Endpoint",
              Description = "This endpoint creates a new Parent. It requires Admin privilege",
              OperationId = "parent.create",
              Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ParentResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-parent")]
        public async Task<ActionResult<ApiResponse<ParentResponse>>> CreateParentAsync([FromForm] CreateParentRequest request)
        {
            var host = Request.Host.ToString();
            var response = await _personaService.CreateParentAsync(request, host);
            return HandleResult(response);
        }


        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
            Summary = "Create a new Student Endpoint",
            Description = "This endpoint creates a new Student. It requires Admin privilege",
            OperationId = "student.create",
            Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-student")]
        public async Task<ActionResult<ApiResponse<StudentResponse>>> CreateStudentAsync([FromForm] CreateStudentRequest request)
        {
            var host = Request.Host.ToString();
            var response = await _personaService.CreateStudentAsync(request, host);
            return HandleResult(response);
        }


        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
        Summary = "Create a new Bus Driver Endpoint",
        Description = "This endpoint creates a new Bus Driver. It requires Admin privilege",
        OperationId = "busDriver.create",
        Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-busDriver")]
        public async Task<ActionResult<BaseResponse>> CreateBusDriverAsync([FromForm] CreateBusDriverRequest request)
        {
            var host = Request.Host.ToString();

            var response = await _personaService.CreateBusDriverAsync(request, host);
            return HandleResult(response);
        }


        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
        Summary = "Create a new Staff Endpoint",
        Description = "This endpoint creates a new Staff. It requires Admin privilege",
        OperationId = "staff.create",
        Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-Staff")]
        public async Task<ActionResult<BaseResponse>> CreateStaffAsync([FromForm] CreateStaffRequest request)
        {
            var host = Request.Host.ToString();

            var response = await _personaService.CreateStaffAsync(request, host);
            return HandleResult(response);
        }

        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
        Summary = "Get List Of Parents Endpoint",
        Description = "This endpoint gets the list of parents . It requires Admin privilege",
        OperationId = "parents.get",
        Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<ParentResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("parents")]
        public async Task<ActionResult<ApiResponse<List<ParentResponse>>>> GetParentsAsync()
        {
            var response = await _personaService.ParentListAsync();
            return HandleResult(response);
        }

        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
        Summary = "Get List Of Students For A Parent Endpoint",
        Description = "This endpoint gets the list of students for a Parent. It requires Admin privilege",
        OperationId = "parentstudents.get",
        Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<StudentResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("parent-students")]
        public async Task<ActionResult<ApiResponse<List<StudentResponse>>>> GetParentStudentsAsync(Guid parentId)
        {
            var response = await _personaService.ParentStudentsListAsync(parentId);
            return HandleResult(response);
        }

        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
        Summary = "Get List Of Students Endpoint",
        Description = "This endpoint gets the list of students . It requires Admin privilege",
        OperationId = "students.get",
        Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<StudentResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("students")]
        public async Task<ActionResult<ApiResponse<List<StudentResponse>>>> GetStudentsAsync()
        {
            var response = await _personaService.StudentListAsync();
            return HandleResult(response);
        }

        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
        Summary = "Get List Of Staff Endpoint",
        Description = "This endpoint gets the list of staff . It requires Admin privilege",
        OperationId = "staff.get",
        Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<StudentResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("staff")]
        public async Task<ActionResult<ApiResponse<List<StaffResponse>>>> GetStaffAsync()
        {
            var response = await _personaService.StaffListAsync();
            return HandleResult(response);
        }


        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
           Summary = "Get List Of Busdriver Endpoint",
           Description = "This endpoint gets the list of Busdriver. It requires Admin privilege",
           OperationId = "BusDriver.get",
           Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<BusDriverResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet(" busDriver")]
        public async Task<ActionResult<ApiResponse<List<BusDriverResponse>>>> GetBusDriverAsync()
        {
            var response = await _personaService.BusDriverListAsync();
            return HandleResult(response);
        }
    }


}
