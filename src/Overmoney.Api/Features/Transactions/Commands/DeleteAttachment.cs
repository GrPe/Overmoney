using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Transactions;

namespace Overmoney.Api.Features.Transactions.Commands;

public sealed record DeleteAttachmentCommand(long Id) : IRequest;

public sealed class DeleteAttachmentCommandValidator : AbstractValidator<DeleteAttachmentCommand>
{
    public DeleteAttachmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand>
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
