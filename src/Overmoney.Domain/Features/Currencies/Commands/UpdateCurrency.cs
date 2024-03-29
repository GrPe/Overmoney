using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Exceptions;
using Overmoney.Domain.Features.Currencies.Models;

namespace Overmoney.Domain.Features.Currencies.Commands;

public sealed record UpdateCurrencyCommand(int Id, string Code, string Name) : IRequest<Currency?>;

internal sealed class UpdateCurrencyCommandValidator : AbstractValidator<UpdateCurrencyCommand>
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

internal sealed class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, Currency?>
{
    private readonly ICurrencyRepository _currencyRepository;

    public UpdateCurrencyCommandHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<Currency?> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currency = await _currencyRepository.GetAsync(request.Id, cancellationToken);

        if(currency is not null & currency?.Code == request.Code)
        {
            await _currencyRepository.UpdateAsync(new Currency(request.Id, request.Code, request.Name), cancellationToken);
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
