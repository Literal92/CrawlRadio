using System.Collections.Generic;

namespace MusicProject.ViewModels.Content
{
  public class HomeViewModel
  {
    public List<CategoryViewModel> LastMusic { get; set; }
    public List<TagViewModel> Artist { get; set; }
    public List<CategoryViewModel> MoreLikes { get; set; }
    public List<TagViewModel> StyleTag { get; set; }
    public List<TagViewModel> MusicTag { get; set; }
    public List<ContentFileViewModel> LastVideo { get; set; }
    public List<SliderViewModel> Slider { get; set; }

    public SessionRrequestViewModel SessionRrequestViewModel { get; set; }
  }
}