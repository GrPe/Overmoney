using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Currencies;
using Overmoney.Api.Infrastructure.Exceptions;

namespace Overmoney.Api.Features.Currencies.Commands;

public sealed record UpdateCurrencyCommand(int Id, string Code, string Name) : IRequest<CurrencyEntity?>;

public sealed class UpdateCurrencyCommandValidator : AbstractValidator<UpdateCurrencyCommand>
{
    public UpdateCurrencyCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
        RuleFor(x => x.Code)
            .NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

public sealed class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, CurrencyEntity?>
{
    private readonly ICurrencyRepository _currencyRepository;

    public UpdateCurrencyCommandHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<CurrencyEntity?> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currency = await _currencyRepository.GetAsync(request.Id, cancellationToken);

        if(currency is not null & currency?.Code == request.Code)
        {
            await _currencyRepository.UpdateAsync(new CurrencyEntity(request.Id, request.Code, request.Name), cancellationToken);
            return null;
        }

        currency = await _currencyRepository.GetAsync(request.Code, cancellationToken);

        if(currency is not null)
        {
            throw new DomainValidationException($"Cannot change currency code because the currency with the same code already exists.");
        }

        return await _currencyRepository.CreateAsync(new(request.Code, request.Name), cancellationToken);
    }
}
