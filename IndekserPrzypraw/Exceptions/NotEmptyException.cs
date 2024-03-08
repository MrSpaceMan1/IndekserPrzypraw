using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IndekserPrzypraw.Exceptions;

public class NotEmptyException(Action<ModelStateDictionary> action) : ModelStateException(action)
{
  
}