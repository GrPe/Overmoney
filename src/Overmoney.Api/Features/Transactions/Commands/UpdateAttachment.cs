using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Overmoney.Api.DataAccess;

namespace Overmoney.Api.Features.Transactions.Commands;

public sealed record UpdateAttachmentCommand(long Id, string Name) : IRequest;

public sealed class UpdateAttachmentCommandValidator : AbstractValidator<UpdateAttachmentCommand>
{
    public UpdateAttachmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

internal sealed class UpdateAttachmentCommandHandler : IRequestHandler<UpdateAttachmentCommand>
{
    private readonly DatabaseContext _databaseContext;

    public UpdateAttachmentCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task Handle(UpdateAttachmentCommand request, CancellationToken cancellationToken)
    {
        var attachment = await _databaseContext.Attachments.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if(attachment == null)
        {
            return;
        }

        attachment.Update(request.Name);
        _databaseContext.Update(attachment);

        await _databaseContext.SaveChangesAsync(cancellationToken);
    }
}