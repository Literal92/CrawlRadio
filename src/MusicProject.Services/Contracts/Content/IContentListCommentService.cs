using MusicProject.Entities.Comment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicProject.Services.Contracts.Content
{
    public interface IContentListCommentService
    {
        Task AddNewCommentAsync(Comment comment);

        Task<int> GetCountAsync(
        int? type, int? contentList,
        bool? isPublish = null, string orderBy = "date");
    }
}
