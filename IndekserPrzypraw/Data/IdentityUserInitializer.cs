using IndekserPrzypraw.Models;
using Microsoft.AspNetCore.Identity;

namespace IndekserPrzypraw.Data;

public class IdentityUserInitializer
{
  public static async Task Initialize(IdentityContext identityContext, UserManager<IdentityUser> userManager,
    ILogger<IdentityUserInitializer> logger)
  {
    identityContext.Database.EnsureCreated();
    if (identityContext.Users.Any()) return;
    var primaryAccount = new IdentityUser("admin")
    {
      Email = "admin@localhost"
    };
    var result = await userManager.CreateAsync(primaryAccount, "Admin1$");
    logger.LogError(result.ToString());
  }
}