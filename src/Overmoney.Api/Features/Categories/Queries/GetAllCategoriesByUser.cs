using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Categories;
using Overmoney.Api.Features.Categories.Models;

namespace Overmoney.Api.Features.Categories.Queries;

public sealed record GetAllCategoriesByUserQuery(int UserId) : IRequest<IEnumerable<Category>>;

public sealed class GetAllCategoriesByUserQueryValidator : AbstractValidator<GetAllCategoriesByUserQuery>
{
    public GetAllCategoriesByUserQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}

public sealed class GetAllCategoriesByUserQueryHandler : IRequestHandler<GetAllCategoriesByUserQuery, IEnumerable<Category>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesByUserQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Category>> Handle(GetAllCategoriesByUserQuery request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetAllByUserAsync(request.UserId, cancellationToken);
    }
}
