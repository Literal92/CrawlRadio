using System;
using System.Collections.Generic;
using System.Text;

namespace MusicProject.ViewModels.Api
{
    public class AppHomeViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Pic { get; set; }
        public string Thumbnail { get; set; }
        public string Video { get; set; }
        public string Video720 { get; set; }
        public string Video480 { get; set; }
        public string Type { get; set; }
        public string Kind { get; set; }
        public string OrderBy { get; set; }
        public string TypeName { get; set; }
        public List<ArtistViewModel> Artist { get; set; }
        public string VideoArtist { get; set; }
        public string ShareLink { get; set; }
        public int LikeCount { get; set; }
        public string Link { get; set; }
        public int VisitCount { get; set; }
        public int Rate { get; set; }
    }
    public class ArtistViewModel
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
    }
}
