using Overmoney.Api.Infrastructure;
using Overmoney.Api.Infrastructure.Filters;
using Overmoney.Domain;
using Overmoney.DataAccess;
using System.Reflection;
using Overmoney.Domain.Converters;
using Microsoft.AspNetCore.Identity;
using Overmoney.DataAccess.Identity;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using OpenTelemetry.Metrics;
using Microsoft.OpenApi.Any;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new ScheduleJsonConverter());
    options.SerializerOptions.Converters.Add(new IntIdentityJsonConverter());
    options.SerializerOptions.Converters.Add(new LongIdentityJsonConverter());
});

builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.GrafanaLoki("http://loki:3100",
        [
            new() {
                Key = "app",
                Value = "overmoney_api"
            }
        ]);
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Overmoney Api"
    });

    options.SchemaFilter<IdentitySchemaFilter>();

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Bearer token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Id = "Bearer",
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddDataAccess(builder.Configuration.GetConnectionString("Database"), builder.Configuration.GetValue<bool>("AutoMigrate"));
builder.Services.AddDomain();

builder.Services.AddScoped<ExceptionHandler>();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationIdentityDbContext>();

builder.Services.AddOpenTelemetry()
    .WithMetrics(builder =>
    {
        builder.AddPrometheusExporter();
        builder.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel");
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(options =>
{
    //for development purposes
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});

app.UseAuthorization();
app.MapGroup("Identity")
    .MapIdentityApi<IdentityUser>()
    .WithTags("Identity");

app.UseMiddleware<ExceptionHandler>();

app.MapPrometheusScrapingEndpoint();

app.MapControllers();

app.UseHealthChecks("/health");

app.Run();

public partial class Program { }