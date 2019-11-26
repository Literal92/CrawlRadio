using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts.Content;
using Microsoft.EntityFrameworkCore;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Content
{
  public class EfSessionRequestService : ISessionRequestService
  {
    IUnitOfWork _uow;
    readonly DbSet<SessionRequest> _SessionRequest;
    public EfSessionRequestService(IUnitOfWork uow)
    {
      _uow = uow;
      _uow.CheckArgumentIsNull(nameof(_uow));

      _SessionRequest = _uow.Set<SessionRequest>();
    }
    public void AddNewSessionRequest(SessionRequest sessionRequest)
    {
      _SessionRequest.Add(sessionRequest);
      _uow.SaveChanges();
    }

    public async Task<PagedSessionRequestViewModel> GetPagedSessionRequestAsync(int pageNumber, int pageSize, SortOrder sortOrder)
    {

      var offset = (pageSize * pageNumber) - pageSize;
      var query = sortOrder == SortOrder.Descending ? _SessionRequest.OrderByDescending(x => x.Id) : _SessionRequest.OrderBy(x => x.Id);

      return new PagedSessionRequestViewModel
      {
        Paging =
        {
          TotalItems = await query.CountAsync().ConfigureAwait(false)
        },
        SessionRequestItem = await query.Skip(offset).Take(pageSize).ToListAsync().ConfigureAwait(false)
      };
    }

    public void Delete(int id)
    {
      var itemToRemove = _SessionRequest.Find(id);
      if (itemToRemove != null)
      {
        _SessionRequest.Remove(itemToRemove);
        _uow.SaveChanges();
      }
    }
  }
}
