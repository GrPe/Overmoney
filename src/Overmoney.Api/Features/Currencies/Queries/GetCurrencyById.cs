using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Currencies;
using Overmoney.Api.Features.Currencies.Models;

namespace Overmoney.Api.Features.Currencies.Queries;

public sealed record GetCurrencyByIdQuery(int Id) : IRequest<Currency?>;

public sealed class GetCurrencyByIdQueryValidator : AbstractValidator<GetCurrencyByIdQuery>
{
    public GetCurrencyByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class GetCurrencyByIdQueryHandler : IRequestHandler<GetCurrencyByIdQuery, Currency?>
{
    private readonly ICurrencyRepository _currencyRepository;

    public GetCurrencyByIdQueryHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<Currency?> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
    {
        return await _currencyRepository.GetAsync(request.Id, cancellationToken);
    }
}
