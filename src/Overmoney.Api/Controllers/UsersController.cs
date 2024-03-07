using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.DataAccess.Wallets.Models;
using Overmoney.Api.Features.Users;
using Overmoney.Api.Features.Wallets;

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
    [ProducesResponseType<int>(StatusCodes.Status202Accepted)]
    public async Task<ActionResult<int>> Register(RegisterUserCommand command)
    {
        var response = await _mediator.Send(command);

        return Accepted(response);
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> Login(LoginUserCommand command)
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


    [HttpGet("{userId}/wallets")]
    [ProducesResponseType<IEnumerable<Wallet>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> GetUserWallets(int userId)
    {
        var result = await _mediator.Send(new GetUserWalletsQuery(userId));

        if (result is null || !result.Any())
        {
            return NotFound();
        }

        return Ok(result);
    }
}
