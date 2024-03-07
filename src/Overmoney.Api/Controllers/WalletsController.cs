using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.DataAccess.Wallets.Models;
using Overmoney.Api.Features.Wallets;

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
    public async Task<ActionResult<int>> GetWallet(int id)
    {
        var result = await _mediator.Send(new GetWalletQuery(id));

        if(result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    public async Task<ActionResult<int>> Create(CreateWalletCommand wallet)
    {
        var result = await _mediator.Send(wallet);
        return CreatedAtAction(nameof(GetWallet), new { id = result }, result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    public async Task<ActionResult<int>> Update(UpdateWalletCommand wallet)
    {
        var result = await _mediator.Send(wallet);

        if(result is null)
        {
            return Ok();
        }

        return CreatedAtAction(nameof(GetWallet), new { id = result }, result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteWalletCommand(id));
        return Accepted();
    }
}
