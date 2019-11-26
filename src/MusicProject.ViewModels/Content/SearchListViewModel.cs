using System.Collections.Generic;

namespace MusicProject.ViewModels.Content
{
    public class SearchListViewModel
    {
        public List<AlbumViewModel> Items { get; set; }
        public int CurrentPage { get; set; }
        public string Type { get; set; }
        public long TotalAlbum { get; set; }
        public long TotalArtist { get; set; }
        public long TotalStyle { get; set; }
        public long TotalVideo { get; set; }
        public long TotalMusic { get; set; }
    }
}