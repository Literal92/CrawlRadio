using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MusicProject.Entities.Content;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Contracts.Content
{
  public interface IContentFileService
  {
    void Delete(int id);
    int AddNewContentFile(ContentFile contentFile, IFormFile photo, IFormFile video, IFormFile video2, IFormFile video3);

    Task<PagedContentFileViewModel> GetPagedContentFileAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type, string title, int? contentId,bool? isSelected=null,bool? isPublish=null,string orderBy=null);
    Task<IList<ContentFile>> GetTopByTypeAsync(string typeId, int pageNumber, int pageSize,string term,string orderBy);
    Task<IList<ContentFile>> GetByCatIdAsync(int catId);
    Task<IList<ContentFile>> GetByTagIdAsync(int tagId);
    Task<IList<ContentFile>> GetSimilarAsync(string tagId);
    Task<ContentFile> FindByIdAsync(int id, bool? visited=false);
    Task<ContentFile> LikeContentFile(int id);
    void UpdateContentFile(ContentFile contentFile, IFormFile photo, IFormFile video, IFormFile video2, IFormFile video3);
    int GetCount(string typeId, bool? isPublish = null);
    Task<IList<ContentFile>> GetByListIdAsync(List<int> id);

  }
}