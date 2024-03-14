using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.Features.Currencies.Commands;
using Overmoney.Api.Features.Currencies.Queries;
using Overmoney.Api.DataAccess.Currencies;

namespace Overmoney.Api.Controllers;

public class CurrenciesController : BaseController
{
    private readonly IMediator _mediator;

    public CurrenciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType<CurrencyEntity>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CurrencyEntity>> GetById(int id)
    {
        var response = await _mediator.Send(new GetCurrencyByIdQuery(id));

        if(response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<CurrencyEntity>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<CurrencyEntity>> GetAll()
    {
        var response = await _mediator.Send(new GetAllCurrenciesQuery());

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType<CurrencyEntity>(StatusCodes.Status201Created)]
    public async Task<ActionResult<CurrencyEntity>> Create(CreateCurrencyCommand currency)
    {
        var response = await _mediator.Send(currency);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<CurrencyEntity>(StatusCodes.Status201Created)]
    public async Task<ActionResult<CurrencyEntity>> Update(UpdateCurrencyCommand currency)
    {
        var response = await _mediator.Send(currency);

        if(response is null)
        {
            return Ok();
        }

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
}
