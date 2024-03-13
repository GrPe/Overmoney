using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.Features.Currencies.Models;
using Overmoney.Api.Features.Currencies.Commands;
using Overmoney.Api.Features.Currencies.Queries;

namespace Overmoney.Api.Controllers;

public class CurrenciesController : BaseController
{
    private readonly IMediator _mediator;

    public CurrenciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType<Currency>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Currency>> GetById(int id)
    {
        var response = await _mediator.Send(new GetCurrencyByIdQuery(id));

        if(response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<Currency>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<Currency>> GetAll()
    {
        var response = await _mediator.Send(new GetAllCurrenciesQuery());

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType<Currency>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Currency>> Create(CreateCurrencyCommand currency)
    {
        var response = await _mediator.Send(currency);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Currency>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Currency>> Update(UpdateCurrencyCommand currency)
    {
        var response = await _mediator.Send(currency);

        if(response is null)
        {
            return Ok();
        }

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
}
