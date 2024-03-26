using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Payees;
using Overmoney.Api.Features.Payees.Models;

namespace Overmoney.Api.Features.Payees.Queries;

public sealed record GetPayeeByIdQuery(int Id) : IRequest<Payee?>;

public sealed class GetPayeeByIdQueryValidator : AbstractValidator<GetPayeeByIdQuery>
{
    public GetPayeeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class GetPayeeByIdQueryHandler : IRequestHandler<GetPayeeByIdQuery, Payee?>
{
    private readonly IPayeeRepository _payeeRepository;

    public GetPayeeByIdQueryHandler(IPayeeRepository payeeRepository)
    {
        _payeeRepository = payeeRepository;
    }

    public async Task<Payee?> Handle(GetPayeeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _payeeRepository.GetAsync(new PayeeId(request.Id), cancellationToken);
    }
}