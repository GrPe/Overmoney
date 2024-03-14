using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Categories;

namespace Overmoney.Api.Features.Categories.Queries;

public sealed record GetCategoryByIdQuery(int Id) : IRequest<CategoryEntity?>;

public sealed class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryEntity?>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryEntity?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetAsync(request.Id, cancellationToken);
    }
}
