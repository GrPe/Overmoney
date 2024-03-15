using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.Features.Wallets.Models;
using Overmoney.Api.Features.Wallets.Commands;
using Overmoney.Api.Features.Wallets.Queries;

namespace Overmoney.Api.Controllers;

public class WalletsController : BaseController
{
    private readonly IMediator _mediator;

    public WalletsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType<Wallet>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Wallet>> GetWallet(int id)
    {
        var result = await _mediator.Send(new GetWalletQuery(id));

        if(result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<Wallet>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Wallet>> Create(CreateWalletCommand wallet)
    {
        var result = await _mediator.Send(wallet);
        return CreatedAtAction(nameof(GetWallet), new { id = result.Id }, result);
    }

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

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteWalletCommand(id));
        return Accepted();
    }
}
