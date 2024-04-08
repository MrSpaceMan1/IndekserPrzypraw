using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IndekserPrzypraw.Exceptions;

public class NotFoundException(Action<ModelStateDictionary> action) : ModelStateException(action)
{
  
}