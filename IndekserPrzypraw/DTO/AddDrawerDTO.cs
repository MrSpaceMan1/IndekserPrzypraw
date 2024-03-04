using System.ComponentModel.DataAnnotations;

namespace IndekserPrzypraw.DTO;

public record AddDrawerDTO(
  [MinLength(3)]
  string Name
  );