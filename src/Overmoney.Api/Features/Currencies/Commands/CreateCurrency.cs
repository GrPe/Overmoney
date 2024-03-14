using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Currencies;
using Overmoney.Api.Features.Currencies.Models;
using Overmoney.Api.Infrastructure.Exceptions;

namespace Overmoney.Api.Features.Currencies.Commands;

public sealed record CreateCurrencyCommand(string Code, string Name) : IRequest<Currency>;

public sealed class CreateCurrencyCommandValidator : AbstractValidator<CreateCurrencyCommand>
{
    public CreateCurrencyCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

public sealed class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, Currency>
{
    private readonly ICurrencyRepository _currencyRepository;

    public CreateCurrencyCommandHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<Currency> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currency = await _currencyRepository.GetAsync(request.Code, cancellationToken);

        if(currency is not null)
        {
            throw new DomainValidationException($"Currency with code {request.Code} already exists.");
        }

        return await _currencyRepository.CreateAsync(new(request.Code, request.Name), cancellationToken);
    }
}
