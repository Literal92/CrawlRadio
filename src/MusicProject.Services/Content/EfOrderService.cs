using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;
using Microsoft.EntityFrameworkCore;
using MusicProject.ViewModels.Content;
using Microsoft.AspNetCore.Http;

namespace MusicProject.Services.Content
{
  public class EfOrderService : IOrderService
  {
    IUnitOfWork _uow;
    private readonly IUploadService _uploadService;

    readonly DbSet<Order> _Order;
    public EfOrderService(
      IUnitOfWork uow,
      IUploadService uploadService
      )
    {
      _uow = uow;
      _uow.CheckArgumentIsNull(nameof(_uow));
      _uploadService = uploadService;
      _Order = _uow.Set<Order>();
    }
    public void AddNewOrder(Order order, IFormFile file)
    {
      if (file!=null)
      {
          long fileSize = 0;
      order.File = order.File = _uploadService.UploadFile(file, "/content/files/Order", ref fileSize, new[]
          {
        " application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/pdf",
        "application/x-compressed",
        "application/x-zip-compressed",
        "application/zip",
        "application/x-rar-compressed",
        "application/octet-stream"

      }); 
      }
   

      _Order.Add(order);
      _uow.SaveChanges();
    }

    public async Task<PagedOrderViewModel> GetPagedOrderAsync(int pageNumber, int pageSize, SortOrder sortOrder)
    {

      var offset = (pageSize * pageNumber) - pageSize;
      var query = sortOrder == SortOrder.Descending ? _Order.OrderByDescending(x => x.Id) : _Order.OrderBy(x => x.Id);

      return new PagedOrderViewModel
      {
        Paging =
        {
          TotalItems = await query.CountAsync().ConfigureAwait(false)
        },
        OrderItem = await query.Skip(offset).Take(pageSize).ToListAsync().ConfigureAwait(false)
      };
    }

    public void Delete(int id)
    {
      var itemToRemove = _Order.Find(id);
      if (itemToRemove != null)
      {
        _Order.Remove(itemToRemove);
        _uow.SaveChanges();
      }
    }
  }
}
