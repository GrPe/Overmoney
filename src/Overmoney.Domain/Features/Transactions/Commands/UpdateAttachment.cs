using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Transactions.Models;

namespace Overmoney.Domain.Features.Transactions.Commands;

public sealed record UpdateAttachmentCommand(AttachmentId Id, string Name) : IRequest;

internal sealed class UpdateAttachmentCommandValidator : AbstractValidator<UpdateAttachmentCommand>
{
    public UpdateAttachmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

internal sealed class UpdateAttachmentCommandHandler : IRequestHandler<UpdateAttachmentCommand>
{
    private readonly ITransactionRepository _transactionRepository;

    public UpdateAttachmentCommandHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task Handle(UpdateAttachmentCommand request, CancellationToken cancellationToken)
    {
        var attachment = await _transactionRepository.GetAttachmentAsync(request.Id, cancellationToken);

        if (attachment == null)
        {
            return;
        }

        attachment.Update(request.Name);

        await _transactionRepository.UpdateAttachmentAsync(attachment, cancellationToken);
    }
}