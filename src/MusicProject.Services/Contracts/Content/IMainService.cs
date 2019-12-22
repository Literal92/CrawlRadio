using Microsoft.AspNetCore.Http;
using MusicProject.Entities.Content;
using MusicProject.ViewModels.Content;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MusicProject.Services.Contracts.Content
{
    public interface IMainService
    {
        IList<Main> GetAllContents();
        void Delete(int id);
        Task<bool> CheckDuplicate(string term, string Subterm);
        Task startCrawlerasync();
        Task Create(Main items);
        Task<PagedContentItemsViewModel> GetPagedContentsAsync(
            int pageNumber,
            int pageSize,
            SortOrder sortOrder,
            string type,
             bool? archive,
            int categoryId,
            string from,
            string to,
            string title,
            int artisId = 0,
            int styleId = 0,
            int musicId = 0

           );
    }
}
