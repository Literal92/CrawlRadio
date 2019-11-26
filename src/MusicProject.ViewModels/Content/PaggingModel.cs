using System;
using System.Collections.Generic;
using System.Text;

namespace MusicProject.ViewModels.Content
{
    public class PaggingModel
    {
        public int? Page { get; set; }
        public int? Id { get; set; }
        public string PagerSortBy { get; set; }
        public string PagerSortOrder { get; set; }
        public string SearchType { get; set; }
        public string SearchTerm = "album";// { get; set; }
        public int Type { get; set; }
        public int TypeId { get; set; }
        public int PlayId { get; set; }
    }
}
