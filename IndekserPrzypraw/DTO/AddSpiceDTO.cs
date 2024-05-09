using System.ComponentModel.DataAnnotations;

namespace IndekserPrzypraw.DTO;

public record AddSpiceDTO(
  [Required] string Name,
  [Required] int Grams,
  DateOnly? ExpirationDate,
  [Required] string Barcode
);