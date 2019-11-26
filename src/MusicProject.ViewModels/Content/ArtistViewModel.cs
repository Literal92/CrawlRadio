using System.Collections.Generic;

namespace MusicProject.ViewModels.Content
{
  public class ArtistViewModel
  {

    public List<TagViewModel> SelectedArtist { get; set; }
    public List<TagViewModel> PopularArtist { get; set; }
    public List<TagViewModel> Artists { get; set; }
    public int CurrentPage { get; set; }
    public string TotalItems { get; set; }

  }
}