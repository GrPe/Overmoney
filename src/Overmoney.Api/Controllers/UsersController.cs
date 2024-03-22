using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.Features.Categories.Queries;
using Overmoney.Api.Features.Payees.Queries;
using Overmoney.Api.Features.Users.Commands;
using Overmoney.Api.Features.Wallets.Queries;
using Overmoney.Api.Features.Categories.Models;
using Overmoney.Api.Features.Payees.Models;
using Overmoney.Api.Features.Wallets.Models;

namespace Overmoney.Api.Controllers;

public class UsersController : BaseController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }


    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="command"></param>
    /// <returns>TBD</returns>
    [HttpPost]
    [Route("register")]
    [ProducesResponseType<int>(StatusCodes.Status202Accepted)]
    public async Task<ActionResult<int>> Register(RegisterUserCommand command)
    {
        var response = await _mediator.Send(command);

        return Accepted(response);
    }

    /// <summary>
    /// Authorize user
    /// </summary>
    /// <param name="command"></param>
    /// <returns>TBD</returns>
    [HttpPost]
    [Route("login")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> Login(LoginUserCommand command)
    {
        var response = await _mediator.Send(command);

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="id">User Id</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Remove(int id)
    {
        await _mediator.Send(new DeleteUserCommand(id));
        return Accepted();
    }


    /// <summary>
    /// Retrieve user's wallets
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <returns>List of wallets</returns>
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

    /// <summary>
    /// Retrieve user's categories
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <returns>List of categories</returns>
    [HttpGet("{userId}/categories")]
    [ProducesResponseType<IEnumerable<Category>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Category>>> GetUserCategories(int userId)
    {
        var result = await _mediator.Send(new GetAllCategoriesByUserQuery(userId));

        if (result is null || !result.Any())
        {
            return NotFound();
        }
        return Ok(result);
    }

    /// <summary>
    /// Retrieve user's payees
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <returns>List of payees</returns>
    [HttpGet("{userId}/payees")]
    [ProducesResponseType<IEnumerable<Payee>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Payee>>> GetUserPayees(int userId)
    {
        var result = await _mediator.Send(new GetAllPayeesByUserIdQuery(userId));

        if(result is null || !result.Any())
        {
            return NotFound();
        }
        return Ok(result);
    }
}
