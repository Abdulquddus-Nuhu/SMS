using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Access.Models.Request;
using Module.Access.Models.Response;
using Module.Access.Services.Interfaces;
using Shared.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Module.Access.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Access")]
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

    }

}

