using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Content
{
  public class EfDeviceService : IDeviceService
  {
    IUnitOfWork _uow;
    readonly DbSet<Device> _devices;
    private readonly IUploadService _uploadService;

    public EfDeviceService(IUnitOfWork uow,
      IUploadService uploadService)
    {
      _uow = uow;
      _uploadService = uploadService;

      _uow.CheckArgumentIsNull(nameof(_uow));
      _devices = _uow.Set<Device>();

    }

    public Device AddNewDevice(Device device)
    {
      _devices.Add(device);
      _uow.SaveChanges();
      return device;
    }

  

    public async Task<IList<Device>> GetAllByTypeAsync(string typeId)
    {
      return await _devices.OrderBy(p => p.Id).ToListAsync();

    }

    public void UpdateDevice(Device device)
    {
      _devices.Update(device);
      _uow.SaveChanges();
    }

    public IList<Device> GetPagedDevice(int pageNumber, int pageSize, string type, string title)
    {
      throw new NotImplementedException();
    }

    public Task<Device> FindByIdAsync(int id)
    {
      return _devices.SingleOrDefaultAsync(p => p.Id == id);
    }
    public Task<Device> FindByDeviceAsync(string devId)
    {
      return _devices.Include(a=>a.User).SingleOrDefaultAsync(p => p.DeviceId == devId);
    }

    public void Delete(int id)
    {
      var itemToRemove = _devices.FirstOrDefault(x => x.Id.Equals(id));
      if (itemToRemove != null)
      {
        _devices.Remove(itemToRemove);
        _uow.SaveChanges();
      }
    }
  }
}
