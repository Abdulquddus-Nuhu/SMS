using Access.API.Models.Requests;
using Access.API.Models.Responses;
using Access.API.Services.Interfaces;
using Access.Models.Requests;
using Access.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [ApiExplorerSettings(GroupName = "SPE Module")]
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonaController : BaseController
    {
        private readonly IPersonaService _personaService;

        public PersonaController(IPersonaService personaService)
        {
            _personaService = personaService;
        }


        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
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


        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
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


        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
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


        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
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

        [Authorize(Roles = AuthConstants.Roles.SUPER_ADMIN)]
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

        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
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

        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
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

        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
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

        //for edit student edpoint 







        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
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
        [HttpGet("busDriver")]
        public async Task<ActionResult<ApiResponse<List<BusDriverResponse>>>> GetBusDriverAsync()
        {
            var response = await _personaService.BusDriverListAsync();
            return HandleResult(response);
        }


        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
        [SwaggerOperation(
        Summary = "Delete A Parent Endpoint",
        Description = "This endpoint deletes a parent. It requires Admin privilege",
        OperationId = "parent.delete",
        Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpDelete("delete-parent")]
        public async Task<ActionResult<BaseResponse>> DeleteParentAsync(Guid parentId)
        {
            var response = await _personaService.DeleteParentAsync(parentId, User.Identity!.Name ?? string.Empty);
            return HandleResult(response);
        }


        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
        [SwaggerOperation(
        Summary = "Delete A Student Endpoint",
        Description = "This endpoint deletes a student. It requires Admin privilege",
        OperationId = "student.delete",
        Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpDelete("delete-student")]
        public async Task<ActionResult<BaseResponse>> DeleteStudentAsync(Guid studentId)
        {
            var response = await _personaService.DeleteStudnetAsync(studentId, User.Identity!.Name ?? string.Empty);
            return HandleResult(response);
        }

        //edit student 

        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
        [SwaggerOperation(
        Summary = "Edit Student Endpoint",
        Description = "This endpoint edits a student. It requires Admin privilege",
        OperationId = "student.edit",
        Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPut("edit-student")]
        public async Task<ActionResult<ApiResponse<StudentResponse>>> EditStudentAsync(Guid studentId, [FromBody] EditStudentRequest request)
        {
            var response = await _personaService.EditStudentAsync(studentId, request, User.Identity!.Name ?? string.Empty);
            return HandleResult(response);
        }


        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
        [SwaggerOperation(
         Summary = "Edit Parent Endpoint",
         Description = "This endpoint edits a parent. It requires Admin privilege",
         OperationId = "parent.edit",
         Tags = new[] { "PersonaEndpoints" })
         ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPut("edit-parent")]
        public async Task<ActionResult<BaseResponse>> EditParentAsync(Guid parentId, [FromBody] EditParentRequest request)
        {
            var response = await _personaService.EditParentAsync(parentId, request, User.Identity!.Name ?? string.Empty);
            return HandleResult(response);
        }









        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
        [SwaggerOperation(
        Summary = "Get A Parent Endpoint",
        Description = "This endpoint gets a parent. It requires Admin privilege",
        OperationId = "parent.get",
        Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<ParentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("parentId")]
        public async Task<ActionResult<ApiResponse<ParentResponse>>> GetParentAsync(Guid parentId)
        {
            var response = await _personaService.GetParentAsync(parentId);
            return HandleResult(response);
        }

        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
        [SwaggerOperation(
        Summary = "Get A Student Endpoint",
        Description = "This endpoint gets a student. It requires Admin privilege",
        OperationId = "student.get",
        Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("studentId")]
        public async Task<ActionResult<ApiResponse<StudentResponse>>> GetStudentAsync(Guid studentId)
        {
            var response = await _personaService.GetStudentAsync(studentId);
            return HandleResult(response);
        }

        [Authorize(Policy = AuthConstants.Policies.ADMINS)]
        [SwaggerOperation(
        Summary = "Get List Of Users Endpoint",
        Description = "This endpoint gets the list of Users . It requires Admin privilege",
        OperationId = "personas.get",
        Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<PersonaResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("users")]
        public async Task<ActionResult<ApiResponse<List<PersonaResponse>>>> GetUsersWithRoleAsync()
        {
            var response = await _personaService.GetUsersWithRole();
            return HandleResult(response);
        }

        [AllowAnonymous]
        [SwaggerOperation(
        Summary = "Delete A User Account Endpoint",
        Description = "This endpoint deletes a user account.",
        OperationId = "personaAccount.delete",
        Tags = new[] { "PersonaEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpDelete("delete-account")]
        public async Task<ActionResult<BaseResponse>> DeletePersonaAccountAsync(string email)
        {
            var response = await _personaService.DeletePersonaAccountAsync(email);
            return HandleResult(response);
        }

    }


}
