using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.Features;
using Overmoney.Api.Features.Users;

namespace Overmoney.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Produces("application/json")]
public class UsersController : Controller
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType<Response>(StatusCodes.Status200OK)]
    public async Task<Response> Register(RegisterUserCommand command)
    {
        var response = await _mediator.Send(command);

        return response;
    }
}
