using IndekserPrzypraw.DTO;

namespace IndekserPrzypraw.Profiles.Services;

public interface ISpiceService
{
  Task<BarcodeInfoDTO?> GetSpiceByBarcodeAsync(string barcode);
  Task<IEnumerable<SpiceDTO>> GetAllSpicesAsync();
  Task<SpiceDTO?> GetSpiceAsync(int id);
  Task<SpiceDTO> AddSpiceAsync(int drawerId, AddSpiceDTO addSpiceDto);
  Task<IEnumerable<SpiceDTO>> GetSpicesInDrawerAsync(int drawerId);
  Task RemoveSpiceAsync(int spiceId);
}