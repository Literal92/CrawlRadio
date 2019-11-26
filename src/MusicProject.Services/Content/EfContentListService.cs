using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusicProject.Common;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
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
    public class EfContentListService : IContentListService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ContentList> _contentList;
        private readonly DbSet<Entities.Content.Content> _contents;
        private readonly DbSet<ContentSelected> _contentSelecteds;
        private readonly DbSet<ContentListTag> _listTags;
        private readonly DbSet<Tag> _tags;
        private readonly IUploadService _uploadService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IContentListFileService _contentListFile;

        public EfContentListService(
          IUnitOfWork uow,
          IUploadService uploadService,
          IHostingEnvironment hostingEnvironment,
          IContentListFileService contentListFile
        )
        {
            _hostingEnvironment = hostingEnvironment;
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));
            _uploadService = uploadService;
            _contentList = _uow.Set<ContentList>();
            _contents = _uow.Set<Entities.Content.Content>();
            _contentSelecteds = _uow.Set<ContentSelected>();
            _listTags = _uow.Set<ContentListTag>();
            _tags = _uow.Set<Tag>();
            _contentListFile = contentListFile;
        }






        public int AddNewContentList(ContentList contentList, IFormFile photo)
        {
            if (photo != null)
            {
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 300 };
                var resized = new string[1];
                fileName = _uploadService.UploadPicResize(
                  photo, "/content/files/contentlist",
                  2000,
                  ref fileSize,
                  size,
                  EnumC.Dimensions.Width,
                  ref resized
                );
                contentList.Pic = fileName;
                contentList.Thumbnail = size[0] + "/" + resized[0];
            }
            var id = _contentList.Add(contentList);
            _uow.SaveChanges();
            return id.Entity.Id;
        }


        public async Task<IList<Entities.Content.Content>> GetAllContentByPLId(int pLId)
        {
            var query = await _contentSelecteds.Where(x => x.ContentListId == pLId).Select(c => c.Content)
                .Include(p => p.Category).ThenInclude(ca => ca.CategoryTags).OrderBy(p => p.Priority).ToListAsync();

            return query;
        }



        public IList<ContentList> GetAllContentList(string type)
        {
            return _contentList.Include(p => p.ContentListTags).Where(p => p.Type == type).ToList();

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
        public async Task<PagedContentListViewModel> GetPagedContentListAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type, bool? archive, string title, int id, int tagId, string orderBy = "date")
        {
            if (!string.IsNullOrEmpty(title))
            {
                title = " " + title.Trim() + " ";
            }
            else
            {
                title = "";
            }

            var offset = (pageSize * pageNumber) - pageSize;

            var query = string.IsNullOrWhiteSpace(type) ?
              _contentList :
              _contentList.Where(l => l.Type == type);

            query = string.IsNullOrEmpty(title) ? query :
              query.Where(l => l.Title.Contains(title));

            query = tagId == 0 ? query :
              query.Where(l => l.ContentListTags.Any(p => p.TagId == tagId));

            query = archive == null ? query :
                          query.Where(l => l.IsPublish == archive);

            switch (orderBy)
            {
                case "date":
                    {
                        query = query.OrderByDescending(x => x.Id);
                        break;
                    }
                case "alphabet":
                    {
                        query = query.OrderBy(x => x.Title);
                        break;
                    }
                case "popular":
                    {
                        query = query.OrderByDescending(x => x.LikeCount);
                        break;
                    }
                case "visit":
                    {
                        query = query.OrderByDescending(x => x.VisitCount);
                        break;
                    }
            }

            return new PagedContentListViewModel
            {
                Paging =
        {
          TotalItems = await query.CountAsync().ConfigureAwait(false),
        },
                ContentLIstItem = await query.Skip(offset).Take(pageSize).Select(p => new ContentListViewModel
                {
                    Id = p.Id,
                    Thumbnail = p.Thumbnail,
                    Title = p.Title,
                    Type = "1",
                    IsExist = p.ContentSelecteds.Any(a => a.ContentId == id),

                }).ToListAsync().ConfigureAwait(false)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentList"></param>
        /// <param name="photo"></param>
        public void UpdateContentList(ContentList contentList, IFormFile photo)
        {
            if (photo != null && photo.Length > 0)
            {

                if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/contentlist/" + contentList.Title.SafeFileName()))
                    Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/contentlist/" + contentList.Title.SafeFileName());
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 200 };
                var resized = new string[1];

                fileName = _uploadService.UploadPicResize(
                                                          photo, "/content/files/contentlist/" + contentList.Title.SafeFileName(),
                                                          1200,
                                                          ref fileSize,
                                                          size,
                                                          EnumC.Dimensions.Width,
                                                          ref resized
                                                         );

                contentList.Pic = contentList.Title.SafeFileName() + "/" + fileName;
                contentList.Thumbnail = contentList.Title.SafeFileName() + "/" + size[0] + "/" + resized[0];

            }

            _contentList.Update(contentList);
            _uow.SaveChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ContentList> FindByIdAsync(int id, bool visited = false)
        {
            var PlayList = await _contentList.Include(p => p.ContentSelecteds).ThenInclude(a => a.Content)
                .Include(p => p.ContentListTags).ThenInclude(tag => tag.Tag).SingleOrDefaultAsync(a => a.Id == id);
            if (visited && PlayList != null)
            {
                PlayList.VisitCount++;
                await _uow.SaveChangesAsync();
            }
            return PlayList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var itemToRemove = _contentList.Find(id);
            if (itemToRemove != null)
            {
                _contentList.Remove(itemToRemove);
                _uow.SaveChanges();
            }
        }

        public bool CheckExistInContentList(int id, int listId)
        {
            var item = _contentList.Find(id);
            // check records
            if (item != null)
            {
                return true;
            }
            return false;
        }

        public IList<ContentList> GetTopByType(string type, bool? isPublish)
        {
            var query = _contentList.Include(p => p.ContentListTags).Where(p => p.Type == type);
            query = isPublish != null ? query.Where(p => p.IsPublish == isPublish) : query;
            query = query.OrderByDescending(x => x.Id).Take(8);
            return query.ToList();
        }

        public int GetCount(string typeId, string title)
        {
            var query = string.IsNullOrEmpty(title) ? _contentList :
                _contentList.Where(l => l.Title.Contains(title));
            return query.Where(l => l.Type == typeId).Count();
        }


        public async Task<Tuple<string, string>> DownloadPlayList(int pLId, string type)
        {

            var playList = await FindByIdAsync(pLId);
            if (playList == null || type == "")
            {
                return new Tuple<string, string>("", "");
            }

            var path = _hostingEnvironment.WebRootPath + "/content/files/contentlist/";
            var zipFile = playList.Title.SafeFileName() + "/" +
                          playList.Title.SafeFileName() + type + ".zip";
            var fileName = path + zipFile;
            if (!File.Exists(fileName))
            {
                if (!Directory.Exists(path + playList.Title.SafeFileName()))
                    Directory.CreateDirectory(path + playList.Title.SafeFileName());

                var files = await GetMusicsFiles(pLId);


                using (var zipToOpen = new FileStream(fileName, FileMode.Create))
                {
                    using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {

                        foreach (var file in files)
                        {
                            if (type == "64")
                            {
                                archive.CreateEntryFromFile(
                                    file.Type == 2 ? _hostingEnvironment.WebRootPath + "/content/files/contentlistFile/" + file.Mp364 : _hostingEnvironment.WebRootPath + "/content/files/album/" + file.Mp364
                                   , file.Title.SafeFileName() + ".MP3");

                            }
                            else if (type == "128")
                            {
                                archive.CreateEntryFromFile(
                                    file.Type == 2 ? _hostingEnvironment.WebRootPath + "/content/files/contentlistFile/" + file.Mp3128 : _hostingEnvironment.WebRootPath + "/content/files/album/" + file.Mp3128
                                      , file.Title.SafeFileName() + ".MP3");


                            }
                            else if (type == "320")
                            {
                                archive.CreateEntryFromFile(
                                    file.Type == 2 ? _hostingEnvironment.WebRootPath + "/content/files/contentlistFile/" + file.Mp3320 : _hostingEnvironment.WebRootPath + "/content/files/album/" + file.Mp3320
                                    , file.Title.SafeFileName() + ".MP3");


                            }
                        }
                    }
                }

                if (type == "128")
                {
                    playList.ZipMp3128 = zipFile;
                }
                else if (type == "64")
                {
                    playList.ZipMp364 = zipFile;
                }
                else if (type == "320")
                {
                    playList.ZipMp3320 = zipFile;
                }

                UpdateContentList(playList, null);

            }


            return new Tuple<string, string>(fileName, zipFile);

        }

        public async Task<IList<ContentList>> GetRelated(int? artistId, int? tagId, int? styleId, int? music, int top, int playListId)
        {
            var query = _contentList.Where(l => l.IsPublish && l.Id != playListId && (
                                                l.ContentListTags.Any(a => a.TagId == tagId) 
                                             || l.ContentListTags.Any(a => a.TagId == artistId)
                                             || l.ContentListTags.Any(a => a.TagId == styleId)
                                             || l.ContentListTags.Any(a => a.TagId == music))
            );

            List<ContentList> sorted = new List<ContentList>();


            var catSorted = await query.Where(l => l.ContentListTags.Any(a => a.TagId == tagId)).Take(40).ToListAsync();
            var artistSorted = await query.Where(l => l.ContentListTags.Any(a => a.TagId == artistId)).Take(40).ToListAsync();
            var styleSorted = await query.Where(l => l.ContentListTags.Any(a => a.TagId == styleId) && !artistSorted.Any(p2 => p2.Id == l.Id)).Take(40).ToListAsync();
            var musicSorted = await query.Where(l => l.ContentListTags.Any(a => a.TagId == music)&& !artistSorted.Any(p2 => p2.Id == l.Id)&& !styleSorted.Any(p2 => p2.Id == l.Id)).Take(40).ToListAsync();

            sorted.AddRange(catSorted.OrderByDescending(p => p.VisitCount).Take(4));
            sorted.AddRange(artistSorted.OrderByDescending(p => p.VisitCount).Take(styleSorted.Count() + musicSorted.Count() > 8 ? 4 : 12 - styleSorted.Count() + musicSorted.Count()));         
            sorted.AddRange(styleSorted.OrderByDescending(p => p.VisitCount).Take(artistSorted.Count() + musicSorted.Count() > 8 ? 4 : 12 - artistSorted.Count() + musicSorted.Count()));


            sorted.AddRange(musicSorted.OrderByDescending(p => p.VisitCount).Take(artistSorted.Count() + styleSorted.Count() > 8 ? 4 : 12 - artistSorted.Count() + styleSorted.Count()));


            return sorted.Distinct().ToList();

        }

        public async Task<ContentList> LikePlayList(int id)
        {
            var playList = await FindByIdAsync(id);
            playList.LikeCount++;
            await _uow.SaveChangesAsync();
            return playList;
        }

        public IList<Tag> GetCategoryByPLId(int Id)
        {
            var Category = _listTags.Where(x => x.Type == "cat" && x.ContentListId == Id).Select(g => new Tag
            {
                Title = g.Tag.Title,
                Id = g.Tag.Id
            }).ToList();

            return Category;
        }

        public IList<ContentList> GetTopByType(string typeId, int pageNumber, int pageSize, string title)
        {
            var offset = (pageSize * pageNumber) - pageSize;

            var query = string.IsNullOrWhiteSpace(typeId) ?
              _contentList :
              _contentList.Where(l => l.Type == typeId);

            query = string.IsNullOrEmpty(title) ? query :
              query.Where(l => l.Title.Contains(title));


            query = query.OrderByDescending(x => x.Id);
            return query.Skip(offset).Take(pageSize).ToList();
        }

        public async Task<List<ContentListViewModel.Music>> GetMusicsFiles(int id)
        {
            var file = await _contentListFile.GetAllByContentList(id);

            var musics = (from cs in _contentSelecteds
                          join c in _contents on cs.ContentId equals c.Id
                          where (cs.ContentListId == id)
                          select new ContentListViewModel.Music
                          {
                              Id = c.Id,
                              Title = c.Title,
                              Like = c.LikeCount,
                              Visit = c.VisitCount,
                              Category = c.CategoryId,
                              Mp3128 = c.Mp3128,
                              Mp3320 = c.Mp3320,
                              Mp364 = c.Mp364,
                              Type = 1,
                              Thumbnail = cs.ContentList.Thumbnail,
                              Priority = cs.Order
                          }).ToList();
            musics.AddRange(from f in file
                            select new ContentListViewModel.Music
                            {
                                Id = f.Id,
                                Title = f.Title,
                                Like = f.LikeCount,
                                Visit = f.VisitCount,
                                Category = 0,
                                Mp3128 = f.Mp3128,
                                Mp3320 = f.Mp3320,
                                Mp364 = f.Mp364,
                                Type = 2,
                                Thumbnail = f.ContentList.Thumbnail,
                                Priority = f.Order
                            });
            return musics.OrderBy(x => x.Priority).ToList();
        }
    }
}
