using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Categories.Queries;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Payees.Queries;
using Overmoney.Domain.Features.Transactions.Models;
using Overmoney.Domain.Features.Transactions.Queries;
using Overmoney.Domain.Features.Users.Commands;
using Overmoney.Domain.Features.Users.Models;
using Overmoney.Domain.Features.Wallets.Models;
using Overmoney.Domain.Features.Wallets.Queries;

namespace Overmoney.Api.Controllers;

[Authorize]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly SignInManager<IdentityUser> _signInManager;

    public UsersController(
        IMediator mediator, 
        SignInManager<IdentityUser> signInManager)
    {
        _mediator = mediator;
        _signInManager = signInManager;
    }

    /// <summary>
    /// Create new user profile
    /// </summary>
    /// <param name="command"></param>
    /// <returns>User profile</returns>
    [HttpPost]
    [Route("profile")]
    [AllowAnonymous]
    [ProducesResponseType<UserProfile>(StatusCodes.Status201Created)]
    public async Task<ActionResult<UserProfile>> CreateUserProfile(CreateUserProfileCommand command)
    {
        var response = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetUserProfile), response);
    }

    /// <summary>
    /// Get user profile
    /// </summary>
    /// <returns>User profile</returns>
    [HttpGet]
    [Route("profile")]
    [ProducesResponseType<UserProfile>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<UserProfile>> GetUserProfile()
    {
        if( _signInManager.Context.Request.HttpContext.User?.Identity?.IsAuthenticated == false)
        {
            return Unauthorized();
        }

        var userName =_signInManager.Context.Request.HttpContext.User?.Identity?.Name;

        if (userName is null)
        {
            return Unauthorized();
        }

        var userProfile = await _mediator.Send(new GetUserProfile(userName));

        if (userProfile is null)
        {
            return NotFound();
        }

        return Ok(userProfile);
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
    /// <param name="categoryId">Category Id</param>
    /// <param name="payeeId">Payee Id</param>
    /// <param name="walletId">Wallet Id</param>
    /// <returns></returns>
    [HttpGet("{userId}/transactions")]
    [ProducesResponseType<IEnumerable<Transaction>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetUserTransactions(
        int userId, 
        [FromQuery] int? walletId, 
        [FromQuery] int? categoryId,
        [FromQuery] int? payeeId)
    {
        var query = new GetUserTransactionsQuery(
            new(userId), 
            walletId is not null ? new(walletId.Value) : null,
            categoryId is not null ? new(categoryId.Value) : null, 
            payeeId is not null ? new(payeeId.Value) : null
            );

        var result = await _mediator.Send(query);
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
