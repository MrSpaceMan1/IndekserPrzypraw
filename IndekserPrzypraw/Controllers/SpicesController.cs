using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IndekserPrzypraw.Domain;
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
        private readonly SpicesContext _context;
        private readonly SpiceRepository _spiceRepository;
        private readonly SpiceGroupRepository _spiceGroupRepository;
        private readonly DrawerRepository _drawerRepository;
        private readonly ILogger<SpicesController> _logger;
        private readonly UnitOfWork<SpicesContext> _unitOfWork;
        private readonly IMapper _mapper;

        public SpicesController(SpicesContext context, ILogger<SpicesController> logger, IMapper mapper)
        {
            _context = context;
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
            IEnumerable<Spice> spices = await _spiceRepository.GetAllSpices();
            var spiceDTOs = _mapper.Map<IEnumerable<Spice>, List<SpiceDTO>>(spices);
            _logger.LogInformation("List of all spices: [{0}]", string.Join(", ", spiceDTOs));
            return Ok(spiceDTOs);
        }

        // GET: api/Spices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Spice>> GetSpice(int id)
        {
            var spice = await _context.Spices.FindAsync(id);

            if (spice == null)
            {
                return NotFound();
            }

            return spice;
        }

        // PUT: api/Spices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpice(int id, Spice spice)
        {
            if (id != spice.SpiceId)
            {
                return BadRequest();
            }

            _context.Entry(spice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/Spices
        [HttpPost("{drawerId}")]
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
                Spice spice = await _spiceRepository.AddSpice(new Spice()
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

        // DELETE: api/Spices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpice(int id)
        {
            var spice = await _context.Spices.FindAsync(id);
            if (spice is null)
            {
                return NotFound();
            }

            _context.Spices.Remove(spice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpiceExists(int id)
        {
            return _context.Spices.Any(e => e.SpiceId == id);
        }
    }
}
