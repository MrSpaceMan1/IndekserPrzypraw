using System.Reflection;
using IndekserPrzypraw.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Logging.AddSimpleConsole();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SpicesContext>(options =>
  options
    .UseLazyLoadingProxies()
    .UseNpgsql(
      builder.Configuration.GetConnectionString("SpicesContext")
    )
);
builder.Services.AddControllers();
builder.Services.AddHttpsRedirection(opt => opt.HttpsPort = 443);
var app = builder.Build();

app.UseCors(opt => opt.WithOrigins(
  app
    .Configuration
    .GetSection("FrontendHost")
    .AsEnumerable()
    .Select(pair => pair.Value)
    .Where(v => v is not null)
    .ToArray()
  ));
app.MapGet("/", async context => await context.Response.WriteAsync("OK"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();