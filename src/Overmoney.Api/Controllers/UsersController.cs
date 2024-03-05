using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.Features.Users;

namespace Overmoney.Api.Controllers;

public class UsersController : BaseController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("register")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<int> Register(RegisterUserCommand command)
    {
        var response = await _mediator.Send(command);

        return response;
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
        var response = await _mediator.Send(command);

        if(response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpDelete]
    [Route("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Remove(int id)
    {
        await _mediator.Send(new DeleteUserCommand(id));
        return Accepted();
    }
}
