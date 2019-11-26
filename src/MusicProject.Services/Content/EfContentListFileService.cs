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
    public class EfContentListFileService : IContentListFileService
    {
        IUnitOfWork _uow;
        readonly DbSet<ContentListFile> _contentListFile;
        private readonly IUploadService _uploadService;

        public EfContentListFileService(
          IUnitOfWork uow,
          IUploadService uploadService
        )
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));
            _uploadService = uploadService;
            _contentListFile = _uow.Set<ContentListFile>();
        }

        public IList<ContentListFile> GetAllContentList(string type)
        {
            return _contentListFile.Where(p => p.Type == type).ToList();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortOrder"></param>
        /// <param name="type"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<PagedContentListFileViewModel> GetPagedContentListFileAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type, string title, int? id)
        {
            var offset = (pageSize * pageNumber) - pageSize;

            var query = string.IsNullOrWhiteSpace(type) ?
              _contentListFile :
              _contentListFile.Where(l => l.Type == type);

            query = string.IsNullOrEmpty(title) ? query :
              query.Where(l => l.Title.Contains(title));

            query = (id == null || id == 0) ? query :
              query.Where(l => l.ContentListId == id);

            query = query.OrderByDescending(x => x.Id);

            return new PagedContentListFileViewModel
            {
                Paging =
                {
                      TotalItems = await query.CountAsync().ConfigureAwait(false)
                },
                ContentLIstItem = await query.Skip(offset).Take(pageSize).Select(p => new ContentListFileViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Mp364 = p.Mp364,
                    Mp3128 = p.Mp3128,
                    Mp3320 = p.Mp3320,

                }).ToListAsync().ConfigureAwait(false)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentListFile"></param>
        /// <param name="photo"></param>
        public void UpdateContentList(ContentListFile contentListFile, IFormFile photo)
        {
            _contentListFile.Update(contentListFile);
            _uow.SaveChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var itemToRemove = _contentListFile.Find(id);
            if (itemToRemove != null)
            {
                _contentListFile.Remove(itemToRemove);
                _uow.SaveChanges();
            }
        }

        public void AddNewContentListFile(ContentListFile contentListFile, IFormFile mp364, IFormFile mp3128, IFormFile mp3320)
        {
            if (mp364 != null)
            {
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 300 };
                var resized = new string[1];
                fileName = _uploadService.UploadFile(
                  mp364, "/content/files/contentlistFile",
                  ref fileSize, new[] { "audio/mpeg", "audio/mp3" }
                );
                contentListFile.Mp364 = fileName;
            }
            if (mp3128 != null)
            {
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 300 };
                var resized = new string[1];
                fileName = _uploadService.UploadFile(
                  mp3128, "/content/files/contentlistFile",
                  ref fileSize, new[] { "audio/mpeg", "audio/mp3" }
                );
                contentListFile.Mp364 = fileName;
            }
            if (mp3320 != null)
            {
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 300 };
                var resized = new string[1];
                fileName = _uploadService.UploadFile(
                  mp3320, "/content/files/contentlistFile",
                  ref fileSize, new[] { "audio/mpeg", "audio/mp3" }
                );
                contentListFile.Mp364 = fileName;
            }
            _contentListFile.Add(contentListFile);
            _uow.SaveChanges();
        }

        public async Task<ContentListFile> FindByIdAsync(int id, bool? visited = false)
        {
            return await _contentListFile.Include(p=>p.ContentList).SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<ContentListFile> LikeContentListFile(int id)
        {
            var contentListFile = await _contentListFile.FindAsync(id);
            contentListFile.LikeCount = contentListFile.LikeCount + 1;
            _uow.SaveChanges();
            return contentListFile;
        }

        public void UpdateContentListFile(ContentListFile contentListFile, IFormFile mp364, IFormFile mp3128, IFormFile mp3320)
        {
            if (mp364 != null)
            {
                var allowed = new[] { "audio/mpeg", "audio/mp3" };
                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  mp364, "/content/files/contentlistFile/", ref fileSize, allowed);
                contentListFile.Mp364 = fileName;
            }
            if (mp3128 != null)
            {
                var allowed = new[] { "audio/mpeg", "audio/mp3" };
                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  mp3128, "/content/files/contentlistFile/", ref fileSize, allowed);
                contentListFile.Mp3128 = fileName;
            }
            if (mp3320 != null)
            {
                var allowed = new[] { "audio/mpeg", "audio/mp3" };
                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  mp3320, "/content/files/contentlistFile/", ref fileSize, allowed);
                contentListFile.Mp3320 = fileName;
            }
            _contentListFile.Update(contentListFile);
            _uow.SaveChanges();
        }

        public async Task<IList<ContentListFile>> GetAllByContentList(int id)
        {
            return await _contentListFile.Include(a=>a.ContentList).Where(p => p.ContentListId == id).ToListAsync();
        }

        public async Task<ContentListFile> LikeFilePlayList(int id)
        {
            var contentListFile = await FindByIdAsync(id);
            contentListFile.LikeCount++;
             await _uow.SaveChangesAsync();
            return contentListFile;
        }
    }
}
