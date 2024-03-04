using System.Reflection;
using IndekserPrzypraw.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();