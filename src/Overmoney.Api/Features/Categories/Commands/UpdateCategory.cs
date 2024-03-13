using FluentValidation;
using MediatR;
using Overmoney.Api.Features.Categories;
using Overmoney.Api.Features.Categories.Models;

namespace Overmoney.Api.Features.Categories.Commands;

public sealed record UpdateCategoryCommand(int Id, int UserId, string Name) : IRequest<Category?>;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Category?>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Category?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetAsync(request.Id, cancellationToken);

        if(category is null)
        {
            return await _categoryRepository.CreateAsync(new(request.UserId, request.Name), cancellationToken);
        }

        await _categoryRepository.UpdateAsync(new(request.Id, request.Name), cancellationToken);
        return null;
    }
}
