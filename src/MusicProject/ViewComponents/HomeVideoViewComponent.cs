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
  public class HomeVideoViewComponent : ViewComponent
  {
    private readonly IContentFileService _contentFileService;



    public HomeVideoViewComponent(
      IContentFileService contentFileService

      )
    {
      _contentFileService = contentFileService;

    }
    public async Task<IViewComponentResult> InvokeAsync()
    {

      var lastVideo = (await _contentFileService.GetTopByTypeAsync("video", 1, 10, "", "")).Select(p =>
        new ContentFileViewModel
        {
          Id = p.Id,
          Type = p.Type,
          Title = p.Title,
          FileName = p.FileName,
          ContentId = p.ContentId,
          ContentText =
            p.Content.Category.CategoryTags != null
              ? (string.Join(",",
                p.Content.Category.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag)
                  .Select(a => a.Title)))
              : "",
          Description = p.Description,
          Ext = p.Ext,
          FileSize = p.FileSize,
          LikeCount = p.LikeCount,
          PhotoFileName = p.Pic,
          Thumbnail = p.Thumbnail,
          Pic = p.Pic,
          VideoFileName = p.FileName,
          VisitCount = p.VisitCount
        }).ToList();



      return View(lastVideo);
    }
  }
}
