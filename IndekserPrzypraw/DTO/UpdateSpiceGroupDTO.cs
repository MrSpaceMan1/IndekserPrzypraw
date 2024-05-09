using System.ComponentModel.DataAnnotations;

namespace IndekserPrzypraw.DTO;

public class UpdateSpiceGroupDTO
{
  public int? MinimumGrams { get; set; }
  public int? MinimumCount { get; set; }
  public string? Name { get; set; }
}