using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Overmoney.Api.DataAccess;
using Overmoney.Api.Features;
using Overmoney.Api.Infrastructure;
using Overmoney.Api.Infrastructure.Converters;
using Overmoney.Api.Infrastructure.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new ScheduleJsonConverter());
    options.SerializerOptions.Converters.Add(new IntIdentityJsonConverter());
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

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddMediatR(
    cfg => {
        cfg.RegisterServicesFromAssemblyContaining<Program>();
        cfg.AddOpenRequestPreProcessor(typeof(RequestValidationBehavior<>));
    });
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.Scan(
    s => s.FromAssemblyOf<Program>()
    .AddClasses(classes => classes.AssignableTo<IRepository>())
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddScoped<ExceptionHandler>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandler>();

app.MapControllers();

app.Run();
