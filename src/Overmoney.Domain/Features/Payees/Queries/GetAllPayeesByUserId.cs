using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Payees.Models;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.Features.Payees.Queries;

public sealed record GetAllPayeesByUserIdQuery(UserId UserId) : IRequest<IEnumerable<Payee>>;

internal sealed class GetAllPayeesByUserIdQueryValidator : AbstractValidator<GetAllPayeesByUserIdQuery>
{
    public GetAllPayeesByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
    }
}

internal sealed class GetAllPayeesByUserIdQueryHandler : IRequestHandler<GetAllPayeesByUserIdQuery, IEnumerable<Payee>>
{
    private readonly IPayeeRepository _payeeRepository;

    public GetAllPayeesByUserIdQueryHandler(IPayeeRepository paymentRepository)
    {
        _payeeRepository = paymentRepository;
    }

    public async Task<IEnumerable<Payee>> Handle(GetAllPayeesByUserIdQuery request, CancellationToken cancellationToken)
    {
        return await _payeeRepository.GetAllByUserIdAsync(request.UserId, cancellationToken);
    }
}
