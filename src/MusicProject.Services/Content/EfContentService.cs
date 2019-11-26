using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusicProject.Common;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Entities.Identity;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;
using MusicProject.ViewModels.Content;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace MusicProject.Services.Content
{
    public class EfContentService : IContentService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        IUnitOfWork _uow;
        private readonly IUploadService _uploadService;
        private readonly ICategoryService _categoryService;
        private readonly DbSet<Entities.Content.Content> _products;
        public EfContentService(IUnitOfWork uow,
          IUploadService uploadService,
          ICategoryService categoryService,
          IHostingEnvironment hostingEnvironment,
          IContentSelectedService contentSelectedService
          )
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));
            _uploadService = uploadService;
            _products = _uow.Set<Entities.Content.Content>();
            _categoryService = categoryService;
            _hostingEnvironment = hostingEnvironment;

        }
        public async Task<Tuple<string, string>> DownloadAlbum(int albumId, string type)
        {

            var album = await _categoryService.FindByIdAsync(albumId);
            if (album == null || type == "")
            {
                return new Tuple<string, string>("", "");
            }

            var path = _hostingEnvironment.WebRootPath + "/content/files/album/";
            var zipFile = album.Description.SafeFileName() + "/" +
                          album.Description.SafeFileName() + type + ".zip";
            var fileName = path + zipFile;

            if (!File.Exists(fileName))
            {
                var files = await GetAllByCategoryAsync(albumId);
                using (var zipToOpen = new FileStream(fileName, FileMode.Create))
                {
                    using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {

                        foreach (var file in files)
                        {
                            if (type == "64")
                            {
                                archive.CreateEntryFromFile(path + file.Mp364, file.Mp364);
                                album.ZipMp364Size = zipToOpen.Length.ToString();
                            }
                            else if (type == "128")
                            {
                                archive.CreateEntryFromFile(path + file.Mp3128, file.Mp3128);
                                album.ZipMp3128Size = zipToOpen.Length.ToString();
                            }
                            else if (type == "320")
                            {
                                archive.CreateEntryFromFile(path + file.Mp3320, file.Mp3320);
                                album.ZipMp3320Size = zipToOpen.Length.ToString();
                            }
                        }
                    }
                }

                if (type == "128")
                {
                    album.ZipMp3128 = zipFile;
                }
                else if (type == "64" && album.ZipMp364 != null)
                {
                    album.ZipMp364 = zipFile;
                }
                else if (type == "320" && album.ZipMp3320 != null)
                {
                    album.ZipMp3320 = zipFile;
                }

                _categoryService.UpdateCategory(album, null, null, null, null);

            }


            return new Tuple<string, string>(fileName, zipFile);

        }

        public async Task<PagedContentItemsViewModel> GetPagedContentsListAsync(
          int pageNumber, int recordsPerPage,
          string sortByField, SortOrder sortOrder,
          bool showAllUsers)
        {
            var skipRecords = pageNumber * recordsPerPage;
            var query = _products.AsNoTracking();

            if (!showAllUsers)
            {
                //  query = query.Where(x => x.IsActive);
            }

            switch (sortByField)
            {
                case nameof(User.Id):
                    query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
                    break;
                default:
                    query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
                    break;
            }

            return new PagedContentItemsViewModel
            {
                Paging =
        {
          TotalItems = await query.CountAsync().ConfigureAwait(false)
        },
                Contents = await query.Skip(skipRecords).Take(recordsPerPage).ToListAsync().ConfigureAwait(false)

            };
        }
        private readonly IList<string> _allowedExtensions;


        public async Task<IList<Entities.Content.Content>> GetByListIdAsync(List<int> id)
        {
            return await _products.Include(p => p.Category).ThenInclude(ca => ca.CategoryTags).ThenInclude(t => t.Tag).Where(x => id.Contains(x.Id)).ToListAsync();
        }

        public void AddNewContent(Entities.Content.Content product, IFormFile photo, IFormFile photo2, IFormFile photo3, IFormFile[] files, IFormFile video, IFormFile pdf,

          IFormFile Mp364, IFormFile Mp3128, IFormFile Mp3320)
        {

            var directoryName = _categoryService.FindByIdAsync(product.CategoryId).Result.Description.SafeFileName();

            if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName))
                Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName);


            if (photo != null)
            {
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 300 };
                var resized = new string[1];
                fileName = _uploadService.UploadPicResize(
                                                          photo, "/content/files/album/" + directoryName,
                                                         2000,
                                                          ref fileSize,
                                                          size,
                                                          EnumC.Dimensions.Width,
                                                          ref resized
                                                         );
                product.Pic = directoryName + "/" + fileName;
                product.Thumbnail = directoryName + "/" + size[0] + "/" + resized[0];
            }

            if (photo2 != null)
            {
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 300 };
                var resized = new string[1];
                fileName = _uploadService.UploadPicResize(
                  photo2, "/content/files/content",
                  2000,
                  ref fileSize,
                  size,
                  EnumC.Dimensions.Width,
                  ref resized
                );
                product.Pic2 = fileName;
                product.Thumbnail2 = size[0] + "/" + resized[0];
            }

            if (photo3 != null)
            {
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 300 };
                var resized = new string[1];
                fileName = _uploadService.UploadPicResize(
                  photo3, "/content/files/content",
                  2000,
                  ref fileSize,
                  size,
                  EnumC.Dimensions.Width,
                  ref resized
                );
                product.Pic3 = fileName;
                product.Thumbnail3 = size[0] + "/" + resized[0];
            }
            if (video != null)
            {
                var allowed = new[] { "video/mp4", "image/x-png", "image/pjpeg", "image/jpeg", "image/gif" };

                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                                                          video, "/content/files/content/video",

                                                          ref fileSize,
                                                          allowed

                                                         );
                product.Video = fileName;

            }


            if (pdf != null)
            {
                var allowed = new[]
                {
          //"application/pdf", "application/x-pdf"
          "audio/mpeg",
          "audio/mp3"
        };

                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                                                          pdf, "/content/files/content/mp3",

                                                          ref fileSize,
                                                          allowed
                                                         );
                product.Pdf = fileName;

            }


            if (Mp3128 != null)
            {
                var allowed = new[]
                {
          //"application/pdf", "application/x-pdf"
          "audio/mpeg",
          "audio/mp3"
        };

                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  Mp3128, "/content/files/album/" + directoryName,

                                                          ref fileSize,
                                                          allowed
                                                         );
                product.Mp3128 = directoryName + "/" + fileName;

            }

            if (Mp364 != null)
            {
                if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/64"))
                    Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/64");
                var allowed = new[]
                {
          //"application/pdf", "application/x-pdf"
          "audio/mpeg",
          "audio/mp3"
        };

                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  Mp364, "/content/files/album/" + directoryName + "/64",

                                                          ref fileSize,
                                                          allowed
                                                         );
                product.Mp364 = directoryName + "/64/" + fileName;

            }
            if (Mp3320 != null)
            {
                var allowed = new[]
                {
          //"application/pdf", "application/x-pdf"
          "audio/mpeg",
          "audio/mp3"
        };
                if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/320"))
                    Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/320");
                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  Mp3320, "/content/files/album/" + directoryName + "/320",
                                                          ref fileSize,
                                                          allowed
                                                         );
                product.Mp3320 = directoryName + "/320/" + fileName;

            }


            var allowedMimeType = new[] { "image/png", "image/x-png", "image/pjpeg", "image/jpeg", "image/gif"
        };

            if (files != null)
            {
                List<ContentFile> c = new List<ContentFile>();
                foreach (var f in files)
                {
                    string ffileName = "";
                    long ffileSize = 0;
                    var fsize = new[] { 650 };
                    var fresized = new string[1];
                    if (allowedMimeType.Contains(f.ContentType))
                    {

                        ffileName = _uploadService.UploadPicResize(
                          f, "/content/files/content/files",
                          2000,
                          ref ffileSize,
                          fsize,
                          EnumC.Dimensions.Width,
                          ref fresized
                        );


                        c.Add(new ContentFile
                        {
                            Ext = f.ContentType,
                            FileName = ffileName,
                            FileSize = ffileSize,
                            Thumbnail = fsize[0] + "/" + fresized[0],
                            Type = "pic"
                        });
                    }

                    else
                    {

                        ffileName = _uploadService.UploadFile(
                          f, "/content/files/content/files",
                          ref ffileSize, new[] { "image/svg+xml", "application/svg+xml", "text/html+svg", "video/mp4", "audio/mp3", "audio/mpeg", }
                        );

                        c.Add(new ContentFile
                        {
                            Ext = f.ContentType,
                            FileName = ffileName,
                            FileSize = ffileSize,
                            Type = "file"
                        });
                    }
                }

                product.ContentFiles = c;

            }

            _products.Add(product);
            _uow.SaveChanges();
        }

        public void UpdateContent(Entities.Content.Content product, IFormFile photo, IFormFile photo2, IFormFile photo3, IFormFile[] files, IFormFile video, IFormFile pdf, IFormFile Mp364, IFormFile Mp3128, IFormFile Mp3320)
        {
            var directoryName = _categoryService.FindByIdAsync(product.CategoryId).Result.Description.SafeFileName();

            if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName))
                Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName);

            if (photo != null && photo.Length > 0)
            {

                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + product.Pic))
                {
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + product.Pic);

                }

                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + product.Thumbnail))
                {
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" +
                                product.Thumbnail);
                }



                string fileName = "";
                long fileSize = 0;
                var size = new[] { 300 };
                var resized = new string[1];
                fileName = _uploadService.UploadPicResize(
                                                          photo,
                                                          "/content/files/album/" + directoryName,
                                                         2000,
                                                          ref fileSize,
                                                          size,
                                                          EnumC.Dimensions.Width,
                                                          ref resized
                                                         );
                product.Pic = directoryName + "/" + fileName;
                product.Thumbnail = directoryName + "/" + size[0] + "/" + resized[0];
            }


            if (photo2 != null && photo2.Length > 0)
            {
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 300 };
                var resized = new string[1];
                fileName = _uploadService.UploadPicResize(
                  photo2,
                  "/content/files/content",
                  2000,
                  ref fileSize,
                  size,
                  EnumC.Dimensions.Width,
                  ref resized
                );
                product.Pic2 = fileName;
                product.Thumbnail2 = size[0] + "/" + resized[0];
            }

            if (photo3 != null && photo3.Length > 0)
            {
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 300 };
                var resized = new string[1];
                fileName = _uploadService.UploadPicResize(
                  photo3, "/content/files/content",
                  2000,
                  ref fileSize,
                  size,
                  EnumC.Dimensions.Width,
                  ref resized
                );
                product.Pic3 = fileName;
                product.Thumbnail3 = size[0] + "/" + resized[0];
            }

            if (Mp3128 != null)
            {
                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + product.Mp3128))
                {
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + product.Mp3128);
                }

                var allowed = new[]
                {
          //"application/pdf", "application/x-pdf"
          "audio/mpeg",
          "audio/mp3"
        };

                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  Mp3128, "/content/files/album/" + directoryName,

                                                          ref fileSize,
                                                          allowed
                                                         );
                product.Mp3128 = directoryName + "/" + fileName;

            }

            if (Mp364 != null)
            {
                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + product.Mp364))
                {
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + product.Mp364);
                }
                if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/64"))
                    Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/64");
                var allowed = new[]
                {
          //"application/pdf", "application/x-pdf"
          "audio/mpeg",
          "audio/mp3"
        };

                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  Mp364, "/content/files/album/" + directoryName + "/64",

                                                          ref fileSize,
                                                          allowed
                                                         );

                product.Mp364 = directoryName + "/64/" + fileName;

            }
            if (Mp3320 != null)
            {
                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + product.Mp3320))
                {
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + product.Mp3320);
                }
                var allowed = new[]
                {
          //"application/pdf", "application/x-pdf"
          "audio/mpeg",
          "audio/mp3"
        };
                if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/320"))
                    Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/320");
                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  Mp3320, "/content/files/album/" + directoryName + "/320",
                                                          ref fileSize,
                                                          allowed
                                                         );
                product.Mp3320 = directoryName + "/320/" + fileName;

            }


            if (video != null)
            {
                var allowed = new[] { "video/mp4", "image/x-png", "image/pjpeg", "image/jpeg", "image/gif" };

                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  video, "/content/files/content/video",

                  ref fileSize,
                  allowed

                );
                product.Video = fileName;

            }
            if (pdf != null)
            {
                var allowed = new[]
                {
          //"application/pdf", "application/x-pdf"
          "audio/mpeg",
          "audio/mp3"
        };

                string fileName = "";
                long fileSize = 0;
                fileName = _uploadService.UploadFile(
                  pdf, "/content/files/content/mp3",

                  ref fileSize,
                  allowed
                );
                product.Pdf = fileName;

            }
            var allowedMimeType = new[] { "image/png", "image/x-png", "image/pjpeg", "image/jpeg", "image/gif",
        };

            if (files != null)
            {
                //  List<ContentFile> c = new List<ContentFile>();
                var c = product.ContentFiles;
                foreach (var f in files)
                {
                    string ffileName = "";
                    long ffileSize = 0;
                    var fsize = new[] { 650 };
                    var fresized = new string[1];
                    if (allowedMimeType.Contains(f.ContentType))
                    {

                        ffileName = _uploadService.UploadPicResize(
                          f, "/content/files/content/files",
                          2000,
                          ref ffileSize,
                          fsize,
                          EnumC.Dimensions.Width,
                          ref fresized
                        );

                        c.Add(new ContentFile
                        {
                            Ext = f.ContentType,
                            FileName = ffileName,
                            FileSize = ffileSize,
                            Thumbnail = fsize[0] + "/" + fresized[0],
                            Type = "pic"
                        });
                    }

                    else
                    {

                        ffileName = _uploadService.UploadFile(
                          f, "/content/files/content/files",
                          ref ffileSize, new[] { "image/svg+xml", "application/svg+xml", "text/html+svg", "video/mp4", "audio/mp3", "audio/mpeg", }
                        );

                        c.Add(new ContentFile
                        {
                            Ext = f.ContentType,
                            FileName = ffileName,
                            FileSize = ffileSize,
                            Type = "file"
                        });
                    }
                }

                product.ContentFiles = c;

            }


            _products.Update(product);
            _uow.SaveChanges();
        }

        public async Task<Entities.Content.Content> LikeContent(int id)
        {
            var content = await _products.FindAsync(id);
            content.LikeCount = content.LikeCount + 1;
            _uow.SaveChanges();
            return content;
        }

        public async Task<Entities.Content.Content> VisitContent(int id)
        {
            var content = await _products.FindAsync(id);
            if (content != null)
            {
                content.VisitCount = content.VisitCount + 1;
                _uow.SaveChanges();
            }

            return content;
        }

        public IList<Entities.Content.Content> GetAllContents()
        {
            return _products.Include(x => x.Category).ToList();
        }
        public Entities.Content.Content GetByTitle(string title)
        {
            return _products.SingleOrDefault(p => p.Title == title);
        }
        public IList<Entities.Content.Content> GetAllByType(int typeId)
        {
            return _products.Where(p => p.TypeId == typeId).OrderBy(p => p.Priority).Include(x => x.Category).ToList();
        }
        public async Task<IList<Entities.Content.Content>> GetAllByCategoryAsync(int categoryId)
        {
            return await _products.Include(p => p.Category).ThenInclude(ca => ca.CategoryTags).ThenInclude(t => t.Tag).Where(p => p.CategoryId == categoryId).OrderBy(p => p.Priority).ToListAsync();
        }
        public async Task<IList<Entities.Content.Content>> GetAllByTypeAsync(int typeId)
        {
            return await _products.Where(p => p.TypeId == typeId).OrderBy(p => p.Priority).Include(x => x.Category).ToListAsync();
        }
        public int GetCount(int typeId, string title)
        {

            var query = typeId == 0 ?
              _products :
              _products.Where(l => l.TypeId == typeId);

            query = string.IsNullOrEmpty(title) ? query :
              query.Where(l => l.Title.Contains(title));
            return query.ToList().Count;

        }

        public IList<Entities.Content.Content> GetTopByType(string typeId, int pageNumber, int pageSize, string title)
        {

            var offset = (pageSize * pageNumber) - pageSize;

            var query = string.IsNullOrWhiteSpace(typeId) ?
              _products :
              _products.Where(l => l.TypeId == Convert.ToInt32(typeId));

            query = string.IsNullOrEmpty(title) ? query :
              query.Where(l => l.Title.Contains(title));
            query = query.Where(p => p.Mp364 == null);
            query = query.OrderByDescending(x => x.Id);
            return query.Skip(offset).Take(pageSize).Include(p => p.Category).ToList();
            //  return _products.Where(p => p.TypeId == typeId).OrderByDescending(p => p.Id).Include(x => x.Category).Take(top).ToList();

        }
        public IList<Entities.Content.Content> GetTopByTypeAndSkip(int typeId, int top, int skip, int catId)
        {

            var query = catId == 0 ? _products :
              _products.Where(l => l.CategoryId == catId);


            return query.Where(p => p.TypeId == typeId).OrderByDescending(p => p.Id).Include(x => x.Category).Skip(skip).Take(top).ToList();
        }


        public IList<Entities.Content.Content> GetTopInCat(int top)
        {
            return _products.GroupBy(row => row.Category)
                .SelectMany(g => g.OrderBy(row => row.Id).Take(top)).Include(x => x.Category)
                .ToList();
        }

        public Entities.Content.Content FindById(int id)
        {

            return _products.Find(id);
        }
        public Entities.Content.Content UpdateVisit(int id)
        {
            var content = _products.Find(id);
            content.VisitCount = content.VisitCount + 1;
            _uow.SaveChanges();
            return content;
        }

        public Task<Entities.Content.Content> FindByIdAsync(int id)
        {
            return _products.Include(a => a.Category).Include(p => p.ContentFiles).Include(p => p.ContentTags).ThenInclude(contentTag => contentTag.Tag).SingleOrDefaultAsync(p => p.Id == id);
        }
        public void Delete(int id)
        {
            var itemToRemove = _products.FirstOrDefault(x => x.Id.Equals(id));
            if (itemToRemove != null)
            {

                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + itemToRemove.Pic))
                {
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + itemToRemove.Pic);
                }

                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + itemToRemove.Thumbnail))
                {
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" +
                                itemToRemove.Thumbnail);
                }
                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + itemToRemove.Mp364))
                {
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" +
                                itemToRemove.Mp364);
                }
                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + itemToRemove.Mp3128))
                {
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" +
                                itemToRemove.Mp3128);
                }
                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + itemToRemove.Mp3320))
                {
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" +
                                itemToRemove.Mp3320);
                }

                _products.Remove(itemToRemove);
                _uow.SaveChanges();
            }
        }

        public async Task<PagedContentItemsViewModel> GetPagedContentsAsync(
            int pageNumber,
            int pageSize,
            SortOrder sortOrder,
            string type,
             bool? archive,
            int categoryId,
            string from,
            string to,
            string title,
            int artisId=0,
            int styleId=0,
            int musicId=0

           )
        {
            var offset = (pageSize * pageNumber) - pageSize;

            var query = string.IsNullOrWhiteSpace(type) ? _products :
                _products.Where(l => l.TypeId == Convert.ToInt32(type));

            query = archive == null ? query :
            query.Where(l => l.IsArchive == archive);

            query = string.IsNullOrWhiteSpace(title) ? query :
            query.Where(l => l.Title.Contains(title));

            query = string.IsNullOrWhiteSpace(from) ? query :
            query.Where(l => l.PublishDateTime >= from.ToGregorianDateTimeOffset());
            query = string.IsNullOrWhiteSpace(to) ? query :
            query.Where(l => l.PublishDateTime <= to.ToGregorianDateTimeOffset());

            query = categoryId == 0 ? query :
              query.Where(l => l.CategoryId == categoryId);

            query= musicId==0?query:
            query.Include(a=>a.Category).ThenInclude(a=>a.CategoryTags).Where(p=>p.Category.CategoryTags.Any(s=>s.TagId==musicId));

            query = styleId==0?query:
            query.Include(a=>a.Category).ThenInclude(a=>a.CategoryTags).Where(p=>p.Category.CategoryTags.Any(s=>s.TagId== styleId));

            query = artisId==0?query:
            query.Include(a=>a.Category).ThenInclude(a=>a.CategoryTags).Where(p=>p.Category.CategoryTags.Any(s=>s.TagId== artisId));


            query = query.OrderBy(p => p.Priority).ThenBy(x => x.Id);

            return new PagedContentItemsViewModel
            {
                Paging =
              {
                  TotalItems = await query.CountAsync().ConfigureAwait(false)
              },
                Contents = await query.Skip(offset).Take(pageSize).Include(p => p.Category).ToListAsync().ConfigureAwait(false)
            };
        }







    }
}
