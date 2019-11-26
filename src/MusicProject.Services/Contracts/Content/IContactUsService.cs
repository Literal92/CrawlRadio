using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MusicProject.Entities.Content;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Contracts.Content
{
  public interface IContactUsService
  {
    void AddNewContactUs(ContactUs contraContactUs);

    Task<PagedContactUsViewModel> GetPagedContactUsAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type);
    void Delete(int id);
  }
}
