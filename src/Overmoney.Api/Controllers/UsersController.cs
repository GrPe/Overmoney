using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Categories.Queries;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Payees.Queries;
using Overmoney.Domain.Features.Transactions.Models;
using Overmoney.Domain.Features.Transactions.Queries;
using Overmoney.Domain.Features.Users.Commands;
using Overmoney.Domain.Features.Wallets.Models;
using Overmoney.Domain.Features.Wallets.Queries;

namespace Overmoney.Api.Controllers;

[Authorize]
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
        await _mediator.Send(new DeleteUserCommand(new(id)));
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
        var result = await _mediator.Send(new GetUserWalletsQuery(new(userId)));

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
        var result = await _mediator.Send(new GetAllCategoriesByUserQuery(new(userId)));

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
        var result = await _mediator.Send(new GetAllPayeesByUserIdQuery(new(userId)));

        if(result is null || !result.Any())
        {
            return NotFound();
        }
        return Ok(result);
    }

    /// <summary>
    /// Retrieve user's transactions by query
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId}/transactions")]
    [ProducesResponseType<IEnumerable<Transaction>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetUserTransactions(int userId)
    {
        var result = await _mediator.Send(new GetUserTransactionsQuery(new(userId)));
        return result is null || !result.Any() ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Retrieve user's recurring transactions
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <returns></returns>
    [HttpGet("{userId}/transactions/recurring")]
    [ProducesResponseType<IEnumerable<RecurringTransaction>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<RecurringTransaction>>> GetUserRecurringTransactions(int userId)
    {
        var result = await _mediator.Send(new GetRecurringTransactionsByUserIdQuery(new(userId)));

        if (result is null || !result.Any())
        {
            return NotFound();
        }

        return Ok(result);
    }

}
