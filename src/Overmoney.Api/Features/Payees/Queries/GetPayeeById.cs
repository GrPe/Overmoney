using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Payees;

namespace Overmoney.Api.Features.Payees.Queries;

public sealed record GetPayeeByIdQuery(int Id) : IRequest<PayeeEntity?>;

public sealed class GetPayeeByIdQueryValidator : AbstractValidator<GetPayeeByIdQuery>
{
    public GetPayeeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class GetPayeeByIdQueryHandler : IRequestHandler<GetPayeeByIdQuery, PayeeEntity?>
{
    private readonly IPayeeRepository _payeeRepository;

    public GetPayeeByIdQueryHandler(IPayeeRepository payeeRepository)
    {
        _payeeRepository = payeeRepository;
    }

    public async Task<PayeeEntity?> Handle(GetPayeeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _payeeRepository.GetAsync(request.Id, cancellationToken);
    }
}