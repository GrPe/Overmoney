using MediatR;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Api.Features.Users;

namespace Overmoney.Api.Controllers;

public class UsersController : BaseController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<int> Register(RegisterUserCommand command)
    {
        var response = await _mediator.Send(command);

        return response;
    }
}
