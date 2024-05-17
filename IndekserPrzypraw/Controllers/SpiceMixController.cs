using AutoMapper;
using IndekserPrzypraw.DTO;
using IndekserPrzypraw.Exceptions;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using IndekserPrzypraw.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IndekserPrzypraw.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SpiceMixController : ControllerBase
  {
    private readonly SpiceMixRepository _spiceMixRepository;
    private readonly UnitOfWork<SpicesContext> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public SpiceMixController(SpicesContext context, IMapper mapper, ILogger<SpiceMixController> logger)
    {
      _unitOfWork = new UnitOfWork<SpicesContext>(context);
      _spiceMixRepository = new SpiceMixRepository(context);
      _mapper = mapper;
      _logger = logger;
    }

    // GET: api/SpiceMix
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SpiceMixDTO>>> GetSpiceMixRecipes()
    {
      var spiceMixes = await _spiceMixRepository.GetAllSpiceMixAsync();
      return Ok(_mapper.Map<List<SpiceMixRecipe>, List<SpiceMixDTO>>(spiceMixes));
    }

    // GET: api/SpiceMix/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SpiceMixDTO>> GetSpiceMixRecipe(int id)
    {
      var spiceMix = await _spiceMixRepository.GetSpiceMixAsync(id);
      if (spiceMix is null)
        return NotFound();
      return Ok(_mapper.Map<SpiceMixRecipe, SpiceMixDTO>(spiceMix));
    }

    // PUT: api/SpiceMix/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSpiceMixRecipe(int id, AddSpiceMixRecipeDTO spiceMixRecipe)
    {
      var spiceMix = await _spiceMixRepository.GetSpiceMixAsync(id);
      if (spiceMix is null) return NotFound();
      var updatedSpiceMix = _mapper.Map(spiceMixRecipe, spiceMix);

      await _unitOfWork.BeginTransaction();
      try
      {
        await _spiceMixRepository.ChangeSpiceMixAsync(updatedSpiceMix);
        await _unitOfWork.Commit();
        return NoContent();
      }
      catch
      {
        await _unitOfWork.Rollback();
        return BadRequest();
      }
    }

    // POST: api/SpiceMix
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<SpiceMixDTO>> PostSpiceMixRecipe(AddSpiceMixRecipeDTO spiceMixRecipe)
    {
      await _unitOfWork.BeginTransaction();
      try
      {
        List<Ingredient> newIngredients = (
          await _spiceMixRepository.AddIngredientsAsync(_mapper.Map<List<AddIngredientDTO>, List<Ingredient>>(spiceMixRecipe.Ingredients))
          ).ToList();
        var added = await _spiceMixRepository.AddSpiceMixAsync(new SpiceMixRecipe
        {
          Name = spiceMixRecipe.Name,
          Ingredients = newIngredients
        });
        await _unitOfWork.Commit();
        return Ok(_mapper.Map<SpiceMixRecipe, SpiceMixDTO>(added));
      }
      catch (Exception e)
      {
        await _unitOfWork.Rollback();
        _logger.LogError(e.ToString());
        return BadRequest();
      }
    }

    // DELETE: api/SpiceMix/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSpiceMixRecipe(int id)
    {
      await _unitOfWork.BeginTransaction();
      try
      {
        await _spiceMixRepository.RemoveIngredientsAsync(id);
        await _spiceMixRepository.RemoveSpiceMixAsync(id);
        await _unitOfWork.Commit();
      }
      catch (NotFoundException notFoundException)
      {
        await _unitOfWork.Rollback();
        return this.BadRequestDueTo(notFoundException);
      }
      
      catch (Exception e)
      {
        await _unitOfWork.Rollback();
        _logger.LogError(e.ToString());
        return BadRequest();
      }
      return NoContent();
    }
  }
}