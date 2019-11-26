using System.Collections.Generic;
using MusicProject.Entities.Identity;
using cloudscribe.Web.Pagination;

namespace MusicProject.ViewModels.Content
{
  public class PagedSelectedContentViewModel
  {
    public PagedSelectedContentViewModel()
    {
      Paging = new PaginationSettings();
    }

    public List<Entities.Content.Content> Contents { get; set; }
    public string Type { get; set; } = string.Empty;
    public string IsArchive { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public string CotentText { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;

    public PaginationSettings Paging { get; set; }
  }
}
