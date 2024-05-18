using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Domain.Features.Wallets.Commands;
using Overmoney.Domain.Features.Wallets.Models;
using Overmoney.Domain.Features.Wallets.Queries;

namespace Overmoney.Api.Controllers;

[Authorize]
public class WalletsController : BaseController
{
    private readonly IMediator _mediator;

    public WalletsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieve wallet by Id
    /// </summary>
    /// <param name="id">Wallet Id</param>
    /// <returns>Wallet entity</returns>
    [HttpGet("{id}")]
    [ProducesResponseType<Wallet>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Wallet>> GetWallet(int id)
    {
        var result = await _mediator.Send(new GetWalletQuery(new(id)));

        if(result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Create new wallet
    /// </summary>
    /// <param name="wallet"></param>
    /// <returns>Created wallet</returns>
    [HttpPost]
    [ProducesResponseType<Wallet>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Wallet>> Create(CreateWalletCommand wallet)
    {
        var result = await _mediator.Send(wallet);
        return CreatedAtAction(nameof(GetWallet), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update wallet or create new one
    /// </summary>
    /// <param name="wallet"></param>
    /// <returns>Created wallet</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Wallet>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Wallet>> Update(UpdateWalletCommand wallet)
    {
        var result = await _mediator.Send(wallet);

        if(result is null)
        {
            return Ok();
        }

        return CreatedAtAction(nameof(GetWallet), new { id = result.Id }, result);
    }

    /// <summary>
    /// Delete wallet
    /// </summary>
    /// <param name="id">Wallet Id</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteWalletCommand(new(id)));
        return Accepted();
    }
}
