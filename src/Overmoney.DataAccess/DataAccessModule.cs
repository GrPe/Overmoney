using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Overmoney.Api.DataAccess;
using Overmoney.Domain.DataAccess;

namespace Overmoney.DataAccess;
public static class DataAccessModule
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseSnakeCaseNamingConvention();
        });

        services.Scan(
            s => s.FromAssemblyOf<DatabaseContext>()
            .AddClasses(classes => classes.AssignableTo<IRepository>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
