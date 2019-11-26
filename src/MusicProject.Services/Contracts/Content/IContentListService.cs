using Microsoft.AspNetCore.Http;
using MusicProject.Entities.Content;
using MusicProject.ViewModels.Content;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MusicProject.Services.Contracts.Content
{
    public interface IContentListService
    {
        int AddNewContentList(ContentList contentList, IFormFile photo);
        IList<ContentList> GetAllContentList(string type);
        Task<PagedContentListViewModel> GetPagedContentListAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type, bool? archive, string title, int id, int tagId, string orderBy = "date");
        Task<ContentList> FindByIdAsync(int id, bool visited = false);
        void UpdateContentList(ContentList contentList, IFormFile photo);
        void Delete(int id);
        bool CheckExistInContentList(int id, int listId);
        IList<ContentList> GetTopByType(string type, bool? isPublish);
        Task<IList<Entities.Content.Content>> GetAllContentByPLId(int pLId);
        Task<Tuple<string, string>> DownloadPlayList(int pLId, string type);
        Task<IList<ContentList>> GetRelated(int? artistId, int? tagId, int? styleId, int? music, int top, int playListId);
        Task<ContentList> LikePlayList(int id);
        IList<Tag> GetCategoryByPLId(int Id);
        IList<ContentList> GetTopByType(string typeId, int pageNumber, int pageSize, string title);
        int GetCount(string typeId, string title);
        Task<List<ContentListViewModel.Music>> GetMusicsFiles(int id);
    }
}
