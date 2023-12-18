﻿using Access.API.Models.Requests;
using Access.API.Models.Responses;
using Access.API.Services.Implementation;
using Access.API.Services.Interfaces;
using Access.Models.Requests;
using Access.Models.Responses;
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
    [ApiExplorerSettings(GroupName = "Access")]
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ConfigSPEController(IGradeService gradeService, ICampusService campusService,
        IJobTitleService jobTitleService,
        IBusService busService) : BaseController
    {
        private readonly IGradeService _gradeService = gradeService;
        private readonly ICampusService _campusService = campusService;
        private readonly IJobTitleService _jobTitleService = jobTitleService;
        private readonly IBusService _busService = busService;

        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
              Summary = "Create a new Campus Endpoint",
              Description = "This endpoint creates a new campus. It requires Admin privilege",
              OperationId = "campus.create",
              Tags = new[] { "SPE-Configuration-Endpoints" })
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
            var response = await _campusService.CreateCampus(request, User.Identity!.Name ?? string.Empty);
            return HandleResult(response);
        }


        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
        Summary = "Get List Of Campus Endpoint",
        Description = "This endpoint gets the list of campus. It requires Admin privilege",
        OperationId = "campuses.get",
        Tags = new[] { "SPE-Configuration-Endpoints" })
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


        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
        Summary = "Get List Of Campus Including Grades Endpoint",
        Description = "This endpoint gets the list of campus including the grades associated with the campus. It requires Admin privilege",
        OperationId = "campusesGrades.get",
        Tags = new[] { "SPE-Configuration-Endpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<CampusResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("campus-grades-list")]
        public async Task<ActionResult<ApiResponse<List<CampusResponse>>>> GetCampusWithGradesAsync()
        {
            var response = await _campusService.GetCampusWithGradesAsync();
            return HandleResult(response);
        }



        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
              Summary = "Create a new Grade Endpoint",
              Description = "This endpoint creates a new grade. It requires Admin privilege",
              OperationId = "grade.create",
              Tags = new[] { "SPE-Configuration-Endpoints" })
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


        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
        Summary = "Get List Of Grade Endpoint",
        Description = "This endpoint gets the list of grade. It requires Admin privilege",
        OperationId = "grades.get",
        Tags = new[] { "SPE-Configuration-Endpoints" })
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



        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
        Summary = "Create JobTitle Endpoint",
        Description = "This endpoint create a  JobTitle. It requires Admin privilege",
        OperationId = "jobTitle.post",
        Tags = new[] { "SPE-Configuration-Endpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<JobTitleRespone>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-jobTitle")]
        public async Task<ActionResult<BaseResponse>> CreateJobTitleAsync(CreateJobTitleRequest request )
        {
            var response = await _jobTitleService.CreateJobTitle( request);
            return HandleResult(response);
        }




        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
        Summary = "Get List Of JobTitle Endpoint",
        Description = "This endpoint gets the list of JobTitle. It requires Admin privilege",
        OperationId = "jobTitle.get",
        Tags = new[] { "SPE-Configuration-Endpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<JobTitleRespone>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("jobTitle-list")]
        public async Task<ActionResult<ApiResponse<List<JobTitleRespone>>>> GetJobTitleAsync()
        {
            var response = await _jobTitleService.GetAllAsync();
            return HandleResult(response);
        }


        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
          Summary = "Create a new Bus Endpoint",
          Description = "This endpoint creates a new bus. It requires Admin privilege",
          OperationId = "bus.create",
          Tags = new[] { "SPE-Configuration-Endpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("create-bus")]
        public async Task<ActionResult<BaseResponse>> CreateBusAsync(CreateBusRequest request)
        {
            var response = await _busService.CreatBus(request, User.Identity!.Name ?? string.Empty);
            return HandleResult(response);
        }


        [Authorize(Policy = AuthConstants.Policies.CUSTODIANS)]
        [SwaggerOperation(
        Summary = "Get List Of Bus Endpoint",
        Description = "This endpoint gets the list of bus. It requires Admin privilege",
        OperationId = "buses.get",
        Tags = new[] { "SPE-Configuration-Endpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<BusResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("bus-list")]
        public async Task<ActionResult<ApiResponse<List<BusResponse>>>> GetBusesAsync()
        {
            var response = await _busService.GetAllAsync();
            return HandleResult(response);
        }


    }
}
