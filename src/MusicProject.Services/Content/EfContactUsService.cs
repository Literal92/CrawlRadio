using System;
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
  public class EfContactUsService : IContactUsService
  {
    IUnitOfWork _uow;
    readonly DbSet<ContactUs> _ContactUs;
    public EfContactUsService(IUnitOfWork uow)
    {
      _uow = uow;
      _uow.CheckArgumentIsNull(nameof(_uow));

      _ContactUs = _uow.Set<ContactUs>();
    }
    public void AddNewContactUs(ContactUs contactUs)
    {
      _ContactUs.Add(contactUs);
      _uow.SaveChanges();

    }
    public void Delete(int id)
    {
      var itemToRemove = _ContactUs.Find(id);
      if (itemToRemove != null)
      {
        _ContactUs.Remove(itemToRemove);
        _uow.SaveChanges();
      }
    }

    public async Task<PagedContactUsViewModel> GetPagedContactUsAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type)

    {
      var offset = (pageSize * pageNumber) - pageSize;

      var query = string.IsNullOrWhiteSpace(type) ?
        _ContactUs :
        _ContactUs.Where(l => l.Type == type);



      query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);

      return new PagedContactUsViewModel
      {
        Paging =
        {
          TotalItems = await query.CountAsync().ConfigureAwait(false)
        },
        ContactUsItem = await query.Skip(offset).Take(pageSize).ToListAsync().ConfigureAwait(false),
        Type = type
      };
    }

  }
}
