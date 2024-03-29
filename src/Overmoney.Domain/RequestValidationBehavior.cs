using FluentValidation;
using MediatR.Pipeline;

namespace Overmoney.Domain;

internal sealed class RequestValidationBehavior<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly IValidator<TRequest> _validator;

    public RequestValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
    }
}
