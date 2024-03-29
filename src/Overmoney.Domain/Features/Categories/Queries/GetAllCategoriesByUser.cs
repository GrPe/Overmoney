using FluentValidation;
using MediatR;
using Overmoney.Domain.DataAccess;
using Overmoney.Domain.Features.Categories.Models;

namespace Overmoney.Domain.Features.Categories.Queries;

public sealed record GetAllCategoriesByUserQuery(int UserId) : IRequest<IEnumerable<Category>>;

internal sealed class GetAllCategoriesByUserQueryValidator : AbstractValidator<GetAllCategoriesByUserQuery>
{
    public GetAllCategoriesByUserQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}

internal sealed class GetAllCategoriesByUserQueryHandler : IRequestHandler<GetAllCategoriesByUserQuery, IEnumerable<Category>>
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
