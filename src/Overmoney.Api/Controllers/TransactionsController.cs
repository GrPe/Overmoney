﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.DataAccess.Transactions;
using Overmoney.Api.Features.Transactions.Commands;
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
    [ProducesResponseType<TransactionEntity>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionEntity>> GetById(int id)
    {
        var result = await _mediator.Send(new GetTransactionByIdQuery(id));

        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<TransactionEntity>(StatusCodes.Status201Created)]
    public async Task<ActionResult<TransactionEntity>> Create(CreateTransactionCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { result.Id }, result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<TransactionEntity>(StatusCodes.Status201Created)]
    public async Task<ActionResult<TransactionEntity>> Update(UpdateTransactionCommand command)
    {
        var result = await _mediator.Send(command);

        if (result is null)
        {
            return Ok();
        }
        return CreatedAtAction(nameof(GetById), new { result.Id }, result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteTransactionCommand(id));
        return NoContent();
    }
}