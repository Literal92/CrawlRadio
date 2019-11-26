using System.Collections.Generic;
using cloudscribe.Web.Pagination;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
  public class PagedSessionRequestViewModel
  {
    public PagedSessionRequestViewModel()
    {
      Paging = new PaginationSettings();
    }
    public List<SessionRequest> SessionRequestItem { get; set; }
    public PaginationSettings Paging { get; set; }
  }
}
