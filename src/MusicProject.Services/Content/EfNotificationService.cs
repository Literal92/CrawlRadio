using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MusicProject.Common;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Content
{
  public class EfNotificationService : INotificationService
  {
    IUnitOfWork _uow;
    readonly DbSet<Notification> _Notifications;
    private readonly IUploadService _uploadService;
    private readonly IHostingEnvironment _hostingEnvironment;


    public EfNotificationService(
      IUnitOfWork uow,
      IUploadService uploadService,
      IHostingEnvironment hostingEnvironment


    )
    {
      _uow = uow;
      _uow.CheckArgumentIsNull(nameof(_uow));
      _uploadService = uploadService;
      _Notifications = _uow.Set<Notification>();
      _hostingEnvironment = hostingEnvironment;
    }


    public int AddNewNotification(Notification notification)
    {
   
      var id = _Notifications.Add(notification);
      _uow.SaveChanges();
      return id.Entity.Id;
    }

   

    public IList<Notification> GetAllNotifications(string type)
    {
      return _Notifications.Where(p => p.Type == type).OrderByDescending(p => p.Id).ToList();
    }
    

    public async Task<PagedNotificationViewModel> GetPagedNotificationAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type,
      string orderBy = "date"
      )

    {
      var offset = (pageSize * pageNumber) - pageSize;
      var query = string.IsNullOrWhiteSpace(type) ?
        _Notifications :
        _Notifications.Where(l => l.Type == type);

      switch (orderBy)
      {
        case "date":
          {
            query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
            break;
          }
        case "alphabet":
          {
            query = query.OrderBy(x => x.Title);
            break;
          }
      }

      return new PagedNotificationViewModel
      {
        Paging =
        {
          TotalItems = await query.CountAsync().ConfigureAwait(false)
        },
        NotificationItem = await query.Skip(offset).Take(pageSize).ToListAsync().ConfigureAwait(false)

      };
    }




    public async Task<Notification> FindByIdAsync(int id)
    {
      var notification = await _Notifications.SingleOrDefaultAsync(p => p.Id == id);

      return notification;
    }




    public void UpdateNotification(Notification notification)
    {

      _Notifications.Update(notification);
      _uow.SaveChanges();
    }



    public void Delete(int id)
    {
      var itemToRemove = _Notifications.Find(id);
      if (itemToRemove != null)
      {
        _Notifications.Remove(itemToRemove);
        _uow.SaveChanges();
      }
    }
  }
}
