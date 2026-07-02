using Net10.docker.k8s.Services;
using Net10.docker.k8s.Services.Impl;
using Net10.docker.k8s.Repositories;
using Net10.docker.k8s.Repositories.Impl;
using Net10.docker.k8s.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Net10.docker.k8s", LogEventLevel.Debug)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Debug());

// Add services to the container.
// Configure Swagger/OpenAPI (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Configure DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Register repository and services (scoped)
builder.Services.AddScoped<IPersonRepository, PersonRepositoryImpl>();
builder.Services.AddScoped<IPersonServices, PersonServicesImpl>();
builder.Services.AddScoped<ICarRepository, CarRepositoryImpl>();
builder.Services.AddScoped<ICarServices, CarServicesImpl>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
