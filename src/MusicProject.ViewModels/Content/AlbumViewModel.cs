using MusicProject.Entities.Content;
using MusicProject.Entities.Identity;
using MusicProject.ViewModels.Comment;
using System.Collections.Generic;


namespace MusicProject.ViewModels.Content
{
    public class AlbumViewModel
    {
      
        public int Id;
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string ArtistPic { get; set; }
        public string Description { get; set; }
        public string RegisterDate { get; set; }
        public string RegisterTime { get; set; }
        public string ContentText { get; set; }
        public string TotalItems { get; set; }
        public string ZipMp364 { get; set; }
        public string ZipMp3128 { get; set; }
        public string ZipMp3320 { get; set; }

        public ICollection<CategoryTag> CategoryTags { get; set; }
        public ICollection<CategoryTag> ArtistTags { get; set; }
        public string Artist { get; set; }
        public int VisitCount { get; set; }
        public int LikeCount { get; set; }
        public IList<Entities.Content.Tag> Styles { get; set; }
        public IList<Entities.Content.Tag> Musics { get; set; }
        public IList<Entities.Content.Tag> Tags { get; set; }
        public IList<Entities.Content.Tag> Artists { get; set; }

        public List<CategoryComment> CategoryComments { get; set; }
        public IList<Entities.Content.Content> Contents { get; set; }
        public IList<Entities.Content.ContentFile> Videos { get; set; }
        public IList<Entities.Content.Category> Suggested { get; set; }
        public IEnumerable<PagedCommentViewModel> Comments { get; set; }

    }
}
