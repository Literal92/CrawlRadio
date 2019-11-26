using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicProject.Services.Contracts.Content;
using MusicProject.ViewModels.Api;

namespace MusicProject.API.Controllers.Api
{

  //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

  [Route("api/[controller]/[action]")]
  [ApiController]
  public class VideoController : ControllerBase
  {
    private readonly IContentFileService _contentFileService;
    private readonly ITagService _tagService;

    public VideoController(
      IContentFileService contentFileService,
      ITagService tagService
      )
    {
      _contentFileService = contentFileService;
      _tagService = tagService;
    }

    [HttpPost]
    public ActionResult GetById([FromBody] PageLimitViewModel model)
    {
      var video = _contentFileService.FindByIdAsync(model.Id, model.FirstVisit).Result;
      var artistId = video.Content?.Category?.CategoryTags?.FirstOrDefault(t => t.Type == "artist")?.TagId;

      var rel =  _contentFileService.GetByTagIdAsync((int)artistId).Result.Where(p=>p.Id!=model.Id);
      
      var result = new
      {
        video = new
        {
          video.Id,
          Video = "http://mousigha.com/content/files/album/" + video.FileName,
          Pic = "http://mousigha.com/content/files/album/" + video.Thumbnail,
          Thumbnail = "http://mousigha.com/content/files/album/" + video.Thumbnail,
          ArtistId = video.Content?.Category?.CategoryTags?.FirstOrDefault(t => t.Type == "artist")?.TagId,

          video.Title,
          video.VisitCount,
          video.LikeCount,
          Description=video.ContentText,
          

          Artist = video.Content.Category.CategoryTags != null ?
                              (string.Join(",", video.Content.Category.CategoryTags.Where(t => t.Type == "artist")
                                .Select(x => x.Tag).Select(a => a.Title))) : null,
          ShareLink = "http://mousigha.com/video/detail/" + video.Id,

          Related = rel.Select(
           p => new
           {
             p.Id,
             Video = "http://mousigha.com/content/files/album/" + p.FileName,
             Pic = "http://mousigha.com/content/files/album/" + p.Thumbnail,
             Thumbnail = "http://mousigha.com/content/files/album/" + p.Thumbnail,
             p.Title,
             p.VisitCount,
             p.LikeCount,
             p.Description,
             //   
           })
        },
        ArtistVideo = rel.Select(
          p => new
          {
            p.Id,
            Video = "http://mousigha.com/content/files/album/" + p.FileName,
            Pic = "http://mousigha.com/content/files/album/" + p.Thumbnail,
            Thumbnail = "http://mousigha.com/content/files/album/" + p.Thumbnail,

            p.Title,
            p.VisitCount,
            p.LikeCount,
            p.Description,
            //  ShareLink = "http://mousigha.com/share"
          }
        )
      };
      return Ok(result);
    }


    [HttpPost]
    public async Task<ActionResult> GetVideoByArtist([FromBody] PageLimitViewModel model)
    {
      var artist = await _tagService.FindByIdAsync(model.Id);
      var videos = (await _contentFileService.GetByTagIdAsync(model.Id)).Select(p => new AppHomeViewModel
      {
        Id = p.Id,
        Title = p.Title,
        VideoArtist = artist.Title,
        Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
        Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
        Video = "http://mousigha.com/content/files/album/" + p.FileName,
        LikeCount = p.LikeCount,
        VisitCount = p.VisitCount,
        ShareLink = "http://mousigha.com/video/detail/" + p.Id


      });
      return Ok(videos);
    }

  }
}