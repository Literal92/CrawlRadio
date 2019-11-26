using DNTPersianUtils.Core;
using Microsoft.EntityFrameworkCore;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Comment;
using MusicProject.Entities.Content;
using MusicProject.Entities.Identity;
using MusicProject.Services.Contracts.Content;
using MusicProject.ViewModels.Comment;
using MusicProject.ViewModels.Content;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MusicProject.Services.Content
{
    public class EfCommentService : ICommentService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Comment> _Comment;
        private readonly DbSet<CategoryComment> _CategoryComments;
        private readonly DbSet<User> _Users;
        private readonly DbSet<Category> _Categories;
        private readonly DbSet<ContentListComment> _contentListComments;
        private readonly DbSet<ContentList> _contentLists;
        public EfCommentService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));
            _Comment = _uow.Set<Comment>();
            _CategoryComments = _uow.Set<CategoryComment>();
            _Users = _uow.Set<User>();
            _Categories = _uow.Set<Category>();
            _contentListComments = _uow.Set<ContentListComment>();
            _contentLists = _uow.Set<ContentList>();
        }

        public async Task<CommentListViewModel> GetCommentDetailsAsync(int id)
        {
            var comment = await _Comment.Include(p => p.InverseParent).Include(f => f.Parent).Where(x => x.Id == id).FirstAsync();
            var userName = _Users.Where(x => x.FirstName == comment.Name).First().FirstName;
            var type = comment.TypeId;
            var typeName = "";
            var typeId = 0;
            if (type == 1)
            {
                typeId = _CategoryComments.Where(x => x.CommentId == id).First().CategoryId;
                typeName = _Categories.Where(x => x.Id == typeId).First().Title;
            }
            if (type == 2)
            {
                typeId = _contentListComments.Where(x => x.CommentId == id).First().ContentListId;
                typeName = _contentLists.Where(x => x.Id == typeId).First().Title;
            }
            return new CommentListViewModel
            {
                Comment = comment,
                TypeName = typeName,
                UserName = userName,
                TypeId = typeId,
                Type = type
            };
        }

        public async Task<PagedCommentListViewModel> GetPagedCommentListAsync(int pageNumber, int pageSize, SortOrder sortOrder, int TypeId, string name, string body, bool? ispublished, string fromTime, string toTime)
        {
            var offset = (pageSize * pageNumber) - pageSize;
            IQueryable<Comment> query;
            if (TypeId == 0)
                query = _Comment;
            else
                query = _Comment.Where(t => t.TypeId == TypeId);
            string PersianDatetime;
            foreach (var item in _Comment)
            {
                PersianDatetime = item.CreatedDateTime.ToShortPersianDateString();
            }

            query = !string.IsNullOrEmpty(name) ? query.Where(h => h.Name.Contains(name)) : query;
            query = !string.IsNullOrEmpty(body) ? query.Where(h => h.Body.Contains(body)) : query;
            query = ispublished != null ? query.Where(h => h.IsPublished == ispublished) : query;
            query = fromTime != null ? query.Where(h => h.CreatedDateTime >= fromTime.ToGregorianDateTimeOffset()) : query;
            query = toTime != null ? query.Where(h => h.CreatedDateTime <= toTime.ToGregorianDateTimeOffset()) : query;
            query = query.Where(p => p.ParentId == null);
            query = query.Select(p => new Comment
            {
                Id = p.Id,
                TypeId = p.TypeId,
                Parent = p.Parent,
                ParentId = p.ParentId,
                Body = p.Body,
                Email = p.Email,
                Tell = p.Tell,
                CreatedDateTime = p.CreatedDateTime,
                Name = p.Name,
                Like = p.Like,
                Attachment = p.Attachment,
                IsPublished = p.IsPublished,
                Dislike = p.Dislike,
                InverseParent = p.InverseParent,
            });
            return new PagedCommentListViewModel
            {
                Paging =
        {
          TotalItems = await query.CountAsync().ConfigureAwait(false)
        },
                Comments = await query.Skip(offset).Take(pageSize).Include(p => p.InverseParent).Include(t => t.Parent).ToListAsync(),
            };
        }

        public async Task UpdatePublishedAsync(int commentId, bool commentState)
        {
            var toUpdate = await _Comment.FindAsync(commentId);
            toUpdate.IsPublished = commentState;
            _Comment.Update(toUpdate);
            await _uow.SaveChangesAsync();
        }

        public async Task<Comment> GetCommentAsync(int id)
        {
            var CommentInfo = await _Comment.FindAsync(id);
            return CommentInfo;
        }

        public async Task DeleteCommentAsync(int id)
        {
            var ItemtoDelete = await _Comment.Include(p => p.InverseParent).Where(x => x.Id == id).FirstAsync();
            if (ItemtoDelete.InverseParent != null)
            {
                foreach (var item in ItemtoDelete.InverseParent)
                {
                    _Comment.Remove(item);
                }
            }
            if (ItemtoDelete != null)
            {
                _Comment.Remove(ItemtoDelete);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task<Comment> LikeComment(int id)
        {
            var query = await _Comment.FindAsync(id);
            query.Like++;
            await _uow.SaveChangesAsync();
            return query;
        }

        public async Task<Comment> InverseLikeComment(int id)
        {
            var query = await _Comment.FindAsync(id);
            query.Like--;
            await _uow.SaveChangesAsync();
            return query;
        }

        public async Task<Comment> DisLikeComment(int id)
        {
            var query = await _Comment.FindAsync(id);
            query.Dislike++;
            await _uow.SaveChangesAsync();
            return query;
        }
        public async Task<Comment> InverseDisLikeComment(int id)
        {
            var query = await _Comment.FindAsync(id);
            query.Dislike--;
            await _uow.SaveChangesAsync();
            return query;
        }

        public async Task<PagedCommentViewModel> GetPagedCommentAsync(int pageNumber, int pageSize, SortOrder sortOrder, int? type, int? typeId)
        {
            var query = type == null ? _Comment :
              _Comment.Where(l => l.TypeId == Convert.ToInt32(type));

            if (typeId != null)
            {
                if (type == 1)
                {
                    query = _Comment.Include(p => p.InverseParent).Where(p => p.CategoryComments.Any(x => x.CategoryId == typeId)).Where(x => x.IsPublished == true);
                }
                if (type == 2)
                {
                    query = _Comment.Include(p => p.InverseParent).Where(p => p.ContentListComments.Any(x => x.ContentListId == typeId)).Where(x => x.IsPublished == true);
                }
            }

            var offset = (pageSize * pageNumber) - pageSize;

            query = sortOrder == SortOrder.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);

            var user = _Users.Find(query.First().UserCommentId);

            return new PagedCommentViewModel
            {
                Paging =
                        {
                          TotalItems = await query.CountAsync().ConfigureAwait(false)
                        },
                CommentItem = await query.Skip(offset).Take(pageSize).ToListAsync().ConfigureAwait(false),
                Type = type,
                TypeId = typeId,
            };
        }
    }
}
