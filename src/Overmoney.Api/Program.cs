using FluentValidation;
using Overmoney.Api.Infrastructure;
using Overmoney.Api.Infrastructure.Filters;
using Overmoney.Domain;
using Overmoney.Domain.DataAccess;
using Overmoney.DataAccess;
using System.Reflection;
using Overmoney.Domain.Converters;

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

builder.Services.AddDataAccess(builder.Configuration.GetConnectionString("Database"));
builder.Services.AddDomain();

builder.Services.AddScoped<ExceptionHandler>();

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
