using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;
using Microsoft.EntityFrameworkCore;
using MusicProject.Common;
using MusicProject.ViewModels.Content;


namespace MusicProject.Services.Content
{
    public class EfContentFileService : IContentFileService
    {
        IUnitOfWork _uow;
        private readonly IUploadService _uploadService;
        readonly DbSet<ContentFile> _ContentFiles;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ICategoryService _categoryService;
        private readonly IContentService _contentService;

        public EfContentFileService(
          IUnitOfWork uow,
          IUploadService uploadService,
          IHostingEnvironment hostingEnvironment,
          ICategoryService categoryService,
          IContentService contentService


          )
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));
            _uploadService = uploadService;
            _ContentFiles = _uow.Set<ContentFile>();
            _hostingEnvironment = hostingEnvironment;
            _categoryService = categoryService;
            _contentService = contentService;

        }


        public void Delete(int id)
        {
            var file = _ContentFiles.Find(id);
            if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + file.FileName))
                File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + file.FileName);

            if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + file.FileName2))
                File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + file.FileName2);

            if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + file.FileName3))
                File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + file.FileName3);

            _ContentFiles.Remove(file);

            _uow.SaveChanges();
        }

        public async Task<ContentFile> FindByIdAsync(int id, bool? visited = false)
        {
            var model = await _ContentFiles.Include(p => p.Content).ThenInclude(cat => cat.Category)
              .ThenInclude(cattag => cattag.CategoryTags).ThenInclude(tag => tag.Tag).SingleOrDefaultAsync(p => p.Id == id);
            if (visited == true && model != null)
            {
                model.VisitCount = model.VisitCount + 1;
                _uow.SaveChanges();
            }
            return model;
        }

        public void UpdateContentFile(ContentFile contentFile, IFormFile photo, IFormFile video, IFormFile video2, IFormFile video3)
        {

            var directoryName = _categoryService.FindByIdAsync(contentFile.Content.CategoryId).Result.Description.SafeFileName();

            if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/video"))
                Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/video");

            if (photo != null && photo.Length > 0)
            {
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 300 };
                var resized = new string[1];
                fileName = _uploadService.UploadPicResize(
                  photo,
                  "/content/files/album/" + directoryName + "/video",
                  2000,
                  ref fileSize,
                  size,
                  EnumC.Dimensions.Width,
                  ref resized
                );
                contentFile.Pic = directoryName + "/video/" + fileName;
                contentFile.Thumbnail = directoryName + "/video/" + size[0] + "/" + resized[0];
            }

            if (video != null)
            {

                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + contentFile.FileName))
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + contentFile.FileName);



                var allowed = new[] { "video/mp4", "image/x-png", "image/pjpeg", "image/jpeg", "image/gif" };
                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  video, "/content/files/album/" + directoryName + "/video",

                  ref fileSize,
                  allowed
                );
                contentFile.FileName = directoryName + "/video/" + fileName;
                contentFile.FileSize = fileSize;
            }


            if (video2 != null)
            {
                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + contentFile.FileName2))
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + contentFile.FileName2);

                var allowed = new[] { "video/mp4", "image/x-png", "image/pjpeg", "image/jpeg", "image/gif" };
                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  video2, "/content/files/album/" + directoryName + "/video",

                  ref fileSize,
                  allowed
                );
                contentFile.FileName2 = directoryName + "/video/" + fileName;
                contentFile.FileSize2 = fileSize;
            }
            if (video3 != null)
            {

                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + contentFile.FileName3))
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + contentFile.FileName3);

                var allowed = new[] { "video/mp4", "image/x-png", "image/pjpeg", "image/jpeg", "image/gif" };
                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  video3, "/content/files/album/" + directoryName + "/video",

                  ref fileSize,
                  allowed
                );
                contentFile.FileName3 = directoryName + "/video/" + fileName;
                contentFile.FileSize3 = fileSize;
            }

            _ContentFiles.Update(contentFile);
            _uow.SaveChanges();
        }


        public int AddNewContentFile(ContentFile contentFile, IFormFile photo, IFormFile video, IFormFile video2, IFormFile video3)
        {
            var directoryName = _contentService.FindByIdAsync(contentFile.ContentId).Result.Category.Description.SafeFileName();
            if (video != null)
            {
                if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/video"))
                    Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/video");
                var allowed = new[]
                {
          //"application/pdf", "application/x-pdf"
          "video/mpeg",
          "video/mp4"
        };
                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  video, "/content/files/album/" + directoryName + "/video",

                  ref fileSize,
                  allowed
                );
                contentFile.FileName = directoryName + "/video/" + fileName;
                contentFile.FileSize = fileSize;
            }

            if (video2 != null)
            {
                if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/video"))
                    Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/video");
                var allowed = new[]
                {
          //"application/pdf", "application/x-pdf"
          "video/mpeg",
          "video/mp4"
        };

                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  video2, "/content/files/album/" + directoryName + "/video",

                  ref fileSize,
                  allowed
                );
                contentFile.FileName2 = directoryName + "/video/" + fileName;
                contentFile.FileSize2 = fileSize;
            }

            if (video3 != null)
            {
                if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/video"))
                    Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/video");
                var allowed = new[]
                {
          //"application/pdf", "application/x-pdf"
          "video/mpeg",
          "video/mp4"
        };

                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  video3, "/content/files/album/" + directoryName + "/video",

                  ref fileSize,
                  allowed
                );
                contentFile.FileName3 = directoryName + "/video/" + fileName;
                contentFile.FileSize3 = fileSize;
            }

            contentFile.Ext = Path.GetExtension(_hostingEnvironment.WebRootPath + "/content/files/album/" + contentFile.FileName);

            if (photo != null)
            {
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 300 };
                var resized = new string[1];
                fileName = _uploadService.UploadPicResize(
                  photo, "/content/files/album/" + directoryName + "/video",
                  2000,
                  ref fileSize,
                  size,
                  EnumC.Dimensions.Width,
                  ref resized
                );
                contentFile.Pic = directoryName + "/video/" + fileName;
                contentFile.Thumbnail = directoryName + "/video/" + size[0] + "/" + resized[0];
            }


            var contentFileAd = _ContentFiles.Add(contentFile);
            _uow.SaveChanges();
            return contentFileAd.Entity.Id;


        }


        public async Task<IList<ContentFile>> GetByCatIdAsync(int catId)
        {
            return await _ContentFiles.Include(cat => cat.Content).Where(p => p.Content.CategoryId == catId).ToListAsync();
        }


        public async Task<IList<ContentFile>> GetByListIdAsync(List<int> id)
        {
            return await _ContentFiles.Where(x => id.Contains(x.Id)).ToListAsync();
        }


        public int GetCount(string typeId, bool? isPublish = null)
        {
            var query = _ContentFiles.Where(p => p.Type == typeId);
            query = isPublish == null ? query : query.Where(p => p.IsPublish == isPublish);
            return query.Count();
        }
        public async Task<IList<ContentFile>> GetByTagIdAsync(int tagId)
        {
            var param = new SqlParameter("@TagId", tagId.ToString());
            //    return await _ContentFiles.FromSql("GetArtistVideo @TagId", tagId).ToListAsync();
            return await _ContentFiles.FromSql("SELECT [dbo].[ContentFiles]. *  FROM [dbo].[ContentFiles]," + "  [dbo].Contents,[dbo].Categorys," + "[dbo].CategoryTags" + "  where [dbo].ContentFiles.ContentId=[dbo].Contents.Id" + " and [dbo].Contents.CategoryId=[dbo].Categorys.Id" + " and [dbo].CategoryTags.CategoryId=[dbo].Categorys.Id and" + " [dbo].CategoryTags.Type=\'artist\'    and [dbo].CategoryTags.TagId={0}", tagId).ToListAsync();
            //return await _ContentFiles.Include(cat=>cat.Content).ThenInclude(ca=>ca.Category).ThenInclude(ta=>ta.CategoryTags).Where(p=>p.Content.Category.CategoryTags.Where(a=))

            //  ToListAsync();
        }




        public async Task<IList<ContentFile>> GetSimilarAsync(string tagId)
        {
            var param = new SqlParameter("@TagId", tagId.ToString());
            //    return await _ContentFiles.FromSql("GetArtistVideo @TagId", tagId).ToListAsync();
            return await _ContentFiles.FromSql("SELECT distinct top 6 [dbo].[ContentFiles]. *  FROM [dbo].[ContentFiles]," + "  [dbo].Contents,[dbo].Categorys," +
                                               "[dbo].CategoryTags  where [dbo].ContentFiles.ContentId=[dbo].Contents.Id" +
                                               " and [dbo].Contents.CategoryId=[dbo].Categorys.Id and [dbo].CategoryTags.CategoryId=[dbo].Categorys.Id and" + " [dbo].CategoryTags.TagId in (" + tagId + ")").ToListAsync();
            //return await _ContentFiles.Include(cat=>cat.Content).ThenInclude(ca=>ca.Category).ThenInclude(ta=>ta.CategoryTags).Where(p=>p.Content.Category.CategoryTags.Where(a=))

            //  ToListAsync();
        }



        public async Task<IList<ContentFile>> GetTopByTypeAsync(string typeId, int pageNumber, int pageSize, string term, string orderBy)
        {
            if (!string.IsNullOrEmpty(term))
            {
                term = " " + term.Trim() + " ";
            }
            var offset = (pageSize * pageNumber) - pageSize;

            var query = string.IsNullOrWhiteSpace(typeId) ?
              _ContentFiles :
              _ContentFiles.Where(l => l.Type == typeId);
            if (orderBy == "")
                query = query.OrderByDescending(x => x.Id);
            else if (orderBy == "like")
            {
                query = query.OrderByDescending(x => x.LikeCount);
            }


            query = string.IsNullOrWhiteSpace(term) ? query :
              query.Where(l => l.Title.Contains(term)
                               || l.ContentText.Contains(term)
                               || l.Description.Contains(term)
                               || l.FileName.Contains(term)
                               || l.Thumbnail.Contains(term)
                               );

            var result = await query.Skip(offset).Take(pageSize).Include(con => con.Content).ThenInclude(cat => cat.Category).ThenInclude(cattag => cattag.CategoryTags).ThenInclude(tag => tag.Tag)
              .ToListAsync();
            return result;
        }


        public async Task<PagedContentFileViewModel> GetPagedContentFileAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type, string title, int? contentId, bool? isSelected = null, bool? isPublish = null, string orderBy = null)

        {
            var offset = (pageSize * pageNumber) - pageSize;

            var query = string.IsNullOrWhiteSpace(type) ?
              _ContentFiles :
              _ContentFiles.Where(l => l.Type == type);
            if (!string.IsNullOrEmpty(title))
            {
                title = " " + title.Trim() + " ";
            }

            query = (contentId == 0 || contentId == null) ? query :
        query.Where(l => l.ContentId == contentId);


            query = string.IsNullOrEmpty(title) ? query :
              query.Where(l => l.Title.Contains(title));

            query = isSelected == null ? query :
              query.Where(l => l.IsSelected == isSelected);

            query = isPublish == null ? query :
              query.Where(l => l.IsPublish == isPublish);


            if (string.IsNullOrEmpty(orderBy) || orderBy == "date")
                query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
            else if (orderBy == "popular")
            {
                query = query.OrderByDescending(x => x.LikeCount);
            }
            else if (orderBy == "alphabet")
            {
                query = query.OrderBy(x => x.Title);
            }





            return new PagedContentFileViewModel
            {
                Paging =
        {
          TotalItems = await query.CountAsync().ConfigureAwait(false)
        },
                ContentFileItem = await query.Skip(offset).Take(pageSize).Include(con => con.Content).ThenInclude(cat => cat.Category).ThenInclude(cattag => cattag.CategoryTags).ThenInclude(tag => tag.Tag).ToListAsync().ConfigureAwait(false),
                Title = title
            };
        }


        public async Task<ContentFile> LikeContentFile(int id)
        {
            var category = await _ContentFiles.FindAsync(id);
            category.LikeCount = category.LikeCount + 1;
            await _uow.SaveChangesAsync();
            return category;
        }

    }
}
