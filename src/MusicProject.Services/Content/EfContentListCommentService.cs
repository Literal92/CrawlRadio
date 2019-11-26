using Microsoft.EntityFrameworkCore;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Comment;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts.Content;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicProject.Services.Content
{
    public class EfContentListCommentService : IContentListCommentService
    {
        readonly private IUnitOfWork _uow;
        readonly private DbSet<Entities.Identity.User> _users;
        readonly private DbSet<Comment> _comments;
        readonly private DbSet<ContentListComment> _contentListComments;
        public EfContentListCommentService(IUnitOfWork uow)
        {
            _uow = uow;
            _users = _uow.Set<Entities.Identity.User>();
            _comments = _uow.Set<Comment>();
            _contentListComments = _uow.Set<ContentListComment>();
        }

        public async Task AddNewCommentAsync(Comment cmt)
        {
            if (cmt.UserCommentId != 0)
            {
                ContentListComment comment;
                int cmnt = 0;
                var user = _users.Find(cmt.UserCommentId);
                var comments = new Comment
                {
                    Body = cmt.Body,
                    TypeId = 2,
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
                    cmnt = _contentListComments.Where(x => x.CommentId == cmt.ParentId).First().ContentListId;
                }
                comment = new ContentListComment
                {
                    ContentListId = cmt.ParentId == null ? cmt.TypeId : cmnt,
                    CommentId = comments.Id
                };
                await _contentListComments.AddAsync(comment);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task<int> GetCountAsync(
         int? type, int? contentList,
         bool? isPublish = null, string orderBy = "date")
        {
            int count = 0;
            var query = _contentListComments.Where(x => x.ContentListId == contentList);
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

        public void Delete(int id)
        {
            var itemToRemove = _contentListComments.Find(id);
            if (itemToRemove != null)
            {
                _contentListComments.Remove(itemToRemove);
                _uow.SaveChanges();
            }
        }
    }
}
