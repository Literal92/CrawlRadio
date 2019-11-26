using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using cloudscribe.Web.Pagination;
using MusicProject.Entities.Comment;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Comment
{
    public class PagedCommentListViewModel
    {
        public PagedCommentListViewModel()
        {
            Paging = new PaginationSettings();
        }

        public List<Entities.Comment.Comment> Comments { get; set; }
        [DisplayName("صفحه جاری")]
        public int CurrentPage { get; set; }
        public PaginationSettings Paging { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public string Body { get; set; }
        public bool? Ispublished { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int Id { get; set; }
        public bool State { get; set; }

    }
}