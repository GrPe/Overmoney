using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Domain.Features.Budgets.Commands;
using Overmoney.Domain.Features.Budgets.Models;
using Overmoney.Domain.Features.Budgets.Queries;

namespace Overmoney.Api.Controllers;

public class BudgetController : BaseController
{
    private readonly IMediator _mediator;

    public BudgetController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create new budget (including budget lines)
    /// </summary>
    /// <param name="command"></param>
    /// <returns>Created budget </returns>
    [HttpPost]
    [ProducesResponseType<Budget>(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Budget>> Create(CreateBudgetCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }
    /// <summary>
    /// Update existing budget (including budget lines)
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(UpdateBudgetCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Delete budget
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteBudgetCommand(new(id)));
        return NoContent();
    }

    /// <summary>
    /// Retrieve budget by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Budget entity</returns>
    [HttpGet("{id}")]
    [ProducesResponseType<Budget>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Budget?>> Get(int id)
    {
        var result = await _mediator.Send(new GetBudgetByIdQuery(new(id)));

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    //todo: reporting
    public async Task<ActionResult<object>> GetReport(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Update budget lines or create new ones
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("lines")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpsertLine(UpsertBudgetLineCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Delete budget lines
    /// </summary>
    /// <param name="id"></param>
    /// <param name="BudgetLinesIds"></param>
    /// <returns></returns>
    [HttpDelete("{id}/lines")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> DeleteLine(int id, List<int> BudgetLinesIds)
    {
        await _mediator.Send(new DeleteBudgetLinesCommand(new(id), BudgetLinesIds.Select(x => new BudgetLineId(x))));
        return NoContent();
    }
}
