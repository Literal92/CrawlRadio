using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MusicProject.Entities.Content;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Contracts.Content
{
  public interface ITagService
  {
    IList<Tag> GetStartBy(string term, int top);
    IList<Tag> GetStartBy(string term, int top, string type);

    int AddNewTag(Entities.Content.Tag tag, IFormFile photo);
    IList<Tag> GetAllBySelected(string typeId);
    Task<IList<Entities.Content.Tag>> GetAllBySelectedAsync(string typeId);

    Task<IList<Entities.Content.Tag>> GetAllByTypeAsync(string typeId);
    void UpdateTag(Entities.Content.Tag tag, IFormFile photo);

    IList<Tag> GetByTitle(string title, string type);
    IList<Tag> GetPagedTags(int pageNumber, int pageSize, string type, string title);
    Task<PagedTagItemsViewModel> GetPagedTagsAsync(
      int pageNumber,
      int pageSize,
      SortOrder sortOrder,
      string type,
      string title, string orderBy,
      bool? isPublish, bool? isSelected
    );
        IList<Tag> GetStartBy(int pageNumber, int pageSize,
           string type, string title);

    Task<IList<Tag>> GetSortedAsync(string type, int top);
    Task<Tag> FindByIdAsync(int id);
    Task<Tag> LikeTag(int id);
    int GetCount(string typeId, bool? isPublish = null);
    int GetCount(string typeId, string term);
    void Delete(int id);

  }
}