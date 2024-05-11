using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using IndekserPrzypraw.Repositories;

namespace IndekserPrzypraw
{
  [Route("api/[controller]")]
  [ApiController]
  public class SpiceMixController : ControllerBase
  {
    private readonly SpiceMixRepository _spiceMixRepository;
    private readonly UnitOfWork<SpicesContext> _unitOfWork;
    private readonly IMapper _mapper;

    public SpiceMixController(SpicesContext context, IMapper mapper)
    {
      _spiceMixRepository = new SpiceMixRepository(context);
      _unitOfWork = new UnitOfWork<SpicesContext>(context);
      _mapper = mapper;
    }

    // GET: api/SpiceMix
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SpiceMixRecipe>>> GetSpiceMixRecipes()
    {
      var spiceMixes = await _spiceMixRepository.GetAllSpiceMixAsync();
      return Ok(_mapper.)
    }

    // GET: api/SpiceMix/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SpiceMixRecipe>> GetSpiceMixRecipe(int id)
    {
      var spiceMixRecipe = await _context.SpiceMixRecipes.FindAsync(id);

      if (spiceMixRecipe == null)
      {
        return NotFound();
      }

      return spiceMixRecipe;
    }

    // PUT: api/SpiceMix/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSpiceMixRecipe(int id, SpiceMixRecipe spiceMixRecipe)
    {
      if (id != spiceMixRecipe.SpiceMixRecipeId)
      {
        return BadRequest();
      }

      _context.Entry(spiceMixRecipe).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!SpiceMixRecipeExists(id))
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

    // POST: api/SpiceMix
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<SpiceMixRecipe>> PostSpiceMixRecipe(SpiceMixRecipe spiceMixRecipe)
    {
      _context.SpiceMixRecipes.Add(spiceMixRecipe);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetSpiceMixRecipe", new { id = spiceMixRecipe.SpiceMixRecipeId }, spiceMixRecipe);
    }

    // DELETE: api/SpiceMix/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSpiceMixRecipe(int id)
    {
      var spiceMixRecipe = await _context.SpiceMixRecipes.FindAsync(id);
      if (spiceMixRecipe == null)
      {
        return NotFound();
      }

      _context.SpiceMixRecipes.Remove(spiceMixRecipe);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool SpiceMixRecipeExists(int id)
    {
      return _context.SpiceMixRecipes.Any(e => e.SpiceMixRecipeId == id);
    }
  }
}