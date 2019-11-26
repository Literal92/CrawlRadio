using System.Collections.Generic;
using MusicProject.Entities.Identity;
using cloudscribe.Web.Pagination;

namespace MusicProject.ViewModels.Content
{
  public class PagedTagItemsViewModel
  {
    public PagedTagItemsViewModel()
    {
      Paging = new PaginationSettings();
    }
    public List<Entities.Content.Tag> Tags { get; set; }
  
    public string Type { get; set; } = string.Empty;
 
    public string Title { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string OrderBy { get; set; } = string.Empty;
    public bool? IsPublish { get; set; } =null;
    public bool? IsSelected { get; set; } =null;

    public PaginationSettings Paging { get; set; }
  }
}
