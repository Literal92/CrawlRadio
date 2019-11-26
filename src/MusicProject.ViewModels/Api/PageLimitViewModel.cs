using System;
using System.Collections.Generic;
using System.Text;

namespace MusicProject.ViewModels.Api
{
  public class PageLimitViewModel
  {
    public string OrderBy { get; set; }
    public string Kind { get; set; }
    public string Action { get; set; }
    public bool FirstVisit { get; set; }
    public string Term { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int Id { get; set; }

  }
}
