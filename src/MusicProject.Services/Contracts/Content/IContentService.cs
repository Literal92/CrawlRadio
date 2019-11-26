using Microsoft.AspNetCore.Http;
using MusicProject.ViewModels.Content;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MusicProject.Services.Contracts.Content
{
    public interface IContentService
    {
        void AddNewContent(Entities.Content.Content product, IFormFile photo, IFormFile photo2, IFormFile photo3, IFormFile[] files, IFormFile video, IFormFile pdf, IFormFile Mp364, IFormFile Mp3128, IFormFile Mp3320);
        IList<Entities.Content.Content> GetAllContents();
        IList<Entities.Content.Content> GetAllByType(int typeId);
        Task<IList<Entities.Content.Content>> GetAllByCategoryAsync(int categoryId);
        Task<IList<Entities.Content.Content>> GetAllByTypeAsync(int typeId);
        IList<Entities.Content.Content> GetTopByType(string typeId, int pageNumber, int pageSize, string title);
        Task<IList<Entities.Content.Content>> GetByListIdAsync(List<int> id);
        int GetCount(int typeId, string title);

        IList<Entities.Content.Content> GetTopByTypeAndSkip(int typeId, int top, int skip, int catId);
        Entities.Content.Content FindById(int id);
        Entities.Content.Content GetByTitle(string title);

        void UpdateContent(Entities.Content.Content product, IFormFile photo, IFormFile photo2, IFormFile photo3, IFormFile[] files, IFormFile video, IFormFile pdf, IFormFile Mp364, IFormFile Mp3128, IFormFile Mp3320);
        Task<PagedContentItemsViewModel> GetPagedContentsListAsync(
          int pageNumber, int recordsPerPage,
          string sortByField, SortOrder sortOrder,
          bool showAllUsers);

        Task<Entities.Content.Content> LikeContent(int id);
        Task<Entities.Content.Content> VisitContent(int id);
        Task<PagedContentItemsViewModel> GetPagedContentsAsync(
           int pageNumber,
           int pageSize,
           SortOrder sortOrder,
           string type, bool? archive,
           int categoryId,
           string from,
           string to,
           string title,
           int artisId = 0,
            int styleId = 0,
            int musicId = 0

       );
        IList<Entities.Content.Content> GetTopInCat(int top);
        Task<Entities.Content.Content> FindByIdAsync(int id);
        Entities.Content.Content UpdateVisit(int id);
        void Delete(int id);
        Task<Tuple<string, string>> DownloadAlbum(int albumId, string type);
    }
}
