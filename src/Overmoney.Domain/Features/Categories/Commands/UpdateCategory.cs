using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Categories.Models;
using Overmoney.Domain.Features.Users.Models;

namespace Overmoney.Domain.Features.Categories.Commands;

public sealed record UpdateCategoryCommand(CategoryId Id, UserProfileId UserId, string Name) : IRequest<Category?>;

internal sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.UserId)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

internal sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Category?>
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

        await _categoryRepository.UpdateAsync(new(request.Id, request.UserId, request.Name), cancellationToken);
        return null;
    }
}
