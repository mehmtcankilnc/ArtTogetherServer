using ArtTogether.Application.Interfaces;
using ArtTogether.Domain.Interfaces;
using ArtTogether.Infrastructure.Hubs;
using ArtTogether.Infrastructure.Persistence;
using ArtTogether.Infrastructure.Repositories;
using ArtTogether.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(ArtTogether.Application.DTOs.StrokeDto).Assembly));

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
app.UseAuthorization();

app.MapControllers();

app.MapHub<DrawingHub>("/hubs/drawing");

app.Run();
