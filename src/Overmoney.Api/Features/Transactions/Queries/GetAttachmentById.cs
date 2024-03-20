using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Transactions;
using Overmoney.Api.Features.Transactions.Models;

namespace Overmoney.Api.Features.Transactions.Queries;

public sealed record GetAttachmentByIdQuery(long Id) : IRequest<Attachment?>;

public sealed class GetAttachmentByIdQueryValidator : AbstractValidator<GetAttachmentByIdQuery>
{
    public GetAttachmentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class GetAttachmentByIdQueryHandler : IRequestHandler<GetAttachmentByIdQuery, Attachment?>
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
