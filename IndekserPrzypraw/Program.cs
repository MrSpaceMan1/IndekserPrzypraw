using System.Reflection;
using System.Security.Claims;
using Castle.Components.DictionaryAdapter.Xml;
using IndekserPrzypraw.Data;
using IndekserPrzypraw.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddAuthentication();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
  .AddEntityFrameworkStores<IdentityContext>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Logging.AddSimpleConsole();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt => opt.AddSecurityDefinition("AspNetCoreIdentity", new OpenApiSecurityScheme()));
builder.Services.AddDbContext<IdentityContext>(
  options =>
  {
    options.UseNpgsql(
      builder.Configuration.GetConnectionString("IdentityContext")
    );
  });
builder.Services.AddDbContext<SpicesContext>(options =>
  options
    .UseNpgsql(
      builder.Configuration.GetConnectionString("SpicesContext")
    )
);
builder.Services.AddControllers();
builder.Services.AddHttpsRedirection(opt => opt.HttpsPort = 443);

var app = builder.Build();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
var scope = scopeFactory.CreateScope();
{
  var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
  var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
  var logger = scope.ServiceProvider.GetRequiredService<ILogger<IdentityUserInitializer>>();
  IdentityUserInitializer.Initialize(identityContext, userManager, logger);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors(opt =>
  opt.WithOrigins(app.Configuration.GetSection("FrontendHost").AsEnumerable().Skip(1).ToList()
      .Select(pair => pair.Value!).ToArray())
    .AllowCredentials()
    .WithHeaders("Content-Type", "User-agent")
    .WithMethods("DELETE", "POST", "GET", "PUT", "PATCH", "OPTIONS"));

app.MapGet("/api", async context => await context.Response.WriteAsync("OK"));
app.MapGroup("/api").MapIdentityApi<IdentityUser>();
app.MapControllers().RequireAuthorization();
app.UseHttpsRedirection();

app.Run();