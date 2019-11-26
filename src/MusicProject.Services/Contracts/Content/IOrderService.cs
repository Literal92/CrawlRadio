using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MusicProject.Entities.Content;
using MusicProject.ViewModels.Content;
using Microsoft.AspNetCore.Http;

namespace MusicProject.Services.Contracts.Content
{
  public interface IOrderService
  {
    void AddNewOrder(Order order, IFormFile file);
    //Task<PagedSessionRequestViewModel> GetPagedSessionRequestAsync(int pageNumber, int pageSize, SortOrder sortOrder);
    void Delete(int id);
    Task<PagedOrderViewModel> GetPagedOrderAsync(int pageNumber, int pageSize, SortOrder sortOrder);
  }
}
