using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.DataAccess.Transactions.Models;
using Overmoney.Api.Features.Transactions.Queries;

namespace Overmoney.Api.Controllers;

public class TransactionsController : BaseController
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType<Transaction>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Transaction>> GetById(int id)
    {
        var result = await _mediator.Send(new GetTransactionByIdQuery(id));

        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<Transaction>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Transaction>> Create()
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Transaction>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Transaction>> Update()
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete()
    {
        throw new NotImplementedException();
    }

}
