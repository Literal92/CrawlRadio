using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MusicProject.ViewModels.Content;

namespace MusicProject.Services.Content
{
    public class EfTagService : ITagService
    {
        IUnitOfWork _uow;
        readonly DbSet<Entities.Content.Tag> _Tags;
        private readonly IUploadService _uploadService;

        public EfTagService(IUnitOfWork uow,
          IUploadService uploadService

          )
        {
            _uow = uow;
            _uploadService = uploadService;

            _uow.CheckArgumentIsNull(nameof(_uow));

            _Tags = _uow.Set<Entities.Content.Tag>();
        }




        public IList<Tag> GetStartBy(string term, int top)
        {
            return _Tags.Where(p => p.Title.StartsWith(term)).Take(top).ToList();
        }

        public async Task<IList<Tag>> GetSortedAsync(string type, int top)
        {
            return await _Tags.Where(p => p.Type == type).OrderByDescending(p => p.Id)
              //  .OrderByDescending(p => p.CategoryTags)
              .Take(top).ToListAsync();
        }

        public IList<Tag> GetByTitle(string title, string type)
        {
            return _Tags.Where(p => p.Title == title && p.Type == type).ToList();
        }
        public IList<Tag> GetStartBy(string term, int top, string type)
        {
            return _Tags.Where(p => p.Title.StartsWith(term) && p.Type == type).Take(top).ToList();
        }

        public int AddNewTag(Tag tag, IFormFile photo)
        {

            if (photo != null)
            {
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 300 };
                var resized = new string[1];
                fileName = _uploadService.UploadPicResize(
                                                          photo, "/content/files/tag",
                                                         2000,
                                                          ref fileSize,
                                                          size,
                                                          EnumC.Dimensions.Width,
                                                          ref resized
                                                         );
                tag.Pic = fileName;
                tag.Thumbnail = size[0] + "/" + resized[0];
            }
            _Tags.Add(tag);
            _uow.SaveChanges();
            return tag.Id;
        }

        public IList<Tag> GetAllSelected(string typeId)
        {
            return _Tags.Where(p => p.Type == typeId && p.IsSelected).OrderBy(p => p.Id).ToList();
        }

        public async Task<IList<Tag>> GetAllByTypeAsync(string typeId)
        {
            return await _Tags.Where(p => p.Type == typeId).OrderBy(p => p.Id).ToListAsync();

        }

        public void UpdateTag(Tag tag, IFormFile photo)
        {

            if (photo != null && photo.Length > 0)
            {
                string fileName = "";
                long fileSize = 0;
                var size = new[] { 300 };
                var resized = new string[1];
                fileName = _uploadService.UploadPicResize(
                                                          photo, "/content/files/tag",
                                                         2000,
                                                          ref fileSize,
                                                          size,
                                                          EnumC.Dimensions.Width,
                                                          ref resized
                                                         );
                tag.Pic = fileName;
                tag.Thumbnail = size[0] + "/" + resized[0];
            }

            _Tags.Update(tag);
            _uow.SaveChanges();



        }


        public IList<Tag> GetPagedTags(int pageNumber, int pageSize, string type, string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                title = " " + title.Trim() + " ";
            }

            var offset = (pageSize * pageNumber) - pageSize;
            var query = string.IsNullOrWhiteSpace(type) ? _Tags :
              _Tags.Where(l => l.Type == type);

            //query = string.IsNullOrWhiteSpace(title) ? query :
            //  query.Where(l => l.Title.Contains(title)
            //                   //  ||l.Content.Contains(title)
            //                   || l.EnglishTitle.Contains(title)
            //                   );

            query = string.IsNullOrWhiteSpace(title) ? query :
            query.Where(l => l.Title.Contains(title) || l.EnglishTitle.Contains(title) || l.Content.Contains(title));

            query = query.Where(l => l.IsPublish == true);

            query = query.OrderByDescending(x => x.Id);

            return query.Skip(offset).Take(pageSize).ToList();

        }

        public async Task<PagedTagItemsViewModel> GetPagedTagsAsync(int pageNumber, int pageSize, SortOrder sortOrder,
          string type, string title, string orderBy = "last",
          bool? isPublish = null, bool? isSelected = null)
        {
            if (!string.IsNullOrEmpty(title))
            {
                title = " " + title.Trim() + " ";
            }

            var offset = (pageSize * pageNumber) - pageSize;

            var query = string.IsNullOrWhiteSpace(type) ? _Tags :
              _Tags.Where(l => l.Type == type);

            query = string.IsNullOrWhiteSpace(title) ? query :
              query.Where(l => l.Title.Contains(title.Trim()) || l.EnglishTitle.Contains(title.Trim()) || l.Content.Contains(title));




            query = isPublish == null ? query :
              query.Where(l => l.IsPublish == isPublish);

            query = isSelected == null ? query :
                  query.Where(l => l.IsSelected == isSelected);

            if (orderBy == "last" || orderBy == "date")
            {
                query = query.OrderByDescending(x => x.Id);

            }
            else if (orderBy == "album")
            {
                query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.CategoryTags.Count) : query.OrderBy(x => x.CategoryTags.Count);
            }
            else if (orderBy == "alphabet")
            {

                query = query.OrderBy(x => x.Title);
            }
            else if (orderBy == "popular")
            {
                query = query.OrderByDescending(x => x.LikeCount).ThenByDescending(p => p.VisitCount);
            }
            else
            {
                query = query.OrderByDescending(x => x.CategoryTags.Count);
            }

            query = query.Select(p => new Tag
            {
                Title = p.Title,
                Id = p.Id,
                EnglishTitle = p.EnglishTitle,
                Link = p.Link,
                Pic = p.Pic,
                ChildCount = p.Type != "cat" ? p.CategoryTags.Count.ToString() : p.ContentListTags.Count().ToString(),
                Type = p.Type,
                Thumbnail = p.Thumbnail,
                IsPublish = p.IsPublish,
                IsSelected = p.IsSelected,
                VisitCount = p.VisitCount,
                LikeCount = p.LikeCount

            });
            return new PagedTagItemsViewModel
            {
                Paging =
        {
          TotalItems = await query.CountAsync().ConfigureAwait(false)
        },
                Tags = await query.Skip(offset).Take(pageSize).ToListAsync()
            };

        }
        

        public IList<Tag> GetStartBy(int pageNumber, int pageSize,
          string type, string title)        {      

            var offset = (pageSize * pageNumber) - pageSize;

            var query = string.IsNullOrWhiteSpace(type) ? _Tags :
              _Tags.Where(l => l.Type == type);

            query = string.IsNullOrWhiteSpace(title) ? query :
              query.Where(l => l.Title.Contains(title.Trim()) );         
       
            query = query.Select(p => new Tag
            {
                Title = p.Title,
                Id = p.Id,
                EnglishTitle = p.EnglishTitle,
                Link = p.Link,
                Pic = p.Pic,
                ChildCount = p.Type != "cat" ? p.CategoryTags.Count.ToString() : p.ContentListTags.Count().ToString(),
                Type = p.Type,
                Thumbnail = p.Thumbnail,
                IsPublish = p.IsPublish,
                IsSelected = p.IsSelected,
                VisitCount = p.VisitCount,
                LikeCount = p.LikeCount
            });
            return query.ToList();
        }

        public async Task<Tag> FindByIdAsync(int id)
        {
            return await _Tags.Include(p => p.CategoryTags).ThenInclude(cat => cat.Category).Include(x => x.ContentListTags).ThenInclude(pl => pl.ContentList).SingleOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Tag> LikeTag(int id)
        {
            var tag = await _Tags.FindAsync(id);
            tag.LikeCount = tag.LikeCount + 1;
            await _uow.SaveChangesAsync();
            return tag;
        }
        public void Delete(int id)
        {
            var itemToRemove = _Tags.FirstOrDefault(x => x.Id.Equals(id));
            if (itemToRemove != null)
            {
                _Tags.Remove(itemToRemove);
                _uow.SaveChanges();
            }
        }

        public async Task<IList<Tag>> GetAllBySelectedAsync(string typeId)
        {
            return await _Tags.Where(p => p.Type == typeId && p.IsSelected & p.IsPublish).OrderBy(p => p.Id).ToListAsync();
        }


        public IList<Tag> GetAllBySelected(string typeId)
        {
            return _Tags.Where(p => p.Type == typeId && p.IsSelected & p.IsPublish).OrderBy(p => p.Id).ToList();
        }


        public int GetCount(string typeId, bool? isPublish = null)
        {
            var query = _Tags.Where(p => p.Type == typeId);
            query = isPublish == null ? query : query.Where(p => p.IsPublish == isPublish);
            return query.ToList().Count;
        }
        public int GetCount(string typeId,string term)
        {
            var query = _Tags.Where(p => p.Type == typeId);
            query = term == null ? query : query.Where(l => l.Title.Contains(term.Trim()) || l.EnglishTitle.Contains(term.Trim()) || l.Content.Contains(term));
            return query.ToList().Count;
        }
    }
}
