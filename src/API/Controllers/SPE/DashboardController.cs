using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Controllers;

namespace Access.API.Controllers
{
    [ApiExplorerSettings(GroupName = "SPE Module")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController
    {
    }
}
