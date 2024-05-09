using AutoMapper;
using IndekserPrzypraw.DTO;
using IndekserPrzypraw.Exceptions;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using IndekserPrzypraw.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IndekserPrzypraw.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DrawerController : ControllerBase
{
  private readonly DrawerRepository _drawerRepository;
  private readonly ILogger<DrawerController> _logger;
  private readonly IMapper _mapper;
  private readonly SpiceGroupRepository _spiceGroupRepository;
  private readonly UnitOfWork<SpicesContext> _unitOfWork;

  public DrawerController(SpicesContext context, IMapper mapper, ILogger<DrawerController> logger)
  {
    _mapper = mapper;
    _unitOfWork = new UnitOfWork<SpicesContext>(context);
    _drawerRepository = new DrawerRepository(_unitOfWork);
    _spiceGroupRepository = new SpiceGroupRepository(_unitOfWork);
    _logger = logger;
  }

  // GET: api/Drawer
  [HttpGet]
  public async Task<ActionResult<IEnumerable<DrawerDTO>>> GetDrawers()
  {
    var drawers = await _drawerRepository.GetAllDrawersAsync();
    var drawersDtos = _mapper.Map<List<Drawer>, List<DrawerDTO>>(drawers);
    return Ok(drawersDtos);
  }

  // GET: api/Drawer/5
  [HttpGet("{id}")]
  public async Task<ActionResult<DrawerDTO>> GetDrawer(int id)
  {
    var drawer = await _drawerRepository.GetDrawerByIdAsync(id);
    if (drawer is null)
      return NotFound($"Drawer with id {id} not found");

    var drawerDto = _mapper.Map<Drawer, DrawerDTO>(drawer);

    return Ok(drawerDto);
  }

  // PUT: api/Drawer/5
  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> PutDrawer(int id, AddDrawerDTO updateDrawerDto)
  {
    var drawer = await _drawerRepository.GetDrawerByIdAsync(id);
    if (drawer is null) return NotFound($"Drawer with id {id} not found");
    var update = _mapper.Map(updateDrawerDto, drawer);
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
      var newDrawer = await _drawerRepository.AddDrawerAsync(new Drawer
      {
        Name = addDrawerDto.Name.ToLower()
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
    var drawer = await _drawerRepository.GetDrawerByIdAsync(id);
    if (drawer is null) return NotFound($"Drawer with id {id} not found");

    if (!drawer.SpiceGroups.IsNullOrEmpty())
      return this.BadRequestDueTo(new NotEmptyException(x =>
        x.AddModelError(
          nameof(drawer.SpiceGroups),
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
    var fromDrawer = await _drawerRepository.GetDrawerByIdAsync(from);
    if (fromDrawer is null) return NotFound($"Drawer with id {from} not found");

    var toDrawer = await _drawerRepository.GetDrawerByIdAsync(to);
    if (toDrawer is null) return NotFound($"Drawer with id {to} not found");

    await _unitOfWork.BeginTransaction();
    try
    {
      await _spiceGroupRepository.TransferSpiceGroupsAsync(from, to);
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