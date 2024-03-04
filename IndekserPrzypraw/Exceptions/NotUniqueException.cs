using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace IndekserPrzypraw.Exceptions;

[Serializable]
public class NotUniqueException(Action<ModelStateDictionary> action) : ModelStateException(action)
{
}