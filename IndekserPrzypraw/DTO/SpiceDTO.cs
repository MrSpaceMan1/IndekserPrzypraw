namespace IndekserPrzypraw.DTO;

public class SpiceDTO(
  int SpiceId, 
  string Name,
  DateOnly ExpirationDate, 
  uint Grams,
  int SpiceGroupId, 
  int DrawerId,
  string Barcode
  );