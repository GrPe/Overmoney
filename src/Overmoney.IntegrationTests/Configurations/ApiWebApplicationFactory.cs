using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Overmoney.DataAccess;

namespace Overmoney.IntegrationTests.Configurations;
internal class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly int _posgresPort;

    public ApiWebApplicationFactory(int posgresPort)
    {
        _posgresPort = posgresPort;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Remove(services.FirstOrDefault(x => x.ServiceType.Name.Contains("DatabaseContext")));
            services.Remove(services.FirstOrDefault(x => x.ServiceType.Name.Contains("ContextOptions")));
            services.AddDataAccess($"Host=localhost:{_posgresPort};Database=overmoney;Username=dev;Password=dev;Timeout=300;CommandTimeout=300", applyMigrations: true);
        });
    }
}
