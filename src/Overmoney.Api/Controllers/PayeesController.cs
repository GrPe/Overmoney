using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Overmoney.Api.Controllers;

public class PayeesController : BaseController
{
    private readonly IMediator _mediator;

    public PayeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task GetById(int id)
    {

    }

    [HttpPost]
    public async Task<ActionResult<object>> Create(object command)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    public async Task<ActionResult<object>> Update(object command)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {

    }
}
