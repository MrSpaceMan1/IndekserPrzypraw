using AutoMapper;
using IndekserPrzypraw.DTO;
using IndekserPrzypraw.Exceptions;
using IndekserPrzypraw.Models;
using IndekserPrzypraw.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IndekserPrzypraw.Profiles.Services;

public class LocalSpiceService : ISpiceService
{
  private SpiceGroupRepository _spiceGroupRepository;
  private SpiceRepository _spiceRepository;
  private UnitOfWork<SpicesContext> _unitOfWork;
  private IMapper _mapper;
  private DrawerRepository _drawerRepository;

  public LocalSpiceService(UnitOfWork<SpicesContext> unitOfWork, IMapper mapper)
  {
    _unitOfWork = unitOfWork;
    _spiceGroupRepository = new SpiceGroupRepository(_unitOfWork);
    _spiceRepository = new SpiceRepository(_unitOfWork);
    _drawerRepository = new DrawerRepository(_unitOfWork);
    _mapper = mapper;
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
    SpiceGroup? spiceGroup = await _spiceGroupRepository.GetSpiceGroupByNameAsync(addSpiceDto.Name);
    if (spiceGroup is null)
    {
      spiceGroup = await _spiceGroupRepository.AddSpiceGroupAsync(addSpiceDto.Name, addSpiceDto.Barcode, addSpiceDto.Grams, null, null);
    }
    Spice spice = await _spiceRepository.AddSpiceAsync(new Spice
    {
      SpiceGroupId = spiceGroup.SpiceGroupId,
      DrawerId = drawer.DrawerId,
      ExpirationDate = addSpiceDto.ExpirationDate,
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
    if (spice is null) throw new NotFoundException(x => x.AddModelError(nameof(spice), $"Spice with id {spiceId} not found"));
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
}