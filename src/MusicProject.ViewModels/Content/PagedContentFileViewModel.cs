using System.Collections.Generic;
using cloudscribe.Web.Pagination;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
  public class PagedContentFileViewModel
  {
    public PagedContentFileViewModel()
    {
      Paging = new PaginationSettings();
    }
    public List<ContentFile> ContentFileItem { get; set; }
    public PaginationSettings Paging { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
   
  }
}
