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
    public async Task<IActionResult> GetWallet(int id)
    {
        var result = await _mediator.Send(new GetWalletQuery(id));

        if(result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<int> Create(CreateWalletCommand wallet)
    {
        var result = await _mediator.Send(wallet);
        return result;
    }

    public async Task Update()
    {

    }

    public async Task Delete()
    {

    }
}
