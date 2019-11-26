using System.Collections.Generic;
using cloudscribe.Web.Pagination;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
  public class PagedOrderViewModel
  {
    public PagedOrderViewModel()
    {
      Paging = new PaginationSettings();
    }
    public List<Order> OrderItem { get; set; }
    public PaginationSettings Paging { get; set; }
  }
}
