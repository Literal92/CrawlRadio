using System.Collections.Generic;

namespace MusicProject.ViewModels.Content
{
  public class VideoListViewModel
  {
    public List<ContentFileViewModel> SelectedVideos { get; set; }
    public List<ContentFileViewModel> PopularVideos { get; set; }
    public List<ContentFileViewModel> Videos { get; set; }
    public int CurrentPage { get; set; }
    public string TotalItems { get; set; }

  }
}