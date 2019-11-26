using System.Collections.Generic;
using cloudscribe.Web.Pagination;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
  public class PagedCategoryCommentViewModel
  {
    public PagedCategoryCommentViewModel()
    {
      Paging = new PaginationSettings();
    }
    public List<CategoryComment> CommentItem { get; set; }
    public PaginationSettings Paging { get; set; }
    public int? TypeId { get; set; }
    public int? CategoryId { get; set; }
  }
}
