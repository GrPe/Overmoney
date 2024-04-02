using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Transactions.Models;

namespace Overmoney.Domain.Features.Transactions.Commands;

public sealed record DeleteAttachmentCommand(AttachmentId Id) : IRequest;

internal sealed class DeleteAttachmentCommandValidator : AbstractValidator<DeleteAttachmentCommand>
{
    public DeleteAttachmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
    }
}

internal sealed class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand>
{
    private readonly ITransactionRepository _transactionRepository;

    public DeleteAttachmentCommandHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
    {
        await _transactionRepository.DeleteAttachmentAsync(request.Id, cancellationToken);
    }
}
