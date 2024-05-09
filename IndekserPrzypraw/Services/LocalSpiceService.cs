using AutoMapper;
using IndekserPrzypraw.DTO;
using IndekserPrzypraw.Exceptions;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Profiles;
using IndekserPrzypraw.Profiles.Services;
using IndekserPrzypraw.Repositories;

namespace IndekserPrzypraw.Services;

public class LocalSpiceService : ISpiceService
{
  private SpiceGroupRepository _spiceGroupRepository;
  private SpiceRepository _spiceRepository;
  private UnitOfWork<SpicesContext> _unitOfWork;
  private IMapper _mapper;
  private DrawerRepository _drawerRepository;
  private ILogger _logger;

  public LocalSpiceService(UnitOfWork<SpicesContext> unitOfWork, IMapper mapper, ILogger logger)
  {
    _unitOfWork = unitOfWork;
    _spiceGroupRepository = new SpiceGroupRepository(_unitOfWork);
    _spiceRepository = new SpiceRepository(_unitOfWork);
    _drawerRepository = new DrawerRepository(_unitOfWork);
    _mapper = mapper;
    _logger = logger;
  }

  public async Task<BarcodeInfoDTO?> GetSpiceByBarcodeAsync(string barcode)
  {
    SpiceGroup? spiceGroup = await _spiceGroupRepository.GetSpiceByBarcodeAsync(barcode);
    return spiceGroup is null ? null : _mapper.Map<SpiceGroup, BarcodeInfoDTO>(spiceGroup);
  }

  public async Task<IEnumerable<SpiceDTO>> GetAllSpicesAsync()
  {
    IEnumerable<Spice> spices = await _spiceRepository.GetAllSpicesAsync();
    return _mapper.Map<IEnumerable<Spice>, List<SpiceDTO>>(spices);
  }

  public async Task<SpiceDTO?> GetSpiceAsync(int id)
  {
    Spice? spice = await _spiceRepository.GetSpiceByIdAsync(id);
    return spice is null ? null : _mapper.Map<Spice, SpiceDTO>(spice);
  }

  public async Task<SpiceDTO> AddSpiceAsync(int drawerId, AddSpiceDTO addSpiceDto)
  {
    await _unitOfWork.BeginTransaction();
    Drawer? drawer = await _drawerRepository.GetDrawerByIdAsync(drawerId);
    if (drawer is null)
    {
      await _unitOfWork.Rollback();
      throw new NotFoundException(x => x.AddModelError(nameof(drawerId), $"Drawer with {drawerId} doesn't exist"));
    }

    SpiceGroup? spiceGroup = await _spiceGroupRepository.GetSpiceGroupByNameAsync(addSpiceDto.Name, drawerId);
    if (spiceGroup is null)
    {
      spiceGroup = await _spiceGroupRepository.AddSpiceGroupAsync(
        addSpiceDto.Name,
        addSpiceDto.Barcode,
        addSpiceDto.Grams,
        drawerId,
        null,
        null
      );
    }

    Spice spice = await _spiceRepository.AddSpiceAsync(new Spice
    {
      SpiceGroupId = spiceGroup.SpiceGroupId,
      ExpirationDate = addSpiceDto.ExpirationDate ?? null,
    });
    await _unitOfWork.Commit();

    return _mapper.Map<Spice, SpiceDTO>(spice);
  }

  public async Task<IEnumerable<SpiceDTO>> GetSpicesInDrawerAsync(int drawerId)
  {
    IEnumerable<Spice> spices = await _spiceRepository.GetAllSpicesFromDrawerAsync(drawerId);
    return _mapper.Map<IEnumerable<Spice>, IEnumerable<SpiceDTO>>(spices);
  }

  public async Task RemoveSpiceAsync(int spiceId)
  {
    Spice? spice = await _spiceRepository.GetSpiceByIdAsync(spiceId);
    if (spice is null)
      throw new NotFoundException(x => x.AddModelError(nameof(spice), $"Spice with id {spiceId} not found"));
    await _unitOfWork.BeginTransaction();
    await _spiceRepository.DeleteSpiceAsync(spice);
    await _unitOfWork.Commit();
  }

  public async Task<Dictionary<string, List<Spice>>> GetSpiceGroups()
  {
    var groups = await _spiceRepository.GetSpiceByGroupsAsync(1);
    Console.WriteLine(groups.ToString());
    return groups;
  }

  public async Task RemoveSpiceGroupWithSpices(int spiceGroupId)
  {
    var spiceGroup = await _spiceGroupRepository.GetSpiceGroupByIdAsync(spiceGroupId);
    if (spiceGroup is null)
      throw new NotFoundException(x =>
        x.AddModelError($"{nameof(spiceGroupId)}", $"Spice group with provided id doesn't exist"));
    await _unitOfWork.BeginTransaction();
    foreach (var spice in spiceGroup.Spices)
    {
      await RemoveSpiceAsync(spice.SpiceId);
    }

    await _spiceGroupRepository.RemoveSpiceGroupAsync(spiceGroupId);
    await _unitOfWork.Commit();
  }

  public async Task<SpiceGroupDTO> UpdateSpiceGroup(int spiceGroupId, UpdateSpiceGroupDTO updateSpiceGroupDto)
  {
    var spiceGroup = await _spiceGroupRepository.GetSpiceGroupByIdAsync(spiceGroupId);
    if (spiceGroup is null)
      throw new NotFoundException(x =>
        x.AddModelError(nameof(spiceGroupId), $"Spice group with id {spiceGroupId} doesn't exist"));

    var updatedSpiceGroup = _mapper.Map(updateSpiceGroupDto, spiceGroup);
    await _unitOfWork.BeginTransaction();
    updatedSpiceGroup = await _spiceGroupRepository.UpdateSpiceGroupAsync(updatedSpiceGroup);
    await _unitOfWork.Commit();
    return _mapper.Map<SpiceGroupDTO>(updatedSpiceGroup);
  }

  public async Task<MissingSpicesDTO> GetMissingSpices()
  {
    var missingSpices = new Dictionary<string, ICollection<MissingSpiceGroupDTO>>();
    var drawers = await _drawerRepository.GetAllDrawersAsync();
    foreach (var drawer in drawers)
    {
      var missingSpiceGroupsInDrawer = new List<MissingSpiceGroupDTO>();
      foreach (var spiceGroup in drawer.SpiceGroups)
      {
        var grams = spiceGroup.Spices.Aggregate(0, (acc, spice) => acc + spice.SpiceGroup.Grams,
          u => u);
        var count = spiceGroup.Spices.Count;
        if ((spiceGroup.MinimumCount ?? 0) > count || (spiceGroup.MinimumGrams ?? 0) > grams)
          missingSpiceGroupsInDrawer.Add(new MissingSpiceGroupDTO
          {
            Name = spiceGroup.Name,
            SpiceGroupId = spiceGroup.SpiceGroupId,
            MissingGrams = Math.Clamp((spiceGroup.MinimumGrams ?? 0) - grams, 0, Int32.MaxValue),
            MissingCount = Math.Clamp((spiceGroup.MinimumCount ?? 0) - count, 0, Int32.MaxValue)
          });
      }

      if (missingSpiceGroupsInDrawer.Count > 0)
        missingSpices.Add(drawer.Name, missingSpiceGroupsInDrawer);
    }

    return new MissingSpicesDTO
    {
      MissingSpices = missingSpices
    };
  }
}