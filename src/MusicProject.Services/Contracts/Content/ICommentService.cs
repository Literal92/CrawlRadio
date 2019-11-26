using MusicProject.Entities.Comment;
using MusicProject.ViewModels.Comment;
using MusicProject.ViewModels.Content;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MusicProject.Services.Contracts.Content
{
    public interface ICommentService
    {
        Task<PagedCommentListViewModel> GetPagedCommentListAsync(int pageNumber, int pageSize, SortOrder sortOrder, int TypeId, string name, string body, bool? ispublished, string fromTime, string toTime);
        Task UpdatePublishedAsync(int commentId, bool commentState);
        Task<Comment> GetCommentAsync(int id);
        Task DeleteCommentAsync(int id);
        Task<PagedCommentViewModel> GetPagedCommentAsync(int pageNumber, int pageSize, SortOrder sortOrder, int? type, int? typeId);
        Task<Comment> LikeComment(int id);
        Task<Comment> InverseLikeComment(int id);
        Task<Comment> InverseDisLikeComment(int id);
        Task<Comment> DisLikeComment(int id);
        Task<CommentListViewModel> GetCommentDetailsAsync(int id);
    }
    
}
