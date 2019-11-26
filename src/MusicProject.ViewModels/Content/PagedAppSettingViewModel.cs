using System.Collections.Generic;
using cloudscribe.Web.Pagination;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
  public class PagedAppSettingViewModel
  {
    public PagedAppSettingViewModel()
    {
      Paging = new PaginationSettings();
    }
    public List<AppSetting> AppSettingItem { get; set; }
    public PaginationSettings Paging { get; set; }
    public int Type { get; set; }
    public string Title { get; set; }
  }
}
