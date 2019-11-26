using System;
using System.Collections.Generic;
using System.Text;

namespace MusicProject.ViewModels.Content
{
    public class ContentListCollectionViewModel
    {
        public List<ContentListViewModel> SelectedContentList { get; set; }
        public List<ContentListViewModel> ContentList { get; set; }
        public List<ContentListViewModel> PopularContentList { get; set; }
        public int CurrentPage { get; set; }
    }
}
