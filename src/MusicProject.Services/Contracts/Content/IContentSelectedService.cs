using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MusicProject.Entities.Content;
using Microsoft.AspNetCore.Http;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Contracts.Content
{
    public interface IContentSelectedService
    {

        void AddNewContentSelected(ContentSelected contentSelected);
        IList<ContentSelected> GetAllContentSelected(string type, int listId);
        Task<IList<ContentSelected>> GetAllContentSelectedAsync(string type, int listId);

        Task<PagedContentListViewModel> GetPagedContentSelectedAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type);
        //Task<ContentSelected> FindByIdAsync(int id);
        Task<ContentSelected> FindByIdAsync(int id);

        void UpdateContentSelected(ContentSelected contentList);
        void Delete(int id);
        void Delete(int id, int listidId);

    }
}
