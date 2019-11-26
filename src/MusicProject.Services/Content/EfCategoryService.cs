using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MusicProject.Common;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Content
{
    public class EfCategoryService : ICategoryService
    {
        IUnitOfWork _uow;
        readonly DbSet<Category> _Categories;
        private readonly IUploadService _uploadService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ITagService _tagService;

        public EfCategoryService(
          IUnitOfWork uow,
          IUploadService uploadService,
          ITagService tagService,

          IHostingEnvironment hostingEnvironment


        )
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));
            _uploadService = uploadService;
            _Categories = _uow.Set<Category>();
            _hostingEnvironment = hostingEnvironment;
            _tagService = tagService;


        }



        public IList<Category> GetIndentedCategory(int type)
        {
            var cats = GetAllCategories(type).Where(p => p.ParentId == null);
            IList<Category> comboCat = new List<Category>();
            foreach (var category in cats)
            {
                comboCat.Add(new Category
                {
                    Title = category.Title,
                    Id = category.Id
                });
                if (category.InverseParent != null)
                {
                    foreach (var firstLevel in category.InverseParent)
                    {
                        comboCat.Add(new Category
                        {
                            Title = "--  ↩  --  " + firstLevel.Title,
                            Id = firstLevel.Id
                        });
                        if (firstLevel.InverseParent != null)
                        {
                            foreach (var secondLevel in firstLevel.InverseParent)
                            {
                                comboCat.Add(new Category
                                {
                                    Title = "--  --  ↩  --    --  " + secondLevel.Title,
                                    Id = secondLevel.Id
                                });
                            }
                        }
                    }
                }
            }

            return comboCat;
        }


        public IList<Category> GetAllCategories(int type)
        {
            return _Categories.Where(p => p.TypeId == type).Include(p => p.InverseParent).OrderByDescending(p => p.Id).ToList();
        }

        public IList<Category> GetTopByType(string typeId, int pageNumber, int pageSize, string title, bool? isPublish, bool? isSelected = null)
        {
            var offset = (pageSize * pageNumber) - pageSize;

            var query = string.IsNullOrWhiteSpace(typeId) ?
              _Categories :
              _Categories.Where(l => l.TypeId == Convert.ToInt32(typeId));

            query = string.IsNullOrEmpty(title) ? query :
              query.Where(l => l.Title.Contains(title));

            query = isPublish == null ? query :
              query.Where(l => l.IsPublish == isPublish);

            query = isSelected == null ? query :
              query.Where(l => l.IsSelected == isSelected);

            query = query.OrderByDescending(x => x.Id);
            return query.Skip(offset).Take(pageSize).Include(p => p.Contents)
                                                    .Include(g => g.CategoryTags)
                                                    .ThenInclude(t => t.Tag).ToList();
        }

        public IList<Category> GetForSearch(string typeId, int pageNumber, int pageSize, string title, bool? isPublish)
        {
            if (!string.IsNullOrEmpty(title))
            {
                title = " " + title.Trim() + " ";
            }
            var offset = (pageSize * pageNumber) - pageSize;

            var query = string.IsNullOrWhiteSpace(typeId) ?
              _Categories :
              _Categories.Where(l => l.TypeId == Convert.ToInt32(typeId));

            //query = string.IsNullOrEmpty(title) ? query :
            //  query.Where(l => l.Title.Contains(title) || l.Description.Contains(title)
            //                   //   ||l.ContentText.Contains(title)
            //                   );


            query = string.IsNullOrEmpty(title) ? query :
          query.Where(l => l.Title.Contains(title) || l.ContentText.Contains(title) || l.ContentText.Contains(">" + title.Trim() + "<") || l.Description.Contains(title)
          ||
          Regex.Replace(l.ContentText, "<.*?>", " ").Replace("&nbsp;", " ").Contains(title));





        query = isPublish == null ? query :
              query.Where(l => l.IsPublish == isPublish);




            query = query.OrderByDescending(x => x.Id);
            return query.Skip(offset).Take(pageSize).Include(p => p.Contents)
                                                    .Include(g => g.CategoryTags)
                                                    .ThenInclude(t => t.Tag).ToList();
        }

        public int GetCount(string typeId, string title, bool? isPublish = null, int? tagId = null)
        {


            var query = string.IsNullOrWhiteSpace(typeId) ?
              _Categories :
              _Categories.Where(l => l.TypeId == Convert.ToInt32(typeId));

            query = string.IsNullOrEmpty(title) ? query :
              query.Where(l => l.Title.StartsWith(title));
            query = isPublish == null ? query : query.Where(p => p.IsPublish == isPublish);
            query = tagId == null ? query : query.Where(p => p.CategoryTags.Any(t => t.TagId == tagId));
            return query.ToList().Count;
        }

        public IList<Category> GetTopByLike(string typeId, int pageNumber, int pageSize, bool? isPublish)
        {
            var offset = (pageSize * pageNumber) - pageSize;
            var query = string.IsNullOrWhiteSpace(typeId) ?
              _Categories :
              _Categories.Where(l => l.TypeId == Convert.ToInt32(typeId));
            query = isPublish == null ? query :
              query.Where(l => l.IsPublish == isPublish);
            query = query.OrderByDescending(x => x.LikeCount);
            return query.Skip(offset).Take(pageSize).Include(p => p.Contents).ToList();
        }




        public async Task<IList<Category>> GetTopByTypeAsync(string typeId, int pageNumber, int pageSize, int? tagId, bool? isPublish)
        {
            var offset = (pageSize * pageNumber) - pageSize;
            var query = string.IsNullOrWhiteSpace(typeId) ?
              _Categories :
              _Categories.Where(l => l.TypeId == Convert.ToInt32(typeId));
            query = query.OrderByDescending(x => x.Id);

            query = tagId == null ? query :
              query.Where(l => l.CategoryTags.Any(a => a.TagId == tagId));

            query = isPublish == null ? query :
              query.Where(l => l.IsPublish == isPublish);


            var result = await query.Skip(offset).Take(pageSize).Include(a => a.Contents).Include(p => p.CategoryTags)
            .ThenInclude(tag => tag.Tag)
              .ToListAsync();
            return result;
        }

        public async Task<IList<Category>> GetTopByLikeAsync(string typeId, int pageNumber, int pageSize)
        {
            var offset = (pageSize * pageNumber) - pageSize;
            var query = string.IsNullOrWhiteSpace(typeId) ?
              _Categories :
              _Categories.Where(l => l.TypeId == Convert.ToInt32(typeId));
            query = query.OrderByDescending(x => x.LikeCount);
            return await query.Skip(offset).Take(pageSize).Include(p => p.CategoryTags)
              .ThenInclude(tag => tag.Tag).ToListAsync();
        }


        public async Task<IList<Category>> GetRelated(int? artistId, int? tagId, int? styleId, int? music, int top, int categoryId)
        {
            var query = _Categories.Where(l => l.IsPublish && l.Id != categoryId && (

                                                l.CategoryTags.Any(a => a.TagId == artistId)
                                               || l.CategoryTags.Any(a => a.TagId == styleId)
                                               || l.CategoryTags.Any(a => a.TagId == music))

            )
            //.OrderBy(o=>o.CategoryTags.OrderBy(a=>a.TagId==artistId)
            //.ThenBy(s=>s.TagId==styleId).ThenBy(m=>m.TagId==music)
            //)
            ;
            // var q = await query.Include(tag => tag.CategoryTags).ToListAsync();

            List<Category> sorted = new List<Category>();


            var artistSorted = await query.Where(l => l.CategoryTags.Any(a => a.TagId == artistId)).Take(40).ToListAsync();
            var styleSorted = await query.Where(l => l.CategoryTags.Any(a => a.TagId == styleId

            ) && !artistSorted.Any(p2 => p2.Id == l.Id

             )).Take(40).ToListAsync();
            var musicSorted = await query.Where(l => l.CategoryTags.Any(a => a.TagId == music)

             && !artistSorted.Any(p2 => p2.Id == l.Id)
             && !styleSorted.Any(p2 => p2.Id == l.Id)

            ).Take(40).ToListAsync();


            sorted.AddRange(artistSorted.OrderByDescending(p => p.VisitCount).Take(styleSorted.Count() + musicSorted.Count() > 8 ? 4 : 12 - styleSorted.Count() + musicSorted.Count()));


            sorted.AddRange(styleSorted.OrderByDescending(p => p.VisitCount).Take(artistSorted.Count() + musicSorted.Count() > 8 ? 4 : 12 - artistSorted.Count() + musicSorted.Count()));


            sorted.AddRange(musicSorted.OrderByDescending(p => p.VisitCount).Take(artistSorted.Count() + styleSorted.Count() > 8 ? 4 : 12 - artistSorted.Count() + styleSorted.Count()));


            return sorted;

        }


        public Category GetByTitle(string title)
        {
            return _Categories.SingleOrDefault(p => p.Title == title);
        }

        public async Task<PagedCategoryViewModel> GetPagedCategoryAsync(int pageNumber, int pageSize, SortOrder sortOrder, string type,
          bool? archive, string title, int tagId, string orderBy = "date"
          )

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
              _Categories :
              _Categories.Where(l => l.TypeId == Convert.ToInt32(type));

            query = archive == null ? query :
              query.Where(l => l.IsPublish == archive);
          //  var artists = _tagService.GetByTitle(title.Trim(), "artist");
            query = string.IsNullOrEmpty(title) ? query :
              query.Where(l => l.Title.Contains(title) || l.ContentText.Contains(title) || l.ContentText.Contains(">" + title.Trim() + "<") || l.Description.Contains(title.Trim())
              ||
              Regex.Replace(l.ContentText, "<.*?>", " ").Replace("&nbsp;"," ").Contains(title )


              //     l.CategoryTags.Any(t => artists.Any(p => p.Id == t.Id)

              );


         

            query = tagId == 0 ? query : query.Where(p => p.CategoryTags.Any(t => t.TagId == tagId));
            switch (orderBy)
            {
                case "date":
                    {
                        //query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.PublishDateTime) : query.OrderBy(x => x.PublishDateTime);
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

            return new PagedCategoryViewModel
            {
                Paging =
        {
          TotalItems = await query.CountAsync().ConfigureAwait(false)
        },  
                CategoryItem = await query.Skip(offset).Take(pageSize).Include(cat => cat.CategoryTags).ThenInclude(tag => tag.Tag).Include(p => p.Contents).ToListAsync().ConfigureAwait(false),
                Title = title
            };
        }


        public async Task<Category> FindByIdAsync(int id, bool visited = false)
        {
            var category = await _Categories.Include(p => p.Contents).ThenInclude(vi => vi.ContentFiles).Include(p => p.CategoryTags).ThenInclude(p => p.Tag).SingleOrDefaultAsync(p => p.Id == id);
            if (visited && category != null)
            {
                category.VisitCount = category.VisitCount + 1;
                _uow.SaveChanges();
            }

            return category;
        }

        public async Task<Category> LikeCategory(int id)
        {
            var category = await _Categories.FindAsync(id);
            category.LikeCount = category.LikeCount + 1;
            await _uow.SaveChangesAsync();
            return category;
        }
        public async Task<IList<Category>> GetByListIdAsync(List<int> id)
        {
            return await _Categories.Where(x => id.Contains(x.Id)).Include(p => p.CategoryTags).ThenInclude(p => p.Tag).Include(p => p.Contents).ToListAsync();
        }



        public int AddNewCategory(Category category, IFormFile photo, IFormFile mp364, IFormFile mp3128, IFormFile mp3320)
        {
            long fileSize64 = 0;
            long fileSize128 = 0;
            long fileSize320 = 0;
            string albumName = category.Description.SafeFileName();
            string albumFolder = _hostingEnvironment.WebRootPath + "/content/files/album/" + albumName;
            if (!Directory.Exists(albumFolder))
                Directory.CreateDirectory(albumFolder);
            else
            {
                int i = 0;
                do
                {
                    i = i + 1;
                }
                while (Directory.Exists(albumFolder + i));

                albumName = albumName + i;
                albumFolder = _hostingEnvironment.WebRootPath + "/content/files/album/" + albumName;
                Directory.CreateDirectory(albumFolder);
                category.Description = albumName;
            }
            var contents = new List<Entities.Content.Content>();
            if (photo != null)
            {
                var fileName = "";
                long fileSize = 0;


                var size = new[] { 200 };
                var resized = new string[1];

                fileName = _uploadService.UploadPicResize(
                  photo, "/content/files/album/" + albumName,
                  1200,
                  ref fileSize,
                  size,
                  EnumC.Dimensions.Width,
                  ref resized
                );

                category.Pic = albumName + "/" + fileName;
                // category.Thumbnail = size[0] + "/" + resized[0];
                category.Thumbnail = albumName + "/" + size[0] + "/" + resized[0];
            }

            string zipPath = albumFolder + "/" + albumName;
            if (mp364 != null)
            {
                if (!Directory.Exists(albumFolder + "/64"))
                    Directory.CreateDirectory(albumFolder + "/64");
                var mp364Filename = _uploadService.UploadFile(mp364, "/content/files/album/" + category.Description.SafeFileName(), ref fileSize64,
                  new[] { "application/zip", "application/octet-stream", "application/zip-compressed", "application/x-zip-compressed" },
                                                                                  category.Description.SafeFileName() + "64.zip");
                category.ZipMp364Size = fileSize64.ToString();

                category.ZipMp364 = albumName + "/" + mp364Filename;
                using (var archive = ZipFile.OpenRead(zipPath + "64.zip"))
                {
                    var entries = archive.Entries.
                     OrderBy(p => Int32.Parse(Regex.Match(p.Name, @"^\d+").ToString())).
                      ToList();
                    for (var index = 0; index < entries.Count; index++)
                    {
                        var entry = entries[index];
                        var fileName = entry.FullName.Replace(".mp3", "").SafeFileName();
                        if (entry.FullName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                        {
                            // Gets the full path to ensure that relative segments are removed.
                            var destinationPath = Path.GetFullPath(Path.Combine(albumFolder + "/64", fileName + ".mp3"));
                            var music = contents.SingleOrDefault(p => p.Title == fileName) ?? contents.SingleOrDefault(p => p.Priority == index);
                            if (music != null) music.Mp364 = albumName + "/64/" + fileName + ".mp3";
                            else
                            {
                                contents.Add(new Entities.Content.Content
                                {
                                    Title = entry.FullName.Replace(".mp3", ""),
                                    Mp364 = albumName + "/64/" + fileName + ".mp3",
                                    Priority = index,
                                    TypeId = 1
                                });
                            }
                            entry.ExtractToFile(destinationPath);
                        }
                    }
                }

            }

            if (mp3128 != null)
            {
                var mp3128Filename = _uploadService.UploadFile(mp3128, "/content/files/album/" + category.Description.SafeFileName(), ref fileSize128,
                  new[] { "application/zip", "application/octet-stream", "application/zip-compressed", "application/x-zip-compressed" }
                  , category.Description.SafeFileName() + "128.zip");

                category.ZipMp3128Size = fileSize128.ToString();

                category.ZipMp3128 = albumName + "/" + mp3128Filename;
                using (var archive = ZipFile.OpenRead(zipPath + "128.zip"))
                {
                    var entries = archive.Entries.
                    OrderBy(p => Int32.Parse(Regex.Match(p.Name, @"^\d+").ToString()))
                      .ToList();

                    for (var index = 0; index < entries.Count; index++)
                    {
                        var entry = entries[index];
                        var fileName = entry.FullName.Replace(".mp3", "").SafeFileName();

                        if (entry.FullName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                        {
                            // Gets the full path to ensure that relative segments are removed.
                            var destinationPath = Path.GetFullPath(Path.Combine(albumFolder, fileName + ".mp3"));
                            var music = contents.SingleOrDefault(p => p.Title == fileName) ?? contents.SingleOrDefault(p => p.Priority == index);
                            if (music != null) music.Mp3128 = albumName + "/" + fileName + ".mp3";
                            else
                            {
                                contents.Add(new Entities.Content.Content
                                {
                                    Title = entry.FullName.Replace(".mp3", ""),
                                    Mp3128 = albumName + "/" + fileName + ".mp3",
                                    Priority = index,
                                    TypeId = 1
                                });
                            }
                            entry.ExtractToFile(destinationPath);
                        }
                    }
                }
            }

            if (mp3320 != null)
            {
                if (!Directory.Exists(albumFolder + "/320"))
                    Directory.CreateDirectory(albumFolder + "/320");
                var mp3320Filename = _uploadService.UploadFile(mp3320, "/content/files/album/" + category.Description.SafeFileName(), ref fileSize320,
                  new[] { "application/zip", "application/octet-stream", "application/zip-compressed", "application/x-zip-compressed" }
                   , category.Description.SafeFileName() + "320.zip");

                category.ZipMp3320Size = fileSize320.ToString();

                category.ZipMp3320 = albumName + "/" + mp3320Filename;
                using (var archive = ZipFile.OpenRead(zipPath + "320.zip"))
                {
                    var entries = archive.Entries.
                    OrderBy(p => Int32.Parse(Regex.Match(p.Name, @"^\d+").ToString()))
                      .ToList();

                    for (var index = 0; index < entries.Count; index++)
                    {
                        var entry = entries[index];
                        var fileName = entry.FullName.Replace(".mp3", "").SafeFileName();

                        if (entry.FullName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                        {
                            // Gets the full path to ensure that relative segments are removed.
                            var destinationPath = Path.GetFullPath(Path.Combine(albumFolder + "/320", fileName + ".mp3"));
                            var music = contents.SingleOrDefault(p => p.Title == fileName) ?? contents.SingleOrDefault(p => p.Priority == index);
                            if (music != null) music.Mp3320 = albumName + "/320/" + fileName + ".mp3";

                            else
                            {
                                contents.Add(new Entities.Content.Content
                                {
                                    Title = entry.FullName.Replace(".mp3", ""),
                                    Mp3320 = albumName + "/320/" + fileName + ".mp3",
                                    Priority = index,
                                    TypeId = 1
                                });
                            }
                            entry.ExtractToFile(destinationPath);
                        }
                    }
                }
            }
            category.Contents = contents;
            var id = _Categories.Add(category);
            _uow.SaveChanges();
            return id.Entity.Id;
        }

        public void UpdateCategory(Category category, IFormFile photo, IFormFile mp364, IFormFile mp3128, IFormFile mp3320)
        {

            long fileSize64 = 0;
            long fileSize128 = 0;
            long fileSize320 = 0;
            string albumRoot = _hostingEnvironment.WebRootPath + "/content/files/album/";
            string albumName = category.Description.SafeFileName();
            string albumFolder = albumRoot + albumName;

            if (photo != null && photo.Length > 0)
            {

                //if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + category.Pic))
                //{
                //  File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + category.Pic);
                //}

                //if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + category.Thumbnail))
                //{
                //  File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + category.Thumbnail);
                //}

                if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + category.Description.SafeFileName()))
                    Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/album/" + category.Description.SafeFileName());
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 200 };
                var resized = new string[1];

                fileName = _uploadService.UploadPicResize(
                                                          photo, "/content/files/album/" + category.Description.SafeFileName(),
                                                          1200,
                                                          ref fileSize,
                                                          size,
                                                          EnumC.Dimensions.Width,
                                                          ref resized
                                                         );

                category.Pic = category.Description.SafeFileName() + "/" + fileName;
                category.Thumbnail = category.Description.SafeFileName() + "/" + size[0] + "/" + resized[0];

            }

            string zipPath = albumFolder + "/" + albumName;

            if (mp364 != null)
            {
                if (Directory.Exists(albumFolder + "/64"))
                    Directory.Delete(albumFolder + "/64", true);

                Directory.CreateDirectory(albumFolder + "/64");
                if (File.Exists(zipPath + "64.zip"))
                {
                    File.Delete(zipPath + "64.zip");

                }


                var mp364Filename = _uploadService.UploadFile(mp364, "/content/files/album/" + category.Description.SafeFileName(), ref fileSize64,
                  new[] { "application/zip", "application/octet-stream", "application/zip-compressed", "application/x-zip-compressed" }, category.Description.SafeFileName() + "64.zip");
                category.ZipMp364Size = fileSize64.ToString();

                category.ZipMp364 = albumName + "/" + mp364Filename;
                using (var archive = ZipFile.OpenRead(zipPath + "64.zip"))
                {

                    var entries = archive.Entries
                      .OrderBy(p => int.Parse(Regex.Match(p.Name, @"^\d+").ToString() == "" ? "0" : Regex.Match(p.Name, @"^\d+").ToString()))
                      //  .OrderBy(p => p.Name)

                      .ToList();

                    for (var index = 0; index < entries.Count; index++)
                    {
                        var entry = entries[index];
                        if (entry.FullName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                        {
                            var fileName = entry.FullName.Replace(".mp3", "").SafeFileName();
                            // Gets the full path to ensure that relative segments are removed.
                            var destinationPath = Path.GetFullPath(Path.Combine(albumFolder + "/64", fileName + ".mp3"));

                            var music = category.Contents.FirstOrDefault(
                                          p => p.Title == fileName
                                          ||
                                            p.Title == entry.FullName.Replace(".mp3", "")
                                          ||
                                            Regex.Replace(p.Title, @"^[\d-]*\s*", "", RegexOptions.Multiline)
                                            == Regex.Replace(entry.FullName.Replace(".mp3", ""), @"^[\d-]*\s*", "", RegexOptions.Multiline)

                                               ) ?? category.Contents.SingleOrDefault(p => p.Priority == index);

                            if (music != null) music.Mp364 = albumName + "/64/" + fileName + ".mp3";
                            else
                            {

                                category.Contents.Add(new Entities.Content.Content
                                {
                                    Title = entry.FullName.Replace(".mp3", ""),
                                    Mp364 = albumName + "/64/" + fileName + ".mp3",
                                    Priority = index,
                                    TypeId = 1
                                });
                                _uow.SaveChanges();
                            }


                            entry.ExtractToFile(destinationPath);
                        }
                    }
                }

            }

            if (mp3128 != null)
            {
                if (File.Exists(zipPath + "128.zip"))
                    File.Delete(zipPath + "128.zip");




                var mp3128Filename = _uploadService.UploadFile(mp3128, "/content/files/album/" + category.Description.SafeFileName(), ref fileSize128,
                  new[] { "application/zip", "application/octet-stream", "application/zip-compressed", "application/x-zip-compressed" }
                  , category.Description.SafeFileName() + "128.zip");

                category.ZipMp3128Size = fileSize128.ToString();
                category.ZipMp3128 = albumName + "/" + mp3128Filename;


                foreach (var mzk in category.Contents.Where(p => p.Mp3128 != null))
                    if (File.Exists(Path.Combine(albumRoot, mzk.Mp3128)))
                        File.Delete(Path.Combine(albumRoot, mzk.Mp3128));


                using (var archive = ZipFile.OpenRead(zipPath + "128.zip"))
                {
                    var entries = archive.Entries
                      //  .OrderBy(p => Int32.Parse(Regex.Match(p.Name, @"^\d+").ToString()))
                      .OrderBy(p => int.Parse(Regex.Match(p.Name, @"^\d+").ToString() == "" ? "0" : Regex.Match(p.Name, @"^\d+").ToString()))

                      // .OrderBy(p => p.Name)
                      .ToList();

                    for (var index = 0; index < entries.Count; index++)
                    {

                        var entry = entries[index];
                        var fileName = entry.FullName.Replace(".mp3", "").SafeFileName();

                        if (entry.FullName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                        {
                            // Gets the full path to ensure that relative segments are removed.
                            var destinationPath = Path.GetFullPath(Path.Combine(albumFolder, fileName + ".mp3"));

                            var music = category.Contents.FirstOrDefault(p => p.Title == fileName || p.Title == entry.FullName.Replace(".mp3", "")

                                                                     ||
                                            Regex.Replace(p.Title, @"^[\d-]*\s*", "", RegexOptions.Multiline)
                                            == Regex.Replace(entry.FullName.Replace(".mp3", ""), @"^[\d-]*\s*", "", RegexOptions.Multiline)

                                                                               ) ??
                                        category.Contents.SingleOrDefault(p => p.Priority == index);
                            if (music != null) music.Mp3128 = albumName + "/" + fileName + ".mp3";

                            else
                            {
                                category.Contents.Add(new Entities.Content.Content
                                {
                                    Title = entry.FullName.Replace(".mp3", ""),
                                    Mp3128 = albumName + "/" + fileName + ".mp3",
                                    Priority = index,
                                    TypeId = 1
                                });
                            }

                            entry.ExtractToFile(destinationPath);
                        }
                    }
                }
            }

            if (mp3320 != null)
            {
                if (Directory.Exists(albumFolder + "/320"))
                    Directory.Delete(albumFolder + "/320", true);


                Directory.CreateDirectory(albumFolder + "/320");

                if (File.Exists(zipPath + "320.zip"))
                    File.Delete(zipPath + "320.zip");




                var mp3320Filename = _uploadService.UploadFile(mp3320, "/content/files/album/" + category.Description.SafeFileName(), ref fileSize320,
                  new[] { "application/zip", "application/octet-stream", "application/zip-compressed", "application/x-zip-compressed" }
                   , category.Description.SafeFileName() + "320.zip");

                category.ZipMp3320Size = fileSize320.ToString();
                category.ZipMp3320 = albumName + "/" + mp3320Filename;

                using (var archive = ZipFile.OpenRead(zipPath + "320.zip"))
                {
                    var entries = archive.Entries
                      // .OrderBy(p => p.Name)

                      // .OrderBy(p => Int32.Parse(Regex.Match(p.Name, @"^\d+").ToString()))
                      .OrderBy(p => int.Parse(Regex.Match(p.Name, @"^\d+").ToString() == "" ? "0" : Regex.Match(p.Name, @"^\d+").ToString()))

                      .ToList();

                    for (var index = 0; index < entries.Count; index++)
                    {
                        var entry = entries[index];
                        var fileName = entry.FullName.Replace(".mp3", "").SafeFileName();

                        if (entry.FullName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                        {
                            // Gets the full path to ensure that relative segments are removed.
                            var destinationPath = Path.GetFullPath(Path.Combine(albumFolder + "/320", fileName + ".mp3"));


                            var music = category.Contents.FirstOrDefault(p => p.Title == fileName || p.Title == entry.FullName.Replace(".mp3", "")

                                               ||
                                            Regex.Replace(p.Title, @"^[\d-]*\s*", "", RegexOptions.Multiline)
                                            == Regex.Replace(entry.FullName.Replace(".mp3", ""), @"^[\d-]*\s*", "", RegexOptions.Multiline)

                                                                               ) ?? category.Contents.SingleOrDefault(p => p.Priority == index);
                            if (music != null) music.Mp3320 = albumName + "/320/" + fileName + ".mp3";

                            else
                            {
                                category.Contents.Add(new Entities.Content.Content
                                {
                                    Title = entry.FullName.Replace(".mp3", ""),
                                    Mp3320 = albumName + "/320/" + fileName + ".mp3",
                                    Priority = index,
                                    TypeId = 1
                                });
                            }
                            entry.ExtractToFile(destinationPath);
                        }
                    }
                }
            }


            _Categories.Update(category);
            _uow.SaveChanges();

        }

        public void ResizeCategory(Category category)
        {

            // if (photo != null && photo.Length > 0)
            {
                if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + category.Description.SafeFileName()))
                    Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/album/" + category.Description.SafeFileName());
                string fileName = "";
                long fileSize = 0;
                //   var size = new[] { 200 };
                //  var resized = new string[1];
                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + category.Thumbnail))
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + category.Thumbnail);


                fileName = _uploadService.ResizeImage(
                   //  "bannertojih.jpg",
                   category.Pic,
                  "/content/files/album/", category.Description.SafeFileName(), 200, EnumC.Dimensions.Width);

                category.Thumbnail = category.Description.SafeFileName() + "/200/" + fileName;


                _Categories.Update(category);
                _uow.SaveChanges();

            }



            //_Categories.Update(category);
            //  _uow.SaveChanges();
        }


        public void Delete(int id)
        {
            var itemToRemove = _Categories.Find(id);
            if (itemToRemove != null)
            {

                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + itemToRemove.Pic))
                {
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + itemToRemove.Pic);
                }

                if (File.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + itemToRemove.Thumbnail))
                {
                    File.Delete(_hostingEnvironment.WebRootPath + "/content/files/album/" + itemToRemove.Thumbnail);
                }

                _Categories.Remove(itemToRemove);
                _uow.SaveChanges();
            }
        }
    }
}
