using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.Features.Transactions.Commands;
using Overmoney.Api.Features.Transactions.Models;
using Overmoney.Api.Features.Transactions.Queries;

namespace Overmoney.Api.Controllers;

public class AttachmentsController : BaseController
{
    private readonly IMediator _mediator;

    public AttachmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType<Attachment>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Attachment>> Get(long id)
    {
        var result = await _mediator.Send(new GetAttachmentByIdQuery(id));
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteAttachmentCommand(id));
        return Accepted();
    }

    //[HttpPatch]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesDefaultResponseType]
    //public async Task<ActionResult> UpdateAttachment(UpdateAttachmentCommand command)
    //{
    //    await _mediator.Send(command);
    //    return NoContent();
    //}
}
