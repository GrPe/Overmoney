using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Categories.Models;

namespace Overmoney.Domain.Features.Categories.Commands;

public sealed record CreateCategoryCommand(int UserId, string Name) : IRequest<Category>;

internal sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

internal sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category(request.UserId, request.Name);
        return await _categoryRepository.CreateAsync(category, cancellationToken);
    }
}