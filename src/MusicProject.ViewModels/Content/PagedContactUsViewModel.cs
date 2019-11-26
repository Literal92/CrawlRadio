using System.Collections.Generic;
using cloudscribe.Web.Pagination;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
  public class PagedContactUsViewModel
  {
    public PagedContactUsViewModel()
    {
      Paging = new PaginationSettings();
    }
    public List<ContactUs> ContactUsItem { get; set; }
    public PaginationSettings Paging { get; set; }
    public string Type { get; set; }
  }
}
