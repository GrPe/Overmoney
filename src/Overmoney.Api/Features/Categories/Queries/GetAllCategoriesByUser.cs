using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Categories;

namespace Overmoney.Api.Features.Categories.Queries;

public sealed record GetAllCategoriesByUserQuery(int UserId) : IRequest<IEnumerable<CategoryEntity>>;

public sealed class GetAllCategoriesByUserQueryValidator : AbstractValidator<GetAllCategoriesByUserQuery>
{
    public GetAllCategoriesByUserQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}

public sealed class GetAllCategoriesByUserQueryHandler : IRequestHandler<GetAllCategoriesByUserQuery, IEnumerable<CategoryEntity>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesByUserQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryEntity>> Handle(GetAllCategoriesByUserQuery request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetAllByUserAsync(request.UserId, cancellationToken);
    }
}
