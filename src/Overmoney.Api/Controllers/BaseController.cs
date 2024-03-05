using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Overmoney.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
public class BaseController : Controller
{
}
