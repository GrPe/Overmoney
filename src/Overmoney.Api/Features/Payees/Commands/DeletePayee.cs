using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Payees;

namespace Overmoney.Api.Features.Payees.Commands;

public sealed record DeletePayeeCommand(int Id) : IRequest;

public sealed class DeletePayeeCommandValidator : AbstractValidator<DeletePayeeCommand>
{
    public DeletePayeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class DeletePayeeCommandHandler : IRequestHandler<DeletePayeeCommand>
{
    private readonly IPayeeRepository _payeeRepository;

    public DeletePayeeCommandHandler(IPayeeRepository payeeRepository)
    {
        _payeeRepository = payeeRepository;
    }

    public async Task Handle(DeletePayeeCommand request, CancellationToken cancellationToken)
    {
        await _payeeRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
