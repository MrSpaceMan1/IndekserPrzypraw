namespace IndekserPrzypraw.DTO;

public record SpiceDTO(
  int SpiceId, 
  string Name,
  DateOnly ExpirationDate, 
  uint Grams,
  int SpiceGroupId, 
  int DrawerId
  );