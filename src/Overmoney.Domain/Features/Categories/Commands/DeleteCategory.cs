using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;

namespace Overmoney.Domain.Features.Categories.Commands;

public sealed record DeleteCategoryCommand(int Id) : IRequest;

internal sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

internal sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await _categoryRepository.DeleteAsync(request.Id, cancellationToken);
    }
}