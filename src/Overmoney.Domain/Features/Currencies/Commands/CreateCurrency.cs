using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Exceptions;
using Overmoney.Domain.Features.Currencies.Models;

namespace Overmoney.Domain.Features.Currencies.Commands;

public sealed record CreateCurrencyCommand(string Code, string Name) : IRequest<Currency>;

internal sealed class CreateCurrencyCommandValidator : AbstractValidator<CreateCurrencyCommand>
{
    public CreateCurrencyCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

internal sealed class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, Currency>
{
    private readonly ICurrencyRepository _currencyRepository;

    public CreateCurrencyCommandHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<Currency> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currency = await _currencyRepository.GetAsync(request.Code, cancellationToken);

        if (currency is not null)
        {
            throw new DomainValidationException($"Currency with code {request.Code} already exists.");
        }

        return await _currencyRepository.CreateAsync(new(request.Code, request.Name), cancellationToken);
    }
}
