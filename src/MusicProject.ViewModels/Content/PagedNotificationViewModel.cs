using System.Collections.Generic;
using cloudscribe.Web.Pagination;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
  public class PagedNotificationViewModel
  {
    public PagedNotificationViewModel()
    {
      Paging = new PaginationSettings();
    }
    public List<Notification> NotificationItem { get; set; }
    public PaginationSettings Paging { get; set; }
    public string Type { get; set; }
  }
}
