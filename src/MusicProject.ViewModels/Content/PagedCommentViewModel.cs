using cloudscribe.Web.Pagination;
using System.Collections.Generic;

namespace MusicProject.ViewModels.Content
{
    public class PagedCommentViewModel
    {
        public PagedCommentViewModel()
        {
            Paging = new PaginationSettings();
        }
        public List<Entities.Comment.Comment> CommentItem { get; set; }
        public PaginationSettings Paging { get; set; }
        public int? TypeId { get; set; }
        public int? Type { get; set; }
        public int UserId { get; set; }
    }

}
