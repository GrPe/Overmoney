using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Overmoney.IntegrationTests.Configurations;
public class InfrastructureFixture : IDisposable
{
    readonly IContainer _postgresContainer;
    readonly ApiWebApplicationFactory _application;
    readonly HttpClient _client;

    public InfrastructureFixture()
    {
        _postgresContainer = new ContainerBuilder()
            .WithImage("postgres")
            .WithPortBinding(5432, true)
            .WithEnvironment("POSTGRES_USER", "dev")
            .WithEnvironment("POSTGRES_PASSWORD", "dev")
            .Build();

        _postgresContainer.StartAsync().GetAwaiter().GetResult();
        _application = new ApiWebApplicationFactory(_postgresContainer.GetMappedPublicPort(5432));
        _client = _application.CreateClient();
    }

    public HttpClient GetClient() => _client;

    public void Dispose()
    {
        _client.Dispose();
        _application.Dispose();
        _postgresContainer.StopAsync().GetAwaiter().GetResult();
        _postgresContainer.DisposeAsync().GetAwaiter().GetResult();
    }
}
