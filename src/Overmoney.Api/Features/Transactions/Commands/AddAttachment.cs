using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Transactions;
using Overmoney.Api.Features.Transactions.Models;
using Overmoney.Api.Infrastructure.Exceptions;

namespace Overmoney.Api.Features.Transactions.Commands;

public sealed record AddAttachmentCommand(long TransactionId, string Name, string Path) : IRequest;

public sealed class AddAttachmentCommandValidator : AbstractValidator<AddAttachmentCommand>
{
    public AddAttachmentCommandValidator()
    {
        RuleFor(x => x.TransactionId)
            .GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty();
        RuleFor(x => x.Path)
            .NotEmpty();
    }
}

public sealed class AddAttachmentCommandHandler : IRequestHandler<AddAttachmentCommand>
{
    private readonly ITransactionRepository _transactionRepository;

    public AddAttachmentCommandHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task Handle(AddAttachmentCommand request, CancellationToken cancellationToken)
    {
        var exists = await _transactionRepository.IsExists(request.TransactionId, cancellationToken);

        if (!exists)
        {
            throw new DomainValidationException($"Transaction with id {request.TransactionId} doesn't exists.");
        }

        await _transactionRepository.AddAttachmentAsync(request.TransactionId, new Attachment(request.Name, request.Path), cancellationToken);
    }
}