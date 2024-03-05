using FluentValidation;
using Overmoney.Api.DataAccess;
using Overmoney.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
