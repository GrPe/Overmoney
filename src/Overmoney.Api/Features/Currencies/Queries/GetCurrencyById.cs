using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Currencies;

namespace Overmoney.Api.Features.Currencies.Queries;

public sealed record GetCurrencyByIdQuery(int Id) : IRequest<CurrencyEntity?>;

public sealed class GetCurrencyByIdQueryValidator : AbstractValidator<GetCurrencyByIdQuery>
{
    public GetCurrencyByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class GetCurrencyByIdQueryHandler : IRequestHandler<GetCurrencyByIdQuery, CurrencyEntity?>
{
    private readonly ICurrencyRepository _currencyRepository;

    public GetCurrencyByIdQueryHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<CurrencyEntity?> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
    {
        return await _currencyRepository.GetAsync(request.Id, cancellationToken);
    }
}
