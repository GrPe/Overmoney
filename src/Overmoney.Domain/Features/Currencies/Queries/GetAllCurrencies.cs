using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Currencies.Models;

namespace Overmoney.Domain.Features.Currencies.Queries;

public sealed record GetAllCurrenciesQuery : IRequest<IEnumerable<Currency>>;

internal sealed class GetAllCurrenciesQueryValidator : AbstractValidator<GetAllCurrenciesQuery>
{
    public GetAllCurrenciesQueryValidator() { }
}

internal sealed class GetAllCurrenciesQueryHandler : IRequestHandler<GetAllCurrenciesQuery, IEnumerable<Currency>>
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