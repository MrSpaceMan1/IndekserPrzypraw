namespace IndekserPrzypraw.DTO;

public class SpiceDTO
{
  public int SpiceId { get; set; }
  public string Name { get; set; }
  public DateOnly? ExpirationDate { get; set; }
  public int Grams { get; set; }
  public int SpiceGroupId { get; set; }
  public string Barcode { get; set; }

  public override string ToString()
  {
    return $"SpiceDTO({nameof(SpiceId)}: {SpiceId}, {nameof(Name)}: {Name}, {nameof(ExpirationDate)}: {ExpirationDate},"
           + $" {nameof(Grams)}: {Grams}, {nameof(SpiceGroupId)}: {SpiceGroupId}, {nameof(Barcode)}: {Barcode}";
  }
};