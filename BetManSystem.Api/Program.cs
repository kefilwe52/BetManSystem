

using BetManSystem.Application.Interfaces.Adapters;
using BetManSystem.Application.Interfaces.Services;
using BetManSystem.Application.Services;
using BetManSystem.Common.Settings;
using BetManSystem.DataAccess.Context;
using BetManSystem.DataAccess.IRepositories;
using BetManSystem.DataAccess.Repositories;
using BetManSystem.Integrations.Adapters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BetManDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);

builder.Services.Configure<WalletProvidersSettings>(
    builder.Configuration.GetSection("WalletProviders"));

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<WalletProvidersSettings>>().Value);

builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerExternalAccountRepository, PlayerExternalAccountRepository>();
builder.Services.AddScoped<IMessageLogRepository, MessageLogRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IPlayerExternalAccountService, PlayerExternalAccountService>();
builder.Services.AddScoped<IMessageLogService, MessageLogService>();
builder.Services.AddScoped<IWalletTransactionService, WalletTransactionService>();

builder.Services.AddTransient<BetWayWalletAdapter>();
builder.Services.AddTransient<BetGamesWalletAdapter>();
builder.Services.AddScoped<IWalletIntegrationAdapterFactory, WalletIntegrationAdapterFactory>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});


builder.Services.AddHttpClient<BetWayWalletAdapter>((sp, client) =>
{
    var wp = sp.GetRequiredService<WalletProvidersSettings>().BetWay;
    client.BaseAddress = new Uri(wp.BaseUrl);
    client.DefaultRequestHeaders.Add("X-Api-Key", wp.ApiKey);
    client.DefaultRequestHeaders.Add("X-Api-Secret", wp.ApiSecret);
});

builder.Services.AddHttpClient<BetGamesWalletAdapter>((sp, client) =>
{
    var wp = sp.GetRequiredService<WalletProvidersSettings>().BetGames;
    client.BaseAddress = new Uri(wp.BaseUrl);
    client.DefaultRequestHeaders.Add("X-Api-Key", wp.ApiKey);
    client.DefaultRequestHeaders.Add("X-Api-Secret", wp.ApiSecret);
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BetMan API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthorization();

var app = builder.Build();

//apply migration if db and tanle doesn't exist
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BetManDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
