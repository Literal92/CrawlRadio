using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicProject.Services.Contracts.Content;
using MusicProject.ViewModels.Api;

namespace MusicProject.API.Controllers.Api
{

 //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

  [Route("api/[controller]/[action]")]
  [ApiController]
  public class AlbumController : ControllerBase
  {
    private readonly ICategoryService _categoryService;
    private readonly IContentService _contentService;
    private readonly IContentFileService _contentFileService;


    public AlbumController(
      ICategoryService categoryService,
      IContentService contentService,
        IContentFileService contentFileService
      )
    {
      _categoryService = categoryService;
      _contentService = contentService;
      _contentFileService = contentFileService;
    }


    [HttpPost]
    public async Task<ActionResult> GetAlbumByListId(FavParameterViewModel model)
    {
      var music = (await _contentService.GetByListIdAsync(model.MusicId.ToList())).Select(a => new
      {
        a.Id,
        AlbumId = a.CategoryId,
        AlbumName = a.Category.Title,
        Artist = (a.Category?.CategoryTags != null) ? (string.Join(",", a.Category.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(p => p.Title))) : null,
        ArtistId = (a.Category?.CategoryTags != null) ? (string.Join(",", a.Category.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(p => p.Id))) : null,
        Mp3128 = string.IsNullOrEmpty(a.Mp3128) ? "" : "http://mousigha.com/content/files/album/" + a.Mp3128,
        Mp364 = string.IsNullOrEmpty(a.Mp364) ? "" : "http://mousigha.com/content/files/album/" + a.Mp364,
        Mp3320 = string.IsNullOrEmpty(a.Mp3320) ? "" : "http://mousigha.com/content/files/album/" + a.Mp3320,

        a.ContentText,
        Title = Regex.Replace(a.Title, @"^[\d-]*\s*", "", RegexOptions.Multiline),
        Thumbnail = string.IsNullOrEmpty(a.Category.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + a.Category.Thumbnail,
        Pic = string.IsNullOrEmpty(a.Category.Pic) ? "" : "http://mousigha.com/content/files/album/" + a.Category.Pic,
        ShareLink = "http://mousigha.com/album/detail/" + a.Id,
        a.VisitCount,
        a.LikeCount
      });


      var cats = await _categoryService.GetByListIdAsync(model.Id.ToList());

      var appHomeViewModels = cats.Where(p => p.IsPublish).Select(
        p => new
        {
          Id = p.Id,
          Title = p.Title,
          Artist = p.CategoryTags != null ? (string.Join(",", p.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title))) : null,
          ArtistId = (p.CategoryTags != null) ? (string.Join(",", p.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Id))) : null,

          Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
          Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/album/" + p.Pic,

          Kind = "fav",
          TypeName = "علاقه مندی ها",
          LikeCount = p.LikeCount,
          ShareLink = "http://mousigha.com/album/detail/" + p.Id,
          VisitCount = p.VisitCount,
          Musics = p.Contents?.Select(a => new
          {
            a.Id,
            Mp3128 = string.IsNullOrEmpty(a.Mp3128) ? "" : "http://mousigha.com/content/files/album/" + a.Mp3128,
            Mp364 = string.IsNullOrEmpty(a.Mp364) ? "" : "http://mousigha.com/content/files/album/" + a.Mp364,
            Mp3320 = string.IsNullOrEmpty(a.Mp3320) ? "" : "http://mousigha.com/content/files/album/" + a.Mp3320,

            a.ContentText,
            Title = Regex.Replace(a.Title, @"^[\d-]*\s*", "", RegexOptions.Multiline),
            Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/album/" + p.Pic,
            ShareLink = "http://mousigha.com/content/files/album/" + a.Mp364,

            a.VisitCount,
            a.LikeCount
          }),
          Tags = p.CategoryTags?.Where(ct => ct.Type == "tag").Select(t => new
          {
            t.Tag.Id,
            t.Tag.Title
          }),

          Videos = _contentFileService.GetByCatIdAsync(p.Id).Result.Select(v => new
          {
            v.Id,
            v.Title,
            FileName = string.IsNullOrEmpty(v.FileName) ? "" : "http://mousigha.com/content/files/album/" + v.FileName,
            Pic = string.IsNullOrEmpty(v.Pic) ? "" : "http://mousigha.com/content/files/album/" + v.Pic,
            Thumbnail = string.IsNullOrEmpty(v.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + v.Thumbnail,
            v.Ext,
            v.VisitCount,
            v.ContentText,
            ShareLink = "http://mousigha.com/video/detail/" + v.Id,
            v.LikeCount
          })
        });

      return Ok(new
      {
        albums = appHomeViewModels,
        music
      });
    }


    [HttpPost]
    public async Task<ActionResult> GetAlbumById([FromBody]  PageLimitViewModel model)
    {

      var p = await _categoryService.FindByIdAsync(model.Id, model.FirstVisit);
      var videos = await _contentFileService.GetByCatIdAsync(model.Id);
      string artist = p.CategoryTags != null
        ? (string.Join(",", p.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title)))
        : null;
      var album = new
      {
        p.Id,
        p.Title,
        Artist = artist,
        p.ContentText,
        p.Description,
        p.VisitCount,
        Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
        Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/album/" + p.Pic,
        ArtistId = p.CategoryTags?.FirstOrDefault(t => t.Type == "artist")?.TagId,

        ShareLink = "http://mousigha.com/album/detail/" + p.Id,

          Zip128 = p.ZipMp3128 == null ? (p.Contents.Any(s => s.Mp3128 != null) ? "https://www.mousigha.com/album/download?type=128&id=" + p.Id : "") : "https://www.mousigha.com/content/files/album/" + p.ZipMp3128,
          Zip64 = p.ZipMp364 == null ? (p.Contents.Any(s => s.Mp364 != null) ? "https://www.mousigha.com/album/download?type=64&id=" + p.Id : "") : "https://www.mousigha.com/content/files/album/" + p.ZipMp364,
          Zip320 = p.ZipMp3320 == null ? (p.Contents.Any(s => s.Mp3320 != null) ? "https://www.mousigha.com/album/download?type=320&id=" + p.Id : "") : "https://www.mousigha.com/content/files/album/" + p.ZipMp3320,

        p.LikeCount,
        Musics = p.Contents.Select(a => new
        {
          a.Id,
          Mp3128 = string.IsNullOrEmpty(a.Mp3128) ? "" : "http://mousigha.com/content/files/album/" + a.Mp3128,
          Mp364 = string.IsNullOrEmpty(a.Mp364) ? "" : "http://mousigha.com/content/files/album/" + a.Mp364,
          Mp3320 = string.IsNullOrEmpty(a.Mp3320) ? "" : "http://mousigha.com/content/files/album/" + a.Mp3320,


          a.ContentText,
          Title = Regex.Replace(a.Title, @"^[\d-]*\s*", "", RegexOptions.Multiline),
          Artist = artist,
          Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
          Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/album/" + p.Pic,
          a.VisitCount,
          ShareLink = "http://mousigha.com/content/files/album/" + a.Mp364,
          AlbumName = p.Title,
          a.LikeCount
        }),

        Tags = p.CategoryTags?.Where(ct => ct.Type == "tag").Select(t => new
        {
          t.Tag.Id,
          ShareLink = "http://mousigha.com/share",

          Title = "#" + t.Tag.Title.Replace(" ", "_")
        }),

        Videos = videos.Select(v => new
        {
          v.Id,
          v.Title,
          FileName = string.IsNullOrEmpty(v.FileName) ? "" : "http://mousigha.com/content/files/album/" + v.FileName,
          Pic = string.IsNullOrEmpty(v.Pic) ? "" : "http://mousigha.com/content/files/album/" + v.Pic,
          Thumbnail = string.IsNullOrEmpty(v.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + v.Thumbnail,
          v.Ext,
          v.VisitCount,
          v.ContentText,
          ShareLink = "http://mousigha.com/video/detail/" + v.Id,
          v.LikeCount
        })
      };
      return Ok(album);
    }
  }
}