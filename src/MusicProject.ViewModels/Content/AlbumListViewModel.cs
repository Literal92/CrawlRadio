using System.Collections.Generic;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
  public class AlbumListViewModel
  {
    public List<AlbumViewModel> SelectedAlbums { get; set; }
    public List<AlbumViewModel> PopularAlbums { get; set; }
    public List<AlbumViewModel> Albums { get; set; }
   

    public int CurrentPage { get; set; }
    public string TotalItems { get; set; }

  }
}