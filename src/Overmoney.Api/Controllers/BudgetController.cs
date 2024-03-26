using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.Features.Budgets.Commands;
using Overmoney.Api.Features.Budgets.Models;
using Overmoney.Api.Features.Budgets.Queries;

namespace Overmoney.Api.Controllers;

public class BudgetController : BaseController
{
    private readonly IMediator _mediator;

    public BudgetController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType<Budget>(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Budget>> Create(CreateBudgetCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(UpdateBudgetCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteBudgetCommand(id));
        return NoContent();
    }

    [HttpGet("{id}")]
    [ProducesResponseType<Budget>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Budget?>> Get(int id)
    {
        var result = await _mediator.Send(new GetBudgetByIdQuery(id));

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

    public async Task<IActionResult> UpsertLine(object command)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> DeleteLine(int id)
    {
        throw new NotImplementedException();
    }
}
