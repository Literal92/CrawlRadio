using Microsoft.AspNetCore.Mvc;
using MusicProject.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts.Content;
using MusicProject.ViewModels.Content;

namespace MusicProject.ViewComponents
{
  public class HomeSliderViewComponent : ViewComponent
  {
    private readonly ICategoryService _categoryService;
    private readonly IContentService _contentService;
    private readonly ISliderService _sliderService;


    public HomeSliderViewComponent(
      IContentService contentService,
  ISliderService sliderService,
      ICategoryService categoryService

      )
    {
      _categoryService = categoryService;
      _contentService = contentService;
      _sliderService = sliderService;

    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
var slider1= (await _sliderService.GetAllAsync(2, "enable", 1, 20)).ToList();
      var slider = slider1.Select(p => new SliderViewModel
      {
        Link = p.Link,
        Thumbnail = p.Thumbnail,

        Title = Regex.Replace(p.Title, @"^[\d-]*\s*", "", RegexOptions.Multiline),
        Pic = p.Pic,
        State = p.State,
        SubTitle = p.SubTitle,
        Id = p.Link!=null?  (p.Link.ToLower().Contains("album/detail") ? Convert.ToInt32(Regex.Match(p.Link, @"\d+$").Value) : 0):0,
        IsAlbum = p.Link != null ? p.Link.ToLower().Contains("album/detail"):false,
        Category = p.Link != null ?( p.Link.ToLower().Contains("album/detail")
          ? (_categoryService.FindByIdAsync(Convert.ToInt32(Regex.Match(p.Link, @"\d+$").Value)).Result)
          : null):null,
        Musics = p.Link != null ?( p.Link.ToLower().Contains("album/detail")
          ? (_contentService.GetAllByCategoryAsync(Convert.ToInt32(Regex.Match(p.Link, @"\d+$").Value))).Result.ToList()
          .Select(c => new Content
          {
            Title = Regex.Replace(c.Title, @"^[\d-]*\s*", "", RegexOptions.Multiline),
            Id = c.Id,
            LikeCount = c.LikeCount

          }).ToList()
          : null):null
      }).ToList();



      return View(slider);
    }
  }
}
