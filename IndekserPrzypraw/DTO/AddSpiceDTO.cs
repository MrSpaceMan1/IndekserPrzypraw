using System.ComponentModel.DataAnnotations;

namespace IndekserPrzypraw.DTO;

public record AddSpiceDTO(
    [Required]
    string Name,
    [Required]
    uint Grams,
    DateOnly ExpirationDate,
    [Required]
    string Barcode
  );