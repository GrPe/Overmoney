using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Payees;
using Overmoney.Api.Features.Payees.Models;

namespace Overmoney.Api.Features.Payees.Commands;

public sealed record CreatePayeeCommand(int UserId, string Name) : IRequest<PayeeEntity>;

public sealed class CreatePayeeCommandValidator : AbstractValidator<CreatePayeeCommand>
{
    public CreatePayeeCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

public sealed class CreatePayeeCommandHandler : IRequestHandler<CreatePayeeCommand, PayeeEntity>
{
    private readonly IPayeeRepository _payeeRepository;

    public CreatePayeeCommandHandler(IPayeeRepository payeeRepository)
    {
        _payeeRepository = payeeRepository;
    }

    public async Task<PayeeEntity> Handle(CreatePayeeCommand request, CancellationToken cancellationToken)
    {
        return await _payeeRepository.CreateAsync(new CreatePayee(request.UserId, request.Name), cancellationToken);
    }
}
