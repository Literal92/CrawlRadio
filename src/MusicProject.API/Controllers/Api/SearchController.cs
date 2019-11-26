using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicProject.Services.Contracts.Content;
using MusicProject.ViewModels.Api;

namespace MusicProject.API.Controllers.Api
{
  //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

  [Route("api/[controller]/[action]")]
  [ApiController]
  public class SearchController : ControllerBase
  {
    private readonly IContentFileService _contentFileService;
    private readonly IContentService _contentService;
    private readonly ICategoryService _categoryService;
    private readonly ITagService _tagService;
    public SearchController(
      ICategoryService categoryService,
      IContentFileService contentFileService,
      ITagService tagService
      )
    {
      _categoryService = categoryService;
      _contentFileService = contentFileService;
      _tagService = tagService;
    }

    [HttpPost]
    public ActionResult GetAll([FromBody] PageLimitViewModel model)
    {
      if (string.IsNullOrEmpty(model.Term))
      {
        return Ok();
      }
      if (model.Kind == "album")
      {
        var albums = _categoryService.GetForSearch("1", model.PageNumber, model.PageSize, model.Term, true).Select(
             p => new AppHomeViewModel
             {
               Id = p.Id,
               Title = p.Title,
                 // Artist = p.CategoryTags != null ? (string.Join(",", p.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title))) : null,
                 Artist = p.CategoryTags?
                        .Where(t => t.Type == "artist").Select(art => new ArtistViewModel
                        {
                            ArtistId = art.Tag.Id,
                            ArtistName = art.Tag.Title
                        }).ToList(),
                 Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
               Kind = "album",
               ShareLink = "http://mousigha.com/album/detail/" + p.Id,

               VisitCount = p.VisitCount,
               LikeCount = p.LikeCount,
               TypeName = "آلبوم ها",

             });
        return Ok(albums);
      }

      if (model.Kind == "artist")
      {
        var artist = _tagService.GetPagedTags(model.PageNumber, model.PageSize, "artist", model.Term).Select(
          p => new AppHomeViewModel
          {
            Id = p.Id,
            Title = p.Title,
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
            Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,

            VisitCount = p.VisitCount,
            LikeCount = p.LikeCount,
            Kind = "artist",
            TypeName = "هنرمندان",
            ShareLink = "http://mousigha.com/artist/detail/" + p.Id,


            SubTitle = p.Content != null ? Regex.Replace(p.Content, "<.*?>", string.Empty).Length > 200 ?
              Regex.Replace(p.Content, "<.*?>", string.Empty).Substring(0, 200).Replace("&nbsp;", " ").Replace("&zwnj", " ").Replace("&zwj", " ") :
              Regex.Replace(p.Content, "<.*?>", string.Empty) : ""

          });
        return Ok(artist);
      }


      if (model.Kind == "style")
      {
        var style =
          _tagService.GetPagedTags(model.PageNumber, model.PageSize, "style", model.Term).Select(
          p => new AppHomeViewModel
          {
            Id = p.Id,
            Title = p.Title,
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
            Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,
            VisitCount = p.VisitCount,
            LikeCount = p.LikeCount,
            Kind = "style",
            ShareLink = "http://mousigha.com/style/detail/" + p.Id,


            TypeName = "سبک ها",
            SubTitle = Regex.Replace(p.Content, "<.*?>", string.Empty).Length > 200 ?
              Regex.Replace(p.Content, "<.*?>", string.Empty).Substring(0, 200).Replace("&nbsp;", " ").Replace("&zwnj", " ").Replace("&zwj", " ") :
              Regex.Replace(p.Content, "<.*?>", string.Empty)
          });

        return Ok(style.ToList());
      }


      if (model.Kind == "music")
      {
      
        var music =
          _tagService.GetPagedTags(model.PageNumber, model.PageSize, "music", model.Term).Select(
          p => new AppHomeViewModel
          {
            Id = p.Id,
            Title = p.Title,
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
            Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,
            VisitCount = p.VisitCount,
            LikeCount = p.LikeCount,
            Kind = "music",
            ShareLink = "http://mousigha.com/music/detail/" + p.Id,

            TypeName = "سازها",
            SubTitle = Regex.Replace(p.Content, "<.*?>", string.Empty).Length > 200 ?
              Regex.Replace(p.Content, "<.*?>", string.Empty).Substring(0, 200).Replace("&nbsp;", " ").Replace("&zwnj", " ").Replace("&zwj", " ") :
              Regex.Replace(p.Content, "<.*?>", string.Empty)
          });

        return Ok(music.ToList());
      }



      if (model.Kind == "video")
      {
        var artist = _contentFileService.GetTopByTypeAsync("video", model.PageNumber, model.PageSize, model.Term, "").Result.Select(
          p => new AppHomeViewModel
          {
            VisitCount = p.VisitCount,
            LikeCount = p.LikeCount,
            Id = p.Id,
            Title = p.Title,
            Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
            Video = string.IsNullOrEmpty(p.FileName) ? "" : "http://mousigha.com/content/files/album/" + p.FileName,
            Kind = "video",
            ShareLink = "http://mousigha.com/video/detail/" + p.Id,
            TypeName = "موزیک ویدیو"
          });
        return Ok(artist);
      }

      else
      {
        var albums = _categoryService.GetForSearch("1", model.PageNumber, model.PageSize, model.Term, true).Select(
          p => new AppHomeViewModel
          {
            VisitCount = p.VisitCount,
            LikeCount = p.LikeCount,
            Id = p.Id,
            Title = p.Title,
              //  Artist = p.CategoryTags != null ? (string.Join(",", p.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title))) : null,
              Artist = p.CategoryTags?
                        .Where(t => t.Type == "artist").Select(art => new ArtistViewModel
                        {
                            ArtistId = art.Tag.Id,
                            ArtistName = art.Tag.Title
                        }).ToList(),
              ShareLink = "http://mousigha.com/album/detail/" + p.Id,

            Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
            Type = "last",
            Kind = "album",
            TypeName = "آلبوم ها",

          });

        var artist = _tagService.GetPagedTags(model.PageNumber, model.PageSize, "artist", model.Term).Select(
          p => new AppHomeViewModel
          {
            Id = p.Id,
            Title = p.Title,
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
            Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,
            VisitCount = p.VisitCount,
            LikeCount = p.LikeCount,
            Kind = "artist",
            ShareLink = "http://mousigha.com/artist/detail/" + p.Id,
            TypeName = "هنرمندان",
            SubTitle = p.Content != null ? Regex.Replace(p.Content, "<.*?>", string.Empty).Length > 200 ?
              Regex.Replace(p.Content, "<.*?>", string.Empty).Substring(0, 200).Replace("&nbsp;", " ").Replace("&zwnj", " ").Replace("&zwj", " ") :
              Regex.Replace(p.Content, "<.*?>", string.Empty) : ""

          });

        var style = _tagService.GetPagedTags(model.PageNumber, model.PageSize, "style", model.Term).Select(
          p => new AppHomeViewModel
          {
            Id = p.Id,
            Title = p.Title,
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
            Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,
            VisitCount = p.VisitCount,
            LikeCount = p.LikeCount,
            Kind = "style",
            ShareLink = "http://mousigha.com/style/detail/" + p.Id,
            TypeName = "سبک ها",

          });

        var music = _tagService.GetPagedTags(model.PageNumber, model.PageSize, "music", model.Term).Select(
          p => new AppHomeViewModel
          {
            Id = p.Id,
            Title = p.Title,
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
            Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,
            VisitCount = p.VisitCount,
            LikeCount = p.LikeCount,
            Kind = "music",
            ShareLink = "http://mousigha.com/music/detail/" + p.Id,


            TypeName = "ساز ها",

          });

        style = style.Union(music);

        var video = _contentFileService.GetTopByTypeAsync("video", model.PageNumber, model.PageSize, model.Term, "").Result.Select(
          p => new AppHomeViewModel
          {
            VisitCount = p.VisitCount,
            LikeCount = p.LikeCount,
            Id = p.Id,
            Title = p.Title,
            Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
            Video = string.IsNullOrEmpty(p.FileName) ? "" : "http://mousigha.com/content/files/album/" + p.FileName,
            Kind = "video",
            ShareLink = "http://mousigha.com/video/detail/" + p.Id,
            TypeName = "موزیک ویدیو"
          });

        var merged = albums.Union(artist).ToList();
        merged = merged.Union(style.ToList()).ToList();
        merged = merged.Union(video.ToList()).ToList();
        return Ok(merged);
      }
    }
  }
}