using Microsoft.AspNetCore.Mvc;
using MusicProject.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicProject.Services.Contracts.Content;
using MusicProject.ViewModels.Content;

namespace MusicProject.ViewComponents
{
  public class HomePopularAlbumViewComponent : ViewComponent
  {
    private readonly ICategoryService _categoryService;
    public HomePopularAlbumViewComponent(

      ICategoryService categoryService

      )
    {
      _categoryService = categoryService;

    }
    public async Task<IViewComponentResult> InvokeAsync()
    {

      var moreLikes = (await _categoryService.GetTopByLikeAsync("1", 1, 12)).ToList().Select(p =>
        new CategoryViewModel()
        {
          Id = p.Id,
          Artists = p.CategoryTags.FirstOrDefault(artist => artist.Type == "artist")?.Tag.Title,
          Pic = p.Pic,
          Thumbnail = p.Thumbnail,
          Title = p.Title

        }).ToList();
    
      return View(moreLikes);
    }
  }
}
