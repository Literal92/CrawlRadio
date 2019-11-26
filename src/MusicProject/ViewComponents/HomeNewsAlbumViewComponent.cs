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
  public class HomeNewsAlbumViewComponent : ViewComponent
  {
    private readonly ICategoryService _categoryService;
    public HomeNewsAlbumViewComponent(

      ICategoryService categoryService

      )
    {
      _categoryService = categoryService;

    }
    public async Task<IViewComponentResult> InvokeAsync()
    {

      var lastMusic = (await _categoryService.GetTopByTypeAsync("1", 1, 12, null, true)).ToList().Select(p =>
        new CategoryViewModel()
        {
          Id = p.Id,
          Artists = p.CategoryTags.FirstOrDefault(artist => artist.Type == "artist")?.Tag.Title,
          Pic = p.Pic,
          Thumbnail = p.Thumbnail,
          Title = p.Title
        }).ToList();
    
      return View(lastMusic);
    }
  }
}
