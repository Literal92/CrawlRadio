using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;

using Microsoft.EntityFrameworkCore;
using MusicProject.ViewModels.Content;


namespace MusicProject.Services.Content
{
  public class EfAppSettingService : IAppSettingService
  {
    IUnitOfWork _uow;
    readonly DbSet<AppSetting> _AppSettings;
    private readonly IUploadService _uploadService;
    private readonly IHostingEnvironment _hostingEnvironment;

    public EfAppSettingService(
      IUnitOfWork uow,
      IUploadService uploadService,
      IHostingEnvironment hostingEnvironment
    )
    {
      _uow = uow;
      _uow.CheckArgumentIsNull(nameof(_uow));
      _uploadService = uploadService;
      _AppSettings = _uow.Set<AppSetting>();
      _hostingEnvironment = hostingEnvironment;
    }

    public int AddNewAppSetting(AppSetting appSetting)
    {
      var id = _AppSettings.Add(appSetting);
      _uow.SaveChanges();
      return id.Entity.Id;
    }

    public IList<AppSetting> GetAll(int typeId, string state, int pageNumber, int pageSize)
    {

      var offset = (pageSize * pageNumber) - pageSize;
      var query = typeId == 0 ?
        _AppSettings :
        _AppSettings.Where(l => l.TypeId == typeId);

      query = query.OrderByDescending(x => x.Id);
      return query.Skip(offset).Take(pageSize).ToList();
    }

    public int GetCount(int typeId, string title)
    {
      throw new NotImplementedException();
    }

    public async Task<IList<AppSetting>> GetAllAsync(int typeId, string state, int pageNumber, int pageSize)
    {

      var offset = (pageSize * pageNumber) - pageSize;
      var query = typeId == 0 ?
        _AppSettings :
        _AppSettings.Where(l => l.TypeId == typeId);
 
      query = query.OrderByDescending(x => x.Id);

      var result = await query.Skip(offset).Take(pageSize).ToListAsync();
      return result;
    }



    public async Task<PagedAppSettingViewModel> GetPagedAppSettingAsync(int pageNumber, int pageSize, SortOrder sortOrder, int type)
    {
      var offset = (pageSize * pageNumber) - pageSize;
      var query = type == 0 ?
        _AppSettings :
        _AppSettings.Where(l => l.TypeId == type);

 
      query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
      return new PagedAppSettingViewModel
      {
        Paging =
        {
          TotalItems = await query.CountAsync().ConfigureAwait(false)
        },
        AppSettingItem = await query.Skip(offset).Take(pageSize).ToListAsync().ConfigureAwait(false)
      };
    }

    public Task<AppSetting> FindByIdAsync(int id)
    {
      return _AppSettings.SingleOrDefaultAsync(p => p.Id == id);
    }

    public void UpdateAppSetting(AppSetting appSetting)
    {
      _AppSettings.Update(appSetting);
      _uow.SaveChanges();
    }

    public void Delete(int id)
    {
      var itemToRemove = _AppSettings.Find(id);
      if (itemToRemove != null)
      {
        _AppSettings.Remove(itemToRemove);
        _uow.SaveChanges();
      }
    }
  }
}
