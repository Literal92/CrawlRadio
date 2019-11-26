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
  public class HomeStyleViewComponent : ViewComponent
  {
    private readonly ITagService _tagService;



    public HomeStyleViewComponent(
      ITagService tagService

      )
    {
      _tagService = tagService;

    }
    public async Task<IViewComponentResult> InvokeAsync()
    {

      var artist = (await _tagService.GetSortedAsync("style", 8)).ToList().Select(p => new TagViewModel()
        {
          Id = p.Id,
          Pic = p.Pic,
          Thumbnail = p.Thumbnail,

          Title = p.Title
        }
      ).ToList();



      return View(artist);
    }
  }
}
