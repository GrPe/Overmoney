using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Domain.Features.Categories.Commands;
using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Categories.Queries;

namespace Overmoney.Api.Controllers;

[Authorize]
public class CategoriesController : BaseController
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieve Category by Id
    /// </summary>
    /// <param name="id">Category Id</param>
    /// <returns>Category entity</returns>
    [HttpGet("{id}")]
    [ProducesResponseType<Category>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(new(id)));

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Create new category
    /// </summary>
    /// <param name="command"></param>
    /// <returns>Created category</returns>
    [HttpPost]
    [ProducesResponseType<Category>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Category>> Create(CreateCategoryCommand command)
    {
        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update existing category or create new one
    /// </summary>
    /// <param name="command"></param>
    /// <returns>Created category</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Category>(StatusCodes.Status201Created)]
    public async Task<ActionResult<Category>> Update(UpdateCategoryCommand command)
    {
        var result = await _mediator.Send(command);

        if (result is null)
        {
            return Ok();
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Delete category
    /// </summary>
    /// <param name="id">Category Id</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand(new(id)));
        return NoContent();
    }
}
