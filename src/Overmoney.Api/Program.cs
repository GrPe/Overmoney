using Overmoney.Api.Infrastructure;
using Overmoney.Api.Infrastructure.Filters;
using Overmoney.Domain;
using Overmoney.DataAccess;
using System.Reflection;
using Overmoney.Domain.Converters;
using Microsoft.AspNetCore.Identity;
using Overmoney.DataAccess.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new ScheduleJsonConverter());
    options.SerializerOptions.Converters.Add(new IntIdentityJsonConverter());
    options.SerializerOptions.Converters.Add(new LongIdentityJsonConverter());
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

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

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
app.MapIdentityApi<IdentityUser>();

app.UseMiddleware<ExceptionHandler>();

app.MapControllers();

app.Run();

public partial class Program { }