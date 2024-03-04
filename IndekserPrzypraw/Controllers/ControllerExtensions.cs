using IndekserPrzypraw.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IndekserPrzypraw.Controllers;

public static class ControllerExtensions
{
  public static ActionResult BadRequestDueTo(this ControllerBase controller, ModelStateException exception)
  {
    exception.AddToModelState(controller.ModelState);
    var options = controller.HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
    return (ActionResult)options.Value.InvalidModelStateResponseFactory(controller.ControllerContext);
  }
}