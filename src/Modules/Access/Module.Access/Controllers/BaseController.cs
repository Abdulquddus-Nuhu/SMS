using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Access.Models.Response;
using static Shared.Constants.StringConstants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Module.Access.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Access")]
    public class BaseController : Controller
    {
        public ActionResult<TResponse> HandleResult<TResponse>(TResponse result) where TResponse : BaseResponse
        {
            return result.Code switch
            {
                0 => Ok(result),
                ResponseCodes.Status200OK => Ok(result),
                ResponseCodes.Status201Created => Created(string.Empty, result),
                ResponseCodes.Status400BadRequest => BadRequest(result),
                ResponseCodes.Status401Unauthorized => Unauthorized(result),
                ResponseCodes.Status403Forbidden => Forbid(result.ToString()),
                ResponseCodes.Status404NotFound => NotFound(result),
                ResponseCodes.Status500InternalServerError => StatusCode(StatusCodes.Status500InternalServerError, result),
                _ => StatusCode(StatusCodes.Status500InternalServerError, result),
            };
        }

    }
}

