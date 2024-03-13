using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.Features.Payees.Models;
using Overmoney.Api.Features.Payees.Commands;
using Overmoney.Api.Features.Payees.Queries;

namespace Overmoney.Api.Controllers;

public class PayeesController : BaseController
{
    private readonly IMediator _mediator;

    public PayeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType<Payee>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Payee>> GetById(int id)
    {
        var result = await _mediator.Send(new GetPayeeByIdQuery(id));

        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<Payee>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Payee>> Create(CreatePayeeCommand command)
    {
        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { result.Id }, result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Payee>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Payee>> Update(UpdatePayeeCommand command)
    {
        var result = await _mediator.Send(command);

        if(result is not null)
        {
            return CreatedAtAction(nameof(GetById), new {result.Id}, result);
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeletePayeeCommand(id));
        return NoContent();
    }
}
