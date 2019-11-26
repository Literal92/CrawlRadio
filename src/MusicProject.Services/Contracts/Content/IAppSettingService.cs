using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MusicProject.Entities.Content;
using Microsoft.AspNetCore.Http;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Contracts.Content
{
  public interface IAppSettingService
  {
    int AddNewAppSetting(AppSetting appSetting);


    IList<AppSetting> GetAll(int typeId,string state, int pageNumber, int pageSize);
   int GetCount(int typeId,string title);
   Task<IList<AppSetting>> GetAllAsync(int typeId,string state, int pageNumber, int pageSize);

    Task<PagedAppSettingViewModel> GetPagedAppSettingAsync(int pageNumber, int pageSize, SortOrder sortOrder, int type );
    Task<AppSetting> FindByIdAsync(int id);
    void UpdateAppSetting(AppSetting appSetting);
    void Delete(int id);
  }
}
