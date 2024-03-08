using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IndekserPrzypraw.DTO;
using IndekserPrzypraw.Exceptions;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using IndekserPrzypraw.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IndekserPrzypraw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrawerController : ControllerBase
    {
        private readonly UnitOfWork<SpicesContext> _unitOfWork;
        private readonly DrawerRepository _drawerRepository;
        private readonly SpiceRepository _spiceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DrawerController> _logger;

        public DrawerController(SpicesContext context, IMapper mapper, ILogger<DrawerController> logger)
        {
            _mapper = mapper;
            _unitOfWork = new UnitOfWork<SpicesContext>(context);
            _drawerRepository = new DrawerRepository(_unitOfWork);
            _spiceRepository = new SpiceRepository(_unitOfWork);
            _logger = logger;
        }

        // GET: api/Drawer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DrawerDTO>>> GetDrawers()
        {
            IEnumerable<Drawer> drawers = await _drawerRepository.GetAllDrawersAsync();
            List<DrawerDTO> drawerDtos = _mapper.Map<IEnumerable<Drawer>, List<DrawerDTO>>(drawers);
            return Ok(drawerDtos);
        }

        // GET: api/Drawer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Drawer>> GetDrawer(int id)
        {
            Drawer? drawer = await _drawerRepository.GetDrawerByIdAsync(id);
            if (drawer is null)
                return NotFound($"Drawer with id {id} not found");
            DrawerDTO drawerDto = _mapper.Map<Drawer, DrawerDTO>(drawer);
            return Ok(drawerDto);
        }

        // PUT: api/Drawer/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutDrawer(int id, AddDrawerDTO updateDrawerDto)
        {
            Drawer? drawer = await _drawerRepository.GetDrawerByIdAsync(id);
            if (drawer is null) return NotFound($"Drawer with id {id} not found");
            Drawer update = _mapper.Map(updateDrawerDto, drawer);
            await _unitOfWork.BeginTransaction();
            try
            {
                await _drawerRepository.UpdateDrawerAsync(update);
                await _unitOfWork.Commit();
                return Ok(_mapper.Map<Drawer, DrawerDTO>(update));
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                return StatusCode(500);
            }
        }

        // POST: api/Drawer
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DrawerDTO>> PostDrawer(AddDrawerDTO addDrawerDto)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                Drawer newDrawer = await _drawerRepository.AddDrawerAsync(new Drawer()
                {
                    Name = addDrawerDto.Name
                });
                await _unitOfWork.Commit();
                return Ok(_mapper.Map<Drawer, DrawerDTO>(newDrawer));
            }
            catch (NotUniqueException exception)
            {
                await _unitOfWork.Rollback();
                return this.BadRequestDueTo(exception);
            }
        }

        // DELETE: api/Drawer/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDrawer(int id)
        {
            Drawer? drawer = await _drawerRepository.GetDrawerByIdAsync(id);
            if (drawer is null) return NotFound($"Drawer with id {id} not found");

            if (!drawer.Spices.IsNullOrEmpty())
                return this.BadRequestDueTo(new NotEmptyException(x =>
                    x.AddModelError(
                        nameof(drawer.Spices), 
                        $"Drawer with id {id} is not empty"
                    )
                ));
            
            
            await _unitOfWork.BeginTransaction();
            try
            {
                await _drawerRepository.RemoveDrawerAsync(drawer);
                await _unitOfWork.Commit();
                return NoContent();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                return BadRequest();
            }
        }

        [HttpPost("{from}/{to}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MoveDrawer(int from, int to)
        {
            Drawer? fromDrawer = await _drawerRepository.GetDrawerByIdAsync(from);
            if (fromDrawer is null) return NotFound($"Drawer with id {from} not found");
            
            Drawer? toDrawer = await _drawerRepository.GetDrawerByIdAsync(to);
            if (toDrawer is null) return NotFound($"Drawer with id {to} not found");

            await _unitOfWork.BeginTransaction();
            try
            {
                await _spiceRepository.TransferSpicesAsync(from, to);
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
