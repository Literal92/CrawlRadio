using System.Collections.Generic;
using MusicProject.Entities.Identity;
using cloudscribe.Web.Pagination;

namespace MusicProject.ViewModels.Content
{
    public class PagedContentItemsViewModel
    {
        public PagedContentItemsViewModel()
        {
            Paging = new PaginationSettings();
        }

        public List<Entities.Content.Content> Contents { get; set; }
        public List<Entities.Content.Main> Mains { get; set; }
        public List<Entities.Content.ContentSelected> SelectedContents { get; set; }

        public string Type { get; set; } = string.Empty;
        public string IsArchive { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public int ArtistId { get; set; } 
        public int StyleId { get; set; }
        public int MusicId { get; set; }
        public string CotentText { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public int ListId { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
