using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Domain.Features.Transactions.Commands;
using Overmoney.Domain.Features.Transactions.Models;
using Overmoney.Domain.Features.Transactions.Queries;

namespace Overmoney.Api.Controllers;

public class TransactionsController : BaseController
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieve transaction by Id
    /// </summary>
    /// <param name="id">Transaction Id</param>
    /// <returns>Transaction entity</returns>
    [HttpGet("{id}")]
    [ProducesResponseType<Transaction>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Transaction>> GetById(long id)
    {
        var result = await _mediator.Send(new GetTransactionByIdQuery(new(id)));

        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    /// <summary>
    /// Create new transaction
    /// </summary>
    /// <param name="command"></param>
    /// <returns>Created transaction</returns>
    [HttpPost]
    [ProducesResponseType<Transaction>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Transaction>> Create(CreateTransactionCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { result.Id }, result);
    }

    /// <summary>
    /// Update transaction or create new one
    /// </summary>
    /// <param name="command"></param>
    /// <returns>Created transaction</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Transaction>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Transaction>> Update(UpdateTransactionCommand command)
    {
        var result = await _mediator.Send(command);

        if (result is null)
        {
            return Ok();
        }
        return CreatedAtAction(nameof(GetById), new { result.Id }, result);
    }

    /// <summary>
    /// Delete transaction
    /// </summary>
    /// <param name="id">Transaction Id</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteTransactionCommand(new(id)));
        return NoContent();
    }

    /// <summary>
    /// Add attachment to transaction
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("attachments")]
    [ProducesResponseType<Attachment>(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Attachment>> AddAttachment(AddAttachmentCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Create new recurring transaction
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("recurring")]
    [ProducesResponseType<RecurringTransaction>(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<RecurringTransaction>> CreateRecurringTransaction(CreateRecurringTransactionCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetRecurringById), new { result.Id }, result);
    }

    /// <summary>
    /// Update recurring transaction next occurrence based on schedule
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("recurring/{id}/schedule")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateRecurringTransaction(long id)
    {
        await _mediator.Send(new UpdateRecurringTransactionNextOccurrenceCommand(new(id)));
        return NoContent();
    }

    /// <summary>
    /// Retrieve recurring transaction by Id
    /// </summary>
    /// <param name="id">Recurring Transaction Id</param>
    /// <returns></returns>
    [HttpGet("recurring/{id}")]
    [ProducesResponseType<RecurringTransaction>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<RecurringTransaction>> GetRecurringById(long id)
    {
        var result = await _mediator.Send(new GetRecurringTransactionQuery(new(id)));
        
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    /// <summary>
    /// Update recurring transaction or create new one
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("recurring")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<RecurringTransaction>(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateRecurring(UpdateRecurringTransactionCommand command)
    {
        var result = await _mediator.Send(command);

        if (result is null)
        {
            return Ok();
        }

        return CreatedAtAction(nameof(GetRecurringById), new { result.Id }, result);
    }

    /// <summary>
    /// Delete recurring transaction
    /// </summary>
    /// <param name="id">Recurring Transaction Id</param>
    /// <returns></returns>
    [HttpDelete("recurring/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> DeleteRecurring(long id)
    {
        await _mediator.Send(new DeleteRecurringTransactionCommand(new(id)));
        return NoContent();
    }
}
