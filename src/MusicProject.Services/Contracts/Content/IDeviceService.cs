using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MusicProject.Entities.Content;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Contracts.Content
{
    public interface IDeviceService
    {
      Device AddNewDevice(Device device);
      void UpdateDevice(Device device);
      IList<Device> GetPagedDevice(int pageNumber, int pageSize, string type,
        string title);
      Task<Device> FindByIdAsync(int id);
      Task<Device> FindByDeviceAsync(string devId);
      void Delete(int id);
  }
}