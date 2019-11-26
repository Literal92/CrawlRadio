using System.Collections.Generic;

namespace MusicProject.ViewModels.Content
{
  public class VideoDetailViewModel
  {
    public List<ContentFileViewModel> ArtistVideos { get; set; }
    public List<ContentFileViewModel> SimilarVideos { get; set; }
    public ContentFileViewModel Video { get; set; }
  }
}