using FluentValidation;
using MediatR;
using Overmoney.Api.Features.Currencies;
using Overmoney.Api.Features.Currencies.Models;

namespace Overmoney.Api.Features.Currencies.Queries;

public sealed record GetAllCurrenciesQuery : IRequest<IEnumerable<Currency>>;

public sealed class GetAllCurrenciesQueryValidator : AbstractValidator<GetAllCurrenciesQuery>
{
    public GetAllCurrenciesQueryValidator() { }
}

public sealed class GetAllCurrenciesQueryHandler : IRequestHandler<GetAllCurrenciesQuery, IEnumerable<Currency>>
{
    private readonly ICurrencyRepository _currencyRepository;

    public GetAllCurrenciesQueryHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<IEnumerable<Currency>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        return await _currencyRepository.GetAllAsync(cancellationToken);
    }
}