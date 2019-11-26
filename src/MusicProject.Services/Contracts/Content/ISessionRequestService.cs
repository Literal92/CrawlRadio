using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MusicProject.Entities.Content;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Contracts.Content
{
  public interface ISessionRequestService
  {
    void AddNewSessionRequest(SessionRequest sessionRequest);
    Task<PagedSessionRequestViewModel> GetPagedSessionRequestAsync(int pageNumber, int pageSize, SortOrder sortOrder);
    void Delete(int id);

  }
}
