using Microsoft.AspNetCore.Http;
using MusicProject.Entities.Content;
using MusicProject.ViewModels.Content;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MusicProject.Services.Contracts.Content
{
    public interface IContentListFileService
    {
        void Delete(int id);
        void AddNewContentListFile(ContentListFile contentListFile, IFormFile mp364, IFormFile mp3128, IFormFile mp3320);
        Task<PagedContentListFileViewModel> GetPagedContentListFileAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type, string title, int? contentListId);
        Task<ContentListFile> FindByIdAsync(int id, bool? visited = false);
        Task<ContentListFile> LikeContentListFile(int id);
        void UpdateContentListFile(ContentListFile contentFile, IFormFile mp364, IFormFile mp3128,IFormFile mp3320);
        Task<IList<ContentListFile>> GetAllByContentList(int id);
        Task<ContentListFile> LikeFilePlayList(int id);
    }
}