using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MusicProject.Entities.Content;
using Microsoft.AspNetCore.Http;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Contracts.Content
{
  public interface IPodcastService
  {
    int AddNewPodcast(Podcast podcast, IFormFile photo, IFormFile video);
    IList<Podcast> GetAllPodcats(int type);

    IList<Podcast> GetTopByType(string typeId, int pageNumber, int pageSize,string title);


   Task<IList<Podcast>> GetTopByTypeAsync(string typeId, int pageNumber, int pageSize,int? tagId);

    Task<PagedPodcastViewModel> GetPagedPodcastAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type ,bool? isArchive,string title,int tagId);
    Task<Podcast> FindByIdAsync(int id);
    void UpdatePodcast(Podcast podcast, IFormFile photo, IFormFile video);
    void Delete(int id);
  }
}
