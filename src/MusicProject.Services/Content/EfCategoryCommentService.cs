using Microsoft.EntityFrameworkCore;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Comment;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts.Content;

using MusicProject.ViewModels.Content;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MusicProject.Services.Content
{
    public class EfCategoryCommentService : ICategoryCommentService
    {
        IUnitOfWork _uow;
        readonly DbSet<CategoryComment> _commentCategory;
        readonly DbSet<Comment> _comments;
        readonly DbSet<Entities.Identity.User> _users;
        public EfCategoryCommentService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));
            _comments = _uow.Set<Comment>();
            _users = _uow.Set<Entities.Identity.User>();
            _commentCategory = _uow.Set<CategoryComment>();
        }


        public async Task AddNewCommentAsync(Comment cmt)
        {
            if (cmt.UserCommentId != 0)
            {
                CategoryComment comment;
                int cmnt = 0;
                var user = _users.Find(cmt.UserCommentId);
                var comments = new Comment
                {
                    Body = cmt.Body,
                    TypeId = 1,
                    IsPublished = cmt.UserCommentId == 1 ? true : false,
                    CreatedDateTime = DateTime.Now,
                    Name = user.FirstName,
                    Tell = user.PhoneNumber,
                    Email = user.Email,
                    UserCommentId = (int)cmt.UserCommentId,
                    ParentId = cmt.ParentId ?? null

                };
                await _comments.AddAsync(comments);
                if (cmt.ParentId != null)
                {
                    cmnt = _commentCategory.Where(x => x.CommentId == cmt.ParentId).First().CategoryId;
                }
                comment = new CategoryComment
                {
                    CategoryId = cmt.ParentId == null ? cmt.TypeId : cmnt,
                    CommentId = comments.Id
                };
                await _commentCategory.AddAsync(comment);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task<int> GetCountAsync(
          int? type, int? categoryId,
          bool? isPublish = null, string orderBy = "date")
        {
            int count = 0;
            var query = _commentCategory.Where(x => x.CategoryId == categoryId);
            foreach (var item in query)
            {
                var cmt = await _comments.Include(x => x.InverseParent).Where(c => c.Id == item.CommentId && c.TypeId == type).FirstAsync();
                if (cmt.ParentId == null && cmt.IsPublished)
                {
                    count++;
                    if (cmt.InverseParent != null)
                        foreach (var item2 in cmt.InverseParent)
                            if (item2.IsPublished)
                                count++;
                }
            }

            return count;
        }
        public async Task<PagedCategoryCommentViewModel> GetPagedCommentAsync(
          int pageNumber,
          int pageSize,
          SortOrder sortOrder,
          int? type,
          int? categoryId,
          bool? isPublish = null,
          string orderBy = "date",
          int? parent = null
          )
        {
            var query = _commentCategory;
            var offset = (pageSize * pageNumber) - pageSize;


            return new PagedCategoryCommentViewModel
            {
                Paging =
        {
          TotalItems = await query.CountAsync().ConfigureAwait(false)
        },
                CommentItem = await query.Skip(offset).Take(pageSize).Include(p => p.Category).ToListAsync().ConfigureAwait(false),
                TypeId = type,
                CategoryId = categoryId
            };
        }

        public void Delete(int id)
        {
            var itemToRemove = _commentCategory.Find(id);
            if (itemToRemove != null)
            {
                _commentCategory.Remove(itemToRemove);
                _uow.SaveChanges();
            }
        }
    }
}