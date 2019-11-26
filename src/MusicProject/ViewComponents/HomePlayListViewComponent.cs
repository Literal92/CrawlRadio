using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts.Content;
using MusicProject.ViewModels.Content;
using System.Linq;
using System.Threading.Tasks;

namespace MusicProject.ViewComponents
{
    public class HomePlayListViewComponent : ViewComponent
    {
        private readonly IContentListService _contentListService;
        public HomePlayListViewComponent(IContentListService contentListService, IUnitOfWork unit)
        {
            _contentListService = contentListService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var contentViewModel = new ContentListCollectionViewModel
            {
                SelectedContentList = _contentListService.GetTopByType("1",true).ToList().Select(p =>
             new ContentListViewModel()
             {
                 Id = p.Id,
                 Pic = p.Pic,
                 Thumbnail = p.Thumbnail,
                 Title = p.Title,
                 IsPublish = p.IsPublish,
                 CatPL = _contentListService.GetCategoryByPLId(p.Id),
                 Contents = _contentListService.GetMusicsFiles(p.Id).Result.ToList(),

             }).ToList()
            };

            return View(contentViewModel);
        }

    }
}
