using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Shared.Constants.StringConstants;

namespace Shared.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
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
