using ArtTogether.Application.Interfaces;
using ArtTogether.Domain.Interfaces;
using ArtTogether.Infrastructure.Hubs;
using ArtTogether.Infrastructure.Identity;
using ArtTogether.Infrastructure.Persistence;
using ArtTogether.Infrastructure.Repositories;
using ArtTogether.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddIdentity<ApplicationIdentityUser, IdentityRole<Guid>>(opt =>
{
    opt.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
    };
});

builder.Services.AddMediatR(cfg =>
{
    cfg.LicenseKey = builder.Configuration["MediatR:LicenseKey"];
    cfg.RegisterServicesFromAssembly(typeof(ArtTogether.Application.DTOs.StrokeDto).Assembly);
});

builder.Services.AddScoped<IStrokeRepository, StrokeRepository>();
builder.Services.AddTransient<IDrawingNotifier, SignalRDrawingNotifier>();

builder.Services.AddSignalR();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

//app.UseHttpsRedirection();
app.UseAuthentication();    
app.UseAuthorization();

app.MapControllers();

app.MapHub<DrawingHub>("/hubs/drawing");

app.Run();
