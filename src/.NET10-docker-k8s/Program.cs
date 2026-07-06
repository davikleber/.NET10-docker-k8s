using Net10.docker.k8s.Services;
using Net10.docker.k8s.Services.Impl;
using Net10.docker.k8s.Repositories;
using Net10.docker.k8s.Repositories.Impl;
using Net10.docker.k8s.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Net10.docker.k8s.Repositories.Impl;
using Net10.docker.k8s.Services.Impl;

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
builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();

// Password hasher
builder.Services.AddSingleton<IPasswordHasher<Net10.docker.k8s.Model.User>, PasswordHasher<Net10.docker.k8s.Model.User>>();

// Jwt and auth services
builder.Services.AddSingleton<JwtTokenService>();
builder.Services.AddScoped<AuthService>();

// Cognito token service (client credentials)
builder.Services.AddHttpClient<Net10.docker.k8s.Services.ICognitoTokenService, Net10.docker.k8s.Services.CognitoTokenService>();

var jwtSection = builder.Configuration.GetSection("Jwt");
var key = jwtSection.GetValue<string>("Key");
if (!string.IsNullOrEmpty(key))
{
    var keyBytes = Encoding.UTF8.GetBytes(key);
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection.GetValue<string>("Issuer"),
            ValidAudience = jwtSection.GetValue<string>("Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });
}

var app = builder.Build();

// Fetch a Cognito token on startup (non-blocking)
_ = Task.Run(async () =>
{
    try
    {
        using var scope = app.Services.CreateScope();
        var tokenSvc = scope.ServiceProvider.GetService<Net10.docker.k8s.Services.ICognitoTokenService>();
        if (tokenSvc != null)
        {
            var token = await tokenSvc.GetClientCredentialsTokenAsync();
            if (token != null)
            {
                Log.Logger.Information("Fetched Cognito token (length={Length})", token.Access_Token?.Length ?? 0);
            }
            else
            {
                Log.Logger.Warning("Failed to fetch Cognito token on startup.");
            }
        }
    }
    catch (Exception ex)
    {
        Log.Logger.Error(ex, "Error while fetching Cognito token on startup");
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
