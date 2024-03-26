using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Payees;
using Overmoney.Api.Features.Payees.Models;

namespace Overmoney.Api.Features.Payees.Commands;

public sealed record UpdatePayeeCommand(int Id, int UserId, string Name) : IRequest<Payee?>;

public sealed class UpdatePayeeCommandValidator : AbstractValidator<UpdatePayeeCommand>
{
    public UpdatePayeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

public sealed class UpdatePayeeCommandHandler : IRequestHandler<UpdatePayeeCommand, Payee?>
{
    private readonly IPayeeRepository _payeeRepository;

    public UpdatePayeeCommandHandler(IPayeeRepository payeeRepository)
    {
        _payeeRepository = payeeRepository;
    }

    public async Task<Payee?> Handle(UpdatePayeeCommand request, CancellationToken cancellationToken)
    {
        var payee = await _payeeRepository.GetAsync(new PayeeId(request.Id), cancellationToken);

        if(payee == null)
        {
            return await _payeeRepository.CreateAsync(new Payee(request.UserId, request.Name), cancellationToken);
        }

        await _payeeRepository.UpdateAsync(new Payee(new PayeeId(request.Id), request.UserId, request.Name), cancellationToken);
        return null;
    }
}