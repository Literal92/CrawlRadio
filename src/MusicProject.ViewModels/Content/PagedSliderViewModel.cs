using System.Collections.Generic;
using cloudscribe.Web.Pagination;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
  public class PagedSliderViewModel
  {
    public PagedSliderViewModel()
    {
      Paging = new PaginationSettings();
    }
    public List<Slider> SliderItem { get; set; }
    public PaginationSettings Paging { get; set; }
    public int Type { get; set; }
    public string Title { get; set; }
    public string State { get; set; }
  }
}
