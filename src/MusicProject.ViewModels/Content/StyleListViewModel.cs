using System.Collections.Generic;

namespace MusicProject.ViewModels.Content
{
  public class StyleListViewModel
  {
    public List<StyleViewModel> SelectedStyles { get; set; }
    public List<StyleViewModel> PopularStyles { get; set; }
    public List<StyleViewModel> Styles { get; set; }
    public int CurrentPage { get; set; }
    public string TotalItems { get; set; }

  }
}