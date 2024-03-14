﻿using FluentValidation;
using MediatR;
using Overmoney.Api.DataAccess.Categories;

namespace Overmoney.Api.Features.Categories.Commands;

public sealed record CreateCategoryCommand(int UserId, string Name) : IRequest<CategoryEntity>;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryEntity>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryEntity> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.CreateAsync(new(request.UserId, request.Name), cancellationToken);
    }
}