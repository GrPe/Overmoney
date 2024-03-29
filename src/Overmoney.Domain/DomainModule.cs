using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Overmoney.Domain.Features.Users.Commands;

namespace Overmoney.Domain;
public static class DomainModule
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddMediatR(
            cfg => {
                cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
                cfg.AddOpenRequestPreProcessor(typeof(RequestValidationBehavior<>));
            });
        services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>(includeInternalTypes: true);

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
