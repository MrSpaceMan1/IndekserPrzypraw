using IndekserPrzypraw.DTO;

namespace IndekserPrzypraw.Profiles.Services;

public interface IBarcodeService
{
  Task<BarcodeInfoDTO?> GetBarcodeInfoAsync(string barcode);
  Task AddBarcodeInfo()
}