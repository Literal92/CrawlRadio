using MusicProject.Entities.Comment;
using MusicProject.ViewModels.Content;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MusicProject.Services.Contracts.Content
{
    public interface ICategoryCommentService
    {
        Task AddNewCommentAsync(Comment comment);
            //(int catId, string body, int? userId, int? parentId);
        Task<PagedCategoryCommentViewModel> GetPagedCommentAsync(
          int pageNumber,
          int pageSize,
          SortOrder sortOrder,
          int? type, int? categoryId,
          bool? isPublish = null, string orderBy = "date", int? parent = null);

        Task<int> GetCountAsync(
          int? type, int? categoryId,
          bool? isPublish = null, string orderBy = "date");
        void Delete(int id);
    }
}
