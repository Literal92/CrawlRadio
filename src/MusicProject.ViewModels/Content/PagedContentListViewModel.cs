using System.Collections.Generic;
using cloudscribe.Web.Pagination;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
  public class PagedContentListViewModel
  {
    public PagedContentListViewModel()
    {
      Paging = new PaginationSettings();
    }
    public List<ContentListViewModel> ContentLIstItem { get; set; }
    public PaginationSettings Paging { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public int Id { get; set; }
  }
}
