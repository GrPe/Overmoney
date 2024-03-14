using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Currencies;

namespace Overmoney.Api.Features.Currencies.Queries;

public sealed record GetAllCurrenciesQuery : IRequest<IEnumerable<CurrencyEntity>>;

public sealed class GetAllCurrenciesQueryValidator : AbstractValidator<GetAllCurrenciesQuery>
{
    public GetAllCurrenciesQueryValidator() { }
}

public sealed class GetAllCurrenciesQueryHandler : IRequestHandler<GetAllCurrenciesQuery, IEnumerable<CurrencyEntity>>
{
    private readonly ICurrencyRepository _currencyRepository;

    public GetAllCurrenciesQueryHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<IEnumerable<CurrencyEntity>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        return await _currencyRepository.GetAllAsync(cancellationToken);
    }
}