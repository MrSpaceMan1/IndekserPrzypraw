using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IndekserPrzypraw.Exceptions;

public abstract class ModelStateException(Action<ModelStateDictionary> action) : Exception
{
  public void AddToModelState(ModelStateDictionary modelState)
  {
    action(modelState);
  }
}