using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicProject.Services.Contracts.Content;
using MusicProject.ViewModels.Api;

namespace MusicProject.API.Controllers.Api
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class TagController : ControllerBase
  {
    private readonly ITagService _tagService;
    private readonly ICategoryService _categoryService;

    public TagController(
      ITagService tagService,
      ICategoryService categoryService
    )
    {
      _tagService = tagService;
      _categoryService = categoryService;
    }

    [HttpPost]
    public ActionResult GetAllArtist([FromBody] PageLimitViewModel model)
    {
      var tag = _tagService.GetPagedTagsAsync(
        model.PageNumber,
        model.PageSize,
        SortOrder.Ascending,
        "artist", null, "alphabet",
        true, null).Result.Tags.ToList();



      var a = new
      {
        artists = tag.Select(p => new
        {
          Id = p.Id,
          Title = p.Title,
          Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
          Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,
          LikeCount = p.LikeCount,
          ShareLink = "http://mousigha.com/artist/detail/" + p.Id,

          SubTitle = string.IsNullOrEmpty(p.Content) ? "" :

            Regex.Replace(p.Content, "<.*?>", string.Empty).Length > 200 ?
            Regex.Replace(p.Content, "<.*?>", string.Empty).Substring(0, 200).Replace("&nbsp;", " ") :
            Regex.Replace(p.Content, "<.*?>", string.Empty)
        }),
        selected = model.PageNumber == 1
          ? _tagService.GetAllBySelected("artist").Select(p => new
          {
            Id = p.Id,
            Title = p.Title,
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
            Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,
            ShareLink = "http://mousigha.com/artist/detail/" + p.Id,

            LikeCount = p.LikeCount
          })
          : null,

        popular = model.PageNumber == 1
          ? _tagService.GetPagedTagsAsync(1, 10, SortOrder.Descending,
            "artist", null, "popular", true, null).Result.Tags.ToList().Select(p => new
            {
              Id = p.Id,
              Title = p.Title,
              Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
              Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,
              LikeCount = p.LikeCount,
              ShareLink = "http://mousigha.com/artist/detail/" + p.Id
            })
          : null
      };
      return Ok(a);
    }

    [HttpPost]
    public async Task<ActionResult> GetArtist([FromBody] PageLimitViewModel model)
    {

      var artist = await _tagService.FindByIdAsync(model.Id);

      var artist1 = new
      {
        Id = artist.Id,
        Title = artist.Title,
        Pic = string.IsNullOrEmpty(artist.Pic) ? "" : "http://mousigha.com/content/files/tag/" + artist.Pic,
        Thumbnail = string.IsNullOrEmpty(artist.Thumbnail) ? "" : "http://mousigha.com/content/files/tag/" + artist.Thumbnail,
        Content = string.IsNullOrEmpty(artist.Content) ? "" : artist.Content,
        LikeCount = artist.LikeCount,
        VisitCount = artist.VisitCount,
        ShareLink = "http://mousigha.com/artist/detail/" + artist.Id,

        albums = artist.CategoryTags.ToList().Select(p => p.Category).ToList().Where(p => p.IsPublish).Select(p => new
        {
          Id = p.Id,
          Title = p.Title,
          Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
          Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/album/" + p.Pic,
          LikeCount = p.LikeCount,
          p.VisitCount,
          ShareLink = "http://mousigha.com/album/detail/" + p.Id


        })

      };
      return Ok(artist1);
    }



    [HttpPost]
    public async Task<ActionResult> GetStyleAndMusic([FromBody] PageLimitViewModel model)
    {
      var aa = (await _categoryService.GetPagedCategoryAsync(model.PageNumber, model.PageSize, SortOrder.Descending,
        "1", true, null, model.Id)).CategoryItem.ToList();

      var album = aa.Select(p => new AppHomeViewModel
      {
        Id = p.Id,
        Title = p.Title,
        Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
        Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/album/" + p.Pic,
        LikeCount = p.LikeCount,
        VisitCount = p.VisitCount,
        ShareLink = "http://mousigha.com/album/detail/" + p.Id,
          Artist = p.CategoryTags?
                        .Where(t => t.Type == "artist").Select(art => new ArtistViewModel
                        {
                            ArtistId = art.Tag.Id,
                            ArtistName = art.Tag.Title
                        }).ToList(),

      }).ToList();

      if (model.PageNumber == 1)
      {
        var artist = await _tagService.FindByIdAsync(model.Id);
        var artist1 = new
        {
          Id = artist.Id,
          Title = artist.Title,
          Pic = string.IsNullOrEmpty(artist.Pic) ? "" : "http://mousigha.com/content/files/tag/" + artist.Pic,
          Thumbnail = string.IsNullOrEmpty(artist.Thumbnail) ? "" : "http://mousigha.com/content/files/tag/" + artist.Thumbnail,
          Content = string.IsNullOrEmpty(artist.Content) ? "" : artist.Content,
          LikeCount = artist.LikeCount,
          VisitCount = artist.VisitCount,
          ShareLink = "http://mousigha.com/style/detail/" + artist.Id,

          albums = album
        };
        return Ok(artist1);
      }
      else
      {
        var artist1 = new
        {
          albums = album
        };
        return Ok(artist1);
      }
    }
  }
}