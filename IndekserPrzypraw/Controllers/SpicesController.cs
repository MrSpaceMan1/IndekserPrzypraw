using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IndekserPrzypraw.DTO;
using IndekserPrzypraw.Exceptions;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using IndekserPrzypraw.Profiles.Services;
using IndekserPrzypraw.Repositories;
using IndekserPrzypraw.Services;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace IndekserPrzypraw.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SpicesController : ControllerBase
  {
    private readonly ISpiceService _spiceService;
    private readonly ILogger<SpicesController> _logger;
    private readonly UnitOfWork<SpicesContext> _unitOfWork;
    private readonly IMapper _mapper;

    public SpicesController(SpicesContext context, ILogger<SpicesController> logger, IMapper mapper)
    {
      _unitOfWork = new UnitOfWork<SpicesContext>(context);
      _spiceService = new LocalSpiceService(
        _unitOfWork,
        new SpiceGroupRepository(_unitOfWork),
        new SpiceRepository(_unitOfWork),
        new DrawerRepository(_unitOfWork),
        mapper
      );
      _mapper = mapper;
      _logger = logger;
    }

    [HttpGet("barcode/{barcode}")]
    public async Task<ActionResult<BarcodeInfoDTO>> GetBarcodeInfo(string barcode)
    {
      BarcodeInfoDTO? barcodeInfoDto = await _spiceService.GetSpiceByBarcodeAsync(barcode);
      if (barcodeInfoDto is null) return NotFound();
      return Ok(barcodeInfoDto);
    }

    // GET: api/Spices
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SpiceDTO>>> GetSpices()
    {
      IEnumerable<SpiceDTO> spiceDtos = await _spiceService.GetAllSpicesAsync();
      _logger.LogInformation("List of all spices: [{0}]", string.Join(", ", spiceDtos));
      return Ok(spiceDtos);
    }

    // GET: api/Spices/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SpiceDTO>> GetSpice(int id)
    {
      SpiceDTO? spice = await _spiceService.GetSpiceAsync(id);
      if (spice is null) return NotFound($"Spice with id {id} not found");
      return Ok(spice);
    }

    [HttpPost("drawer/{drawerId}")]
    public async Task<ActionResult<SpiceDTO>> PostSpice(int drawerId, [FromBody] AddSpiceDTO addSpiceDto)
    {
      try
      {
        SpiceDTO spice = await _spiceService.AddSpiceAsync(drawerId, addSpiceDto);
        return Ok(spice);
      }
      catch (Exception exception)
      {
        _logger.LogError(exception.ToString());
        await _unitOfWork.Rollback();
        return StatusCode(500);
      }
    }

    [HttpGet("drawer/{drawerId}")]
    public async Task<ActionResult<SpiceDTO>> GetSpicesInDrawer(int drawerId)
    {
      var spiceDtos = await _spiceService.GetSpicesInDrawerAsync(drawerId);
      return Ok(spiceDtos);
    }

    // DELETE: api/Spices/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSpice(int id)
    {
      try
      {
        await _spiceService.RemoveSpiceAsync(id);
        return NoContent();
      }
      catch (NotFoundException e)
      {
        await _unitOfWork.Rollback();
        return this.BadRequestDueTo(e);
      }
      catch
      {
        await _unitOfWork.Rollback();
        return StatusCode(500);
      }
    }

    [HttpDelete("group/{id}")]
    public async Task<ActionResult> RemoveSpiceGroup(int id)
    {
      try
      {
        await _spiceService.RemoveSpiceGroupWithSpices(id);
      }
      catch (NotFoundException e)
      {
        await _unitOfWork.Rollback();
        return this.BadRequestDueTo(e);
      }

      return NoContent();
    }

    [HttpPut("group/{spiceGroupId}")]
    public async Task<ActionResult> UpdateSpiceGroup(int spiceGroupId,
      [FromBody] UpdateSpiceGroupDTO updateSpiceGroupDto)
    {
      try
      {
        var spiceGroup = await _spiceService.UpdateSpiceGroup(spiceGroupId, updateSpiceGroupDto);
        return Ok(spiceGroup);
      }
      catch (NotFoundException e)
      {
        return this.BadRequestDueTo(e);
      }
    }

    [HttpGet("missing")]
    public async Task<ActionResult> GetMissingSpices()
    {
      var missingSpices = await _spiceService.GetMissingSpices();
      return Ok(missingSpices);
    }
  }
}