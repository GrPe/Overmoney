using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.Features.Transactions.Commands;
using Overmoney.Api.Features.Transactions.Models;
using Overmoney.Api.Features.Transactions.Queries;

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
        var result = await _mediator.Send(new GetTransactionByIdQuery(id));

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
        await _mediator.Send(new DeleteTransactionCommand(id));
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
}
