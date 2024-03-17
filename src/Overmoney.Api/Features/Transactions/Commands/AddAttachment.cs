using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Transactions;
using Overmoney.Api.Features.Transactions.Models;
using Overmoney.Api.Infrastructure.Exceptions;

namespace Overmoney.Api.Features.Transactions.Commands;

public sealed record AddAttachmentCommand(long TransactionId, string Name, string Path) : IRequest<Attachment>;

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

public sealed class AddAttachmentCommandHandler : IRequestHandler<AddAttachmentCommand, Attachment>
{
    private readonly ITransactionRepository _transactionRepository;

    public AddAttachmentCommandHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Attachment> Handle(AddAttachmentCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetAsync(request.TransactionId, cancellationToken);

        if (transaction == null)
        {
            throw new DomainValidationException($"Transaction with id {request.TransactionId} doesn't exists.");
        }

        transaction.AddAttachment(new Attachment(request.Name, request.Path));

        transaction = await _transactionRepository.UpdateAsync(transaction, cancellationToken);
        return transaction.Attachments.Last();
    }
}