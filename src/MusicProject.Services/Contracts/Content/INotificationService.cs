using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MusicProject.Entities.Content;
using Microsoft.AspNetCore.Http;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Contracts.Content
{
  public interface INotificationService
  {
    int AddNewNotification(Notification category);
    IList<Notification> GetAllNotifications(string type);



   
     Task<PagedNotificationViewModel> GetPagedNotificationAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type, string orderBy = "date");
    Task<Notification> FindByIdAsync(int id);
    void UpdateNotification(Notification notification);
 
    void Delete(int id);


  }
}
