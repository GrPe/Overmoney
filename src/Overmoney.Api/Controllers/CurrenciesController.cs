using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Domain.Features.Currencies.Commands;
using Overmoney.Domain.Features.Currencies.Models;
using Overmoney.Domain.Features.Currencies.Queries;

namespace Overmoney.Api.Controllers;

public class CurrenciesController : BaseController
{
    private readonly IMediator _mediator;

    public CurrenciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieve currency by id
    /// </summary>
    /// <param name="id">Id of the currency</param>
    /// <returns>Currency entity</returns>
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

    /// <summary>
    /// Retrieve all available currencies
    /// </summary>
    /// <returns>List of currencies</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<Currency>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<Currency>> GetAll()
    {
        var response = await _mediator.Send(new GetAllCurrenciesQuery());

        return Ok(response);
    }

    /// <summary>
    /// Create new currency
    /// </summary>
    /// <param name="currency"></param>
    /// <returns>Created currency</returns>
    [HttpPost]
    [ProducesResponseType<Currency>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Currency>> Create(CreateCurrencyCommand currency)
    {
        var response = await _mediator.Send(currency);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Update currency or create new one
    /// </summary>
    /// <param name="currency"></param>
    /// <returns>Created currency</returns>
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
