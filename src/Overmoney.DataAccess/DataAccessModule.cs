using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Overmoney.Api.DataAccess;
using Overmoney.DataAccess.Identity;
using Overmoney.Domain.DataAccess;
using System.Reflection;

namespace Overmoney.DataAccess;
public static class DataAccessModule
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, string connectionString, bool applyMigrations = false)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(connectionString, x => x.MigrationsAssembly(Assembly.GetAssembly(typeof(DatabaseContext))!.FullName));
            options.UseSnakeCaseNamingConvention();
        });

        if(applyMigrations)
        {
            var context = services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
            context.Database.Migrate();
        }

        services.Scan(
            s => s.FromAssemblyOf<DatabaseContext>()
            .AddClasses(classes => classes.AssignableTo<IRepository>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddDbContext<ApplicationIdentityDbContext>(options =>
        {
            options.UseNpgsql(connectionString, x => x.MigrationsAssembly(Assembly.GetAssembly(typeof(DatabaseContext))!.FullName));
            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }
}
