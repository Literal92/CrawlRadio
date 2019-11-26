using MusicProject.Entities.Content;
using MusicProject.ViewModels.Content;
using System.Collections.Generic;

namespace MusicProject.ViewModels.Comment
{
    public class CommentListViewModel
    {

        public List<Entities.Comment.Comment> Comments { get; set; }
        public List<CategoryComment> CategoryComments { get; set; }
        public Entities.Comment.Comment Comment { get; set; }
        public AlbumViewModel albumView { get; set; }
        public int CurrentPage { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }
        public int Type { get; set; }
        public string TotalItems { get; set; }
        public string UserName { get; set; }
        public int Id { get; set; }

    }
}