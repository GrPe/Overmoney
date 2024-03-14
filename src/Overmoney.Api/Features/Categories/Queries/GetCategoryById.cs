using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Categories;
using Overmoney.Api.Features.Categories.Models;

namespace Overmoney.Api.Features.Categories.Queries;

public sealed record GetCategoryByIdQuery(int Id) : IRequest<Category?>;

public sealed class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}

public sealed class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
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
