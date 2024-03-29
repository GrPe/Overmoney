using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Payees.Models;

namespace Overmoney.Domain.Features.Payees.Queries;

public sealed record GetPayeeByIdQuery(PayeeId Id) : IRequest<Payee?>;

internal sealed class GetPayeeByIdQueryValidator : AbstractValidator<GetPayeeByIdQuery>
{
    public GetPayeeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

internal sealed class GetPayeeByIdQueryHandler : IRequestHandler<GetPayeeByIdQuery, Payee?>
{
    private readonly IPayeeRepository _payeeRepository;

    public GetPayeeByIdQueryHandler(IPayeeRepository payeeRepository)
    {
        _payeeRepository = payeeRepository;
    }

    public async Task<Payee?> Handle(GetPayeeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _payeeRepository.GetAsync(request.Id, cancellationToken);
    }
}