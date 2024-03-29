using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Transactions.Models;

namespace Overmoney.Domain.Features.Transactions.Queries;

public sealed record GetAttachmentByIdQuery(long Id) : IRequest<Attachment?>;

internal sealed class GetAttachmentByIdQueryValidator : AbstractValidator<GetAttachmentByIdQuery>
{
    public GetAttachmentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

internal sealed class GetAttachmentByIdQueryHandler : IRequestHandler<GetAttachmentByIdQuery, Attachment?>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetAttachmentByIdQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Attachment?> Handle(GetAttachmentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _transactionRepository.GetAttachmentAsync(request.Id, cancellationToken);
    }
}
