using System.Collections.Generic;

namespace MusicProject.ViewModels.Content
{
  public class ArtistDetailViewModel
  {
    public TagViewModel Artist { get; set; }
    public List<AlbumViewModel> Albums { get; set; }
    public int CurrentPage { get; set; }
    public string TotalItems { get; set; }

  }
}