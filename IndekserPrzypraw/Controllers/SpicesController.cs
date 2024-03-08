using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IndekserPrzypraw.DTO;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using IndekserPrzypraw.Repositories;

namespace IndekserPrzypraw.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class SpicesController : ControllerBase
    {
        private readonly SpiceRepository _spiceRepository;
        private readonly SpiceGroupRepository _spiceGroupRepository;
        private readonly DrawerRepository _drawerRepository;
        private readonly ILogger<SpicesController> _logger;
        private readonly UnitOfWork<SpicesContext> _unitOfWork;
        private readonly IMapper _mapper;

        public SpicesController(SpicesContext context, ILogger<SpicesController> logger, IMapper mapper)
        {
            _unitOfWork = new UnitOfWork<SpicesContext>(context);
            _spiceRepository = new SpiceRepository(_unitOfWork);
            _spiceGroupRepository = new SpiceGroupRepository(_unitOfWork);
            _drawerRepository = new DrawerRepository(_unitOfWork);
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Spices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpiceDTO>>> GetSpices()
        {
            IEnumerable<Spice> spices = await _spiceRepository.GetAllSpicesAsync();
            var spiceDtos = _mapper.Map<IEnumerable<Spice>, List<SpiceDTO>>(spices);
            _logger.LogInformation("List of all spices: [{0}]", string.Join(", ", spiceDtos));
            return Ok(spiceDtos);
        }

        // GET: api/Spices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SpiceDTO>> GetSpice(int id)
        {
            Spice? spice = await _spiceRepository.GetSpiceByIdAsync(id);
            if (spice is null) return NotFound($"Spice with id {id} not found");
            return Ok(_mapper.Map<Spice, SpiceDTO>(spice));
        }

        [HttpPost("drawer/{drawerId}")]
        public async Task<ActionResult<SpiceDTO>> PostSpice(int drawerId, [FromBody] AddSpiceDTO addSpiceDto)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                Drawer? drawer = await _drawerRepository.GetDrawerByIdAsync(drawerId);
                if (drawer is null)
                {
                    await _unitOfWork.Rollback();
                    return NotFound($"Drawer with id {drawerId} not found");
                }
                SpiceGroup? spiceGroup = await _spiceGroupRepository.GetSpiceGroupByNameAsync(addSpiceDto.Name);
                if (spiceGroup is null)
                {
                    spiceGroup = await _spiceGroupRepository.AddSpiceGroupAsync(addSpiceDto.Name, null, null);
                }
                Spice spice = await _spiceRepository.AddSpiceAsync(new Spice()
                {
                    SpiceGroupId = spiceGroup.SpiceGroupId,
                    DrawerId = drawer.DrawerId,
                    ExpirationDate = addSpiceDto.ExpirationDate,
                    Grams = addSpiceDto.Grams
                });
                await _unitOfWork.Commit();
                return Ok(_mapper.Map<Spice, SpiceDTO>(spice));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
                await _unitOfWork.Rollback();
                return BadRequest();
            }
            
        }
        
        [HttpGet("drawer/{drawerId}")]
        public async Task<ActionResult<SpiceDTO>> GetSpicesInDrawer(int drawerId)
        {
            IEnumerable<Spice> spices = await _spiceRepository.GetAllSpicesFromDrawerAsync(drawerId);
            List<SpiceDTO> spiceDtos = _mapper.Map<IEnumerable<Spice>, List<SpiceDTO>>(spices);

            return Ok(spiceDtos);
        }

        // DELETE: api/Spices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpice(int id)
        {
            Spice? spice = await _spiceRepository.GetSpiceByIdAsync(id);
            if (spice is null) return NotFound($"Spice with id {id} not found");
            await _unitOfWork.BeginTransaction();
            try
            {
                await _spiceRepository.DeleteSpiceAsync(spice);
                await _unitOfWork.Commit();
                return NoContent();
            }
            catch
            {
                await _unitOfWork.Rollback();
                return BadRequest();
            }
        }
    }
}
