using System.Collections.Generic;
using cloudscribe.Web.Pagination;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
  public class PagedPodcastViewModel
  {
    public PagedPodcastViewModel()
    {
      Paging = new PaginationSettings();
    }
    public List<Podcast> PodcastItem { get; set; }
    public PaginationSettings Paging { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public string IsPublic { get; set; }
    public string TagId { get; set; }
  }
}
