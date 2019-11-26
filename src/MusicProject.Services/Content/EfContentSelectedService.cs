using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Content
{
    public class EfContentSelectedService : IContentSelectedService
    {
        IUnitOfWork _uow;
        readonly DbSet<ContentSelected> _ContentList;
        private readonly IUploadService _uploadService;

        public EfContentSelectedService(
          IUnitOfWork uow,
          IUploadService uploadService
        )
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));
            _uploadService = uploadService;
            _ContentList = _uow.Set<ContentSelected>();
        }

        public void Delete(int id)
        {
            var itemToRemove = _ContentList.Find(id);
            if (itemToRemove != null)
            {
                _ContentList.Remove(itemToRemove);
                _uow.SaveChanges();
            }
        }

        public void Delete(int id, int listId)
        {
            var itemToRemove = _ContentList.FirstOrDefault(p => p.ContentListId == listId && p.ContentId == id);
            if (itemToRemove != null)
            {
                _ContentList.Remove(itemToRemove);
                _uow.SaveChanges();
            }
        }

        public void AddNewContentSelected(ContentSelected contentSelected)
        {
            _ContentList.Add(contentSelected);

        }

        public IList<ContentSelected> GetAllContentSelected(string type, int listId)
        {
            return _ContentList.Where(p => p.Type == type && p.ContentListId == listId).Include(a => a.Content).ThenInclude(cat => cat.Category).ToList();
        }

        public Task<PagedContentListViewModel> GetPagedContentSelectedAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type)
        {
            throw new NotImplementedException();
        }

        public void UpdateContentSelected(ContentSelected contentList)
        {
            _ContentList.Update(contentList);
            _uow.SaveChanges();
        }

        public Task<ContentSelected> FindByIdAsync(int id)
        {
            return _ContentList.FindAsync(id);
        }


        public async Task<IList<ContentSelected>> GetAllContentSelectedAsync(string type, int listId)
        {
            return await _ContentList.Where(p => p.Type == type && p.ContentListId == listId).Include(a => a.Content).ToListAsync();
        }


    }
}
