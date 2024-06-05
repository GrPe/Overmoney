using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Exceptions;
using Overmoney.Domain.Features.Transactions.Models;

namespace Overmoney.Domain.Features.Transactions.Commands;

public sealed record AddAttachmentCommand(TransactionId TransactionId, string Name, string Path) : IRequest<Attachment>;

internal sealed class AddAttachmentCommandValidator : AbstractValidator<AddAttachmentCommand>
{
    public AddAttachmentCommandValidator()
    {
        RuleFor(x => x.TransactionId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.Name)
            .NotEmpty();
        RuleFor(x => x.Path)
            .NotEmpty();
    }
}

internal sealed class AddAttachmentCommandHandler : IRequestHandler<AddAttachmentCommand, Attachment>
{
    private readonly ITransactionRepository _transactionRepository;

    public AddAttachmentCommandHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Attachment> Handle(AddAttachmentCommand request, CancellationToken cancellationToken)
    {
        var exists = await _transactionRepository.IsExists(request.TransactionId, cancellationToken);

        if (!exists)
        {
            throw new DomainValidationException($"Transaction with id {request.TransactionId} doesn't exists.");
        }

        return await _transactionRepository.AddAttachmentAsync(request.TransactionId, new Attachment(request.Name, request.Path), cancellationToken);
    }
}