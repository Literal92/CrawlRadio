using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MusicProject.Entities.Content;
using Microsoft.AspNetCore.Http;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Contracts.Content
{
  public interface ICategoryService
  {
    int AddNewCategory(Category category, IFormFile photo, IFormFile mp364, IFormFile mp3128, IFormFile mp3320);
    IList<Category> GetAllCategories(int type);

    IList<Category> GetTopByType(string typeId, int pageNumber, int pageSize, string title, bool? isPublish, bool? isSelected = null);
    IList<Category> GetForSearch(string typeId, int pageNumber, int pageSize, string title, bool? isPublish);
    int GetCount(string typeId, string title, bool? isPublish=null, int? tagId = null);
    IList<Category> GetTopByLike(string typeId, int pageNumber, int pageSize, bool? isPublish);


    Task<IList<Category>> GetTopByTypeAsync(string typeId, int pageNumber, int pageSize, int? tagId, bool? isPublish);

    Task<IList<Category>> GetByListIdAsync(List<int> id);
    Task<Category> LikeCategory(int id);

    Task<IList<Category>> GetTopByLikeAsync(string typeId, int pageNumber, int pageSize);
    Task<IList<Category>> GetRelated(int? artistId, int? tagId, int? styleId,int? music, int top,int categoryId);

    Category GetByTitle(string title);
    IList<Category> GetIndentedCategory(int type);
    Task<PagedCategoryViewModel> GetPagedCategoryAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type, bool? isArchive, string title, int tagId, string orderBy = "date");
    Task<Category> FindByIdAsync(int id, bool visited = false);
    void UpdateCategory(Category category, IFormFile photo, IFormFile mp364, IFormFile mp3128, IFormFile mp3320);
    void ResizeCategory(Category category);
    void Delete(int id);


  }
}
