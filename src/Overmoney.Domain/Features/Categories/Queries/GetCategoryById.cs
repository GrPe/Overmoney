using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Categories.Models;

namespace Overmoney.Domain.Features.Categories.Queries;

public sealed record GetCategoryByIdQuery(CategoryId Id) : IRequest<Category?>;

internal sealed class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .ChildRules(x => { x.RuleFor(x => x.Value).GreaterThan(0); });
    }
}

internal sealed class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Category?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetAsync(request.Id, cancellationToken);
    }
}
