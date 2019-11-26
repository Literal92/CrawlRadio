using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MusicProject.Services.Contracts.Content;
using MusicProject.ViewModels.Api;



namespace MusicProject.API.Controllers.Api
{
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class HomeController : ControllerBase
  {
    private readonly IContentFileService _contentFileService;
    private readonly IAppSettingService _appSettingService;
    private readonly IContentService _contentService;
    private readonly ICategoryService _categoryService;
    private readonly ITagService _tagService;
    private readonly ISliderService _sliderService;
    private readonly IConfiguration _config;
    private readonly IDeviceService _deviceService;

    public HomeController(
      ICategoryService categoryService,
      IContentFileService contentFileService,
      IContentService contentService,
      ITagService tagService,
      ISliderService sliderService,
      IAppSettingService appSettingService,
      IConfiguration config,
      IDeviceService deviceService

    )
    {
      _categoryService = categoryService;
      _contentFileService = contentFileService;
      _contentService = contentService;
      _tagService = tagService;
      _sliderService = sliderService;
      _appSettingService = appSettingService;
      _deviceService = deviceService;
      _config = config;
    }



    [HttpPost]
    public ActionResult GetAll([FromBody]PageLimitViewModel model)
    {
      if (model.Kind == "album")
      {
        if (model.OrderBy == "last")
        {
          var morLiked = _categoryService.GetTopByType("1", model.PageNumber, model.PageSize, "", true).Select(
            p => new AppHomeViewModel
            {
              Id = p.Id,
              Title = p.Title,
                //   Artist = p.CategoryTags != null ? (string.Join(",", p.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title))) : null,
                Artist = p.CategoryTags?
                        .Where(t => t.Type == "artist").Select(art => new ArtistViewModel
                        {
                            ArtistId = art.Tag.Id,
                            ArtistName = art.Tag.Title
                        }).ToList(),

                Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
              Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/album/" + p.Pic,
              Type = "last",
              Kind = "album",
              TypeName = "آخرین ها",
              LikeCount = p.LikeCount,
              VisitCount = p.VisitCount,

              Rate = 5,
              ShareLink = "http://mousigha.com/album/detail/"+p.Id
            });
          return Ok(morLiked);
        }
        if (model.OrderBy == "like")
        {
          var lk = _categoryService.GetTopByLike("1", model.PageNumber, model.PageSize, true).Select(
            p => new AppHomeViewModel
            {
              Id = p.Id,
              Title = p.Title,
                //Artist = (string.Join(",", p.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title))),
                // Artist = p.CategoryTags != null ? (string.Join(",", p.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title))) : null,
                Artist = p.CategoryTags?
                        .Where(t => t.Type == "artist").Select(art => new ArtistViewModel
                        {
                            ArtistId = art.Tag.Id,
                            ArtistName = art.Tag.Title
                        }).ToList(),
                Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
              Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/album/" + p.Pic,
              Type = "like",
              Kind = "album",
              TypeName = "محبوب ترین ها",
              LikeCount = p.LikeCount,
              VisitCount = p.VisitCount,
              Rate = 5,
              ShareLink = "http://mousigha.com/album/detail/" + p.Id
            });
          return Ok(lk);
        }
      }

      if (model.Kind == "video")
      {
        var moreLike = new List<AppHomeViewModel>();
        if (model.PageNumber == 1)
        {
          moreLike = (_contentFileService.GetTopByTypeAsync("video", 1, 6, "", "like").Result).ToList()
              .Select(p => new AppHomeViewModel
              {
                Id = p.Id,
                Title = p.Title,
                VideoArtist = p.Content.Category.CategoryTags != null
                  ? (string.Join(",",
                    p.Content.Category.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag)
                      .Select(a => a.Title)))
                  : "",
                Thumbnail = string.IsNullOrEmpty(p.Thumbnail)
                  ? ""
                  : "http://mousigha.com/content/files/album/" + p.Thumbnail,
                Pic = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
                Video = "http://mousigha.com/content/files/album/" + p.FileName,
                Type = "last",
                Kind = "video",
                TypeName = "آخرین ویدئوها",
                LikeCount = p.LikeCount,
                VisitCount = p.VisitCount,
                ShareLink = "http://mousigha.com/video/detail/" + p.Id

              }).ToList();
        }
        var videos = (_contentFileService.GetTopByTypeAsync("video", model.PageNumber, model.PageSize, "", "").Result).ToList()
            .Select(p => new AppHomeViewModel
            {
              Id = p.Id,
              Title = p.Title,
              VideoArtist = p.Content.Category.CategoryTags != null ? (string.Join(",", p.Content.Category.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title))) : "",
              Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
              Pic = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
              Video = "http://mousigha.com/content/files/album/" + p.FileName,
              Type = "last",
              Kind = "video",
              TypeName = "آخرین ویدئوها",
              LikeCount = p.LikeCount,
              VisitCount = p.VisitCount,
              ShareLink = "http://mousigha.com/video/detail/" + p.Id
            });
        return Ok(new
        {
          videos,
          moreLike
        });
      }

      if (model.Kind == "tag")
      {
        // if (model.OrderBy == "last")
        {
          var tag = _tagService.GetPagedTagsAsync(1, 10, SortOrder.Descending, "tag", null, "last",true, null).Result.Tags.ToList();

          var tags = tag.Select(p => new AppHomeViewModel
          {
            Id = p.Id,
            Title = "#" + p.Title.Replace(" ", "_"),
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
            Thumbnail = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,
            Type = "last",
            Kind = "tag",
            TypeName = "تگ ها",
            LikeCount = p.LikeCount,
            ShareLink = "http://mousigha.com/tag/detail/" + p.Id



          });

          return Ok(tags);
        }
      }


      if (model.Kind == "style")
      {
        //   if (model.OrderBy == "last")
        {
          var tag = _tagService.GetPagedTagsAsync(model.PageNumber, model.PageSize, SortOrder.Descending, model.Kind, null, "", true, null).Result.Tags.ToList();

          var tags = tag.Select(p => new AppHomeViewModel
          {
            Id = p.Id,
            Title = p.Title,
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
            Thumbnail = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,
            Type = "last",
            Kind = "style",
            TypeName = "سبک ها",
            LikeCount = p.LikeCount,
            VisitCount = p.VisitCount,
            ShareLink = "http://mousigha.com/style/detail/" + p.Id


          });

          return Ok(tags);
        }

      }

      if (model.Kind == "music")
      {
        // if (model.OrderBy == "last")
        {
          var tag = _tagService.GetPagedTagsAsync(model.PageNumber, model.PageSize, SortOrder.Descending, "music", null, "last", true, null).Result.Tags.ToList();

          var tags = tag.Select(p => new AppHomeViewModel
          {
            Id = p.Id,
            Title = p.Title,
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
            Thumbnail = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,
            Type = "last",
            Kind = "music",
            TypeName = "ساز ها",
            LikeCount = p.LikeCount,
            VisitCount = p.VisitCount,
            ShareLink = "http://mousigha.com/music/detail/" + p.Id
          });
          return Ok(tags);
        }
      }




      if (model.Kind == "artist")
      {
        if (model.OrderBy == "last")
        {
          var tag = _tagService.GetPagedTagsAsync(model.PageNumber, model.PageSize, SortOrder.Descending, model.Kind, null, "last", true, null).Result.Tags.ToList();

          var tags = tag.Select(p => new AppHomeViewModel
          {
            Id = p.Id,
            Title = p.Title,
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
            Thumbnail = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,
            Type = "last",
            Kind = "artist",
            TypeName = "هنرمندان",
            LikeCount = p.LikeCount,
            VisitCount = p.VisitCount,

            Rate = 0,
            ShareLink = "http://mousigha.com/artist/detail/" + p.Id


          });

          return Ok(tags);
        }
      }


      if (model.Kind == "all")
      {

        var latest = _categoryService.GetTopByType("1", model.PageNumber, model.PageSize, "", true).Select(
          p => new AppHomeViewModel
          {
            Id = p.Id,
            Title = p.Title,
              //  Artist = p.CategoryTags != null ? (string.Join(",", p.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title))) : null,
              Artist = p.CategoryTags?
                        .Where(t => t.Type == "artist").Select(art => new ArtistViewModel
                        {
                            ArtistId = art.Tag.Id,
                            ArtistName = art.Tag.Title
                        }).ToList(),
              Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/album/" + p.Pic,
            Type = "album-last",
            Kind = "album",
            OrderBy = "last",
            TypeName = "آخرین ها",
            LikeCount = p.LikeCount,
            VisitCount = p.VisitCount,
            Rate = 5,
            ShareLink = "http://mousigha.com/album/detail/" + p.Id


          });

        var morLiked = _categoryService.GetTopByLike("1", model.PageNumber, model.PageSize, true).Select(
          p => new AppHomeViewModel
          {
            Id = p.Id,
            Title = p.Title,
              //  Artist = p.CategoryTags != null ? (string.Join(",", p.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title))) : null,
              Artist = p.CategoryTags?
                        .Where(t => t.Type == "artist").Select(art => new ArtistViewModel
                        {
                            ArtistId = art.Tag.Id,
                            ArtistName = art.Tag.Title
                        }).ToList(),
              Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/album/" + p.Pic,

            Type = "album-like",
            Kind = "album",
            OrderBy = "like",
            TypeName = "محبوب ترین ها",
            LikeCount = p.LikeCount,
            VisitCount = p.VisitCount,

            Rate = 5,
            ShareLink = "http://mousigha.com/album/detail/" + p.Id


          });


        var style = _tagService.GetPagedTagsAsync(1, 10, SortOrder.Descending, "style", null, "", true, null).Result.Tags.ToList();

        var styles = style.Select(p => new AppHomeViewModel
        {
          Id = p.Id,
          Title = p.Title,
          //   Artist = (string.Join(",", p.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title))),
          Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
          Thumbnail = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,
          Video = null,
          Type = "style-last",
          Kind = "style",
          OrderBy = "last",
          TypeName = "سبک ها",
          LikeCount = p.LikeCount,
          VisitCount = p.VisitCount,

          Rate = 0,
          ShareLink = "http://mousigha.com/style/detail/" + p.Id


        });

        var tag = _tagService.GetPagedTagsAsync(1, 10, SortOrder.Descending, "tag", null, "", true, null).Result.Tags.ToList();

        var tags = tag.Select(p => new AppHomeViewModel
        {
          Id = p.Id,
          Title = "#" + p.Title.Replace(" ", "_"),
          Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Pic,
          Thumbnail = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/tag/" + p.Thumbnail,
          Type = "tag-last",
          Kind = "tag",
          OrderBy = "last",
          TypeName = "تگ ها",
          LikeCount = p.LikeCount,
          VisitCount = p.VisitCount,
          Rate = 0,
          ShareLink = "http://mousigha.com/tag/detail/" + p.Id


        });

        var videos = (_contentFileService.GetTopByTypeAsync("video", model.PageNumber, model.PageSize, "", "").Result).ToList()
          .Select(p => new AppHomeViewModel
          {
            Id = p.Id,
            Title = p.Title,
              VideoArtist = p.Content.Category.CategoryTags != null ? (string.Join(",", p.Content.Category.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title))) : null,
              //VideoArtist = p.CategoryTags?
              //          .Where(t => t.Type == "artist").Select(art => new ArtistViewModel
              //          {
              //              ArtistId = art.Tag.Id,
              //              ArtistName = art.Tag.Title
              //          }).ToList(),
              Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
            Pic = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
            Video = string.IsNullOrEmpty(p.FileName) ? "" : "http://mousigha.com/content/files/album/" + p.FileName,
            Type = "video-last",
            Kind = "video",
            OrderBy = "last",
            TypeName = "آخرین ویدئوها",
            LikeCount = p.LikeCount,
            VisitCount = p.VisitCount,
            Rate = 0,
            ShareLink = "https://www.mousigha.com/album/detail/" + p.Id
          });


        var sliders = (_sliderService.GetAll(1, "enable", model.PageNumber, model.PageSize)).ToList()
            .Select(p => new AppHomeViewModel
            {

              Title = p.Title,
              Pic = string.IsNullOrEmpty(p.Pic) ? "" : "https://www.mousigha.com/content/files/slider/" + p.Pic,
              Video = "",
              //Type = p.Link.StartsWith("https://www.mousigha.com/album")==true?"album":"100",
              Kind = "slider",
              OrderBy = "last",
              TypeName = "اسلایدر",
              Type = SetType(p.Link),
              Link = p.Link,
              Rate = 0,
              SubTitle = p.SubTitle,
          
              Id = SetType(p.Link) == "album" ? Convert.ToInt32(Regex.Match(p.Link, @"\d+$").Value) : p.Id
            });


        var merged = sliders.Union(styles).ToList();
        merged = merged.Union(latest.ToList()).ToList();
        merged = merged.Union(tags.ToList()).ToList();
        merged = merged.Union(morLiked.ToList()).ToList();
        merged = merged.Union(videos.ToList()).ToList();

        //merged = merged.Union(sliders.ToList()).ToList();
        //var merged = morLiked.Union(latest).ToList();
        //merged = merged.Union(styles.ToList()).ToList();
        //merged = merged.Union(videos.ToList()).ToList();
        //merged = merged.Union(tags.ToList()).ToList();
        //merged = merged.Union(sliders.ToList()).ToList();

        return Ok(merged);
      }


      return Ok();


    }

    private string SetType(string link)
    {
      string type = "";
      if (!string.IsNullOrEmpty(link) && (link.ToLower().StartsWith("https://www.mousigha.com/album") || link.ToLower().StartsWith("https://mousigha.com/album")))
      {
        type = "album";

      }
      else if (!string.IsNullOrEmpty(link)
               && (link.ToLower().StartsWith("https://www.mousigha.com/video") || link.ToLower().StartsWith("https://mousigha.com/video")))
      {
        type = "video";

      }
      else
      {
        type = "link";
      }
      return type;
    }

    [HttpPost]
    public ActionResult GetItemList([FromBody] PageLimitViewModel model)
    {
      if (model.Kind == "album")
      {
        var p = _categoryService.FindByIdAsync(model.Id, model.FirstVisit).Result;

        var album = new
        {
          p.Id,
          p.Title,

          Artist = p.CategoryTags != null ? (string.Join(",", p.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title))) : null,
          p.ContentText,
          p.Description,
          p.VisitCount,
          Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
          Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/album/" + p.Pic,
          p.LikeCount,

          ShareLink = "http://mousigha.com/album/detail/" + p.Id,


          Musics = p.Contents.Select(a => new
          {
            a.Id,
            Mp3128 = string.IsNullOrEmpty(a.Mp3128) ? "" : "http://mousigha.com/content/files/album/" + a.Mp3128,
            Mp364 = string.IsNullOrEmpty(a.Mp364) ? "" : "http://mousigha.com/content/files/album/" + a.Mp364,
            Mp3320 = string.IsNullOrEmpty(a.Mp3320) ? "" : "http://mousigha.com/content/files/album/" + a.Mp3320,

            a.ContentText,
            Title = Regex.Replace(a.Title, @"^[\d-]*\s*", "", RegexOptions.Multiline),

            Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
            ShareLink =  "http://mousigha.com/content/files/album/" + a.Mp364,

            //   a.Thumbnail,
            a.VisitCount,
            a.LikeCount


          })
        };
        return Ok(album);

      }

      if (model.Kind == "video")
      {
        var p = _contentFileService.FindByIdAsync(model.Id ,model.FirstVisit).Result;

        var video = new
        {
          p.Id,
          p.Title,
          ArtistId = p.Content?.Category?.CategoryTags?.FirstOrDefault(t => t.Type == "artist")?.TagId,
          Artist = p.Content.Category.CategoryTags != null ? (string.Join(",", p.Content.Category.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title))) : null,
          p.ContentText,
         Description= p.ContentText,
          Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
          Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/album/" + p.Pic,
          p.LikeCount,
          Video = string.IsNullOrEmpty(p.FileName) ? "" : "http://mousigha.com/content/files/album/" + p.FileName,
          p.VisitCount,
          ShareLink = "http://mousigha.com/video/detail/" + p.Id

        };

        return Ok(video);
      }

      if (model.Kind == "tag" || model.Kind == "style" || model.Kind == "music")
      {
        var tags = _categoryService.GetTopByTypeAsync("1", model.PageNumber, model.PageSize, model.Id, true).Result.Select(
          p => new AppHomeViewModel
          {
            Id = p.Id,
            Title = p.Title,
              //  Artist = p.CategoryTags != null ? (string.Join(",", p.CategoryTags.Where(t => t.Type == "artist").Select(x => x.Tag).Select(a => a.Title))) : null,
              Artist = p.CategoryTags?
                        .Where(t => t.Type == "artist").Select(art => new ArtistViewModel
                        {
                            ArtistId = art.Tag.Id,
                            ArtistName = art.Tag.Title
                        }).ToList(),

              Thumbnail = string.IsNullOrEmpty(p.Thumbnail) ? "" : "http://mousigha.com/content/files/album/" + p.Thumbnail,
            Pic = string.IsNullOrEmpty(p.Pic) ? "" : "http://mousigha.com/content/files/album/" + p.Pic,
            //Type = "last",
            //Kind = "album",
            //TypeName = "آخرین ها",
            LikeCount = p.LikeCount,
            VisitCount = p.VisitCount,
            ShareLink = "http://mousigha.com/style/detail/" + p.Id

          });
        return Ok(tags);
      }
      return Ok();
    }


    [HttpPost]
    public ActionResult LikeItem([FromBody] PageLimitViewModel model)
    {
      if (model.Kind == "album")
      {

        var p = _categoryService.FindByIdAsync(model.Id, false).Result;

        if (model.Action == "like")
        {
          p.LikeCount = p.LikeCount + 1;
        }
        else if (model.Action == "dislike")
        {
          p.LikeCount = p.LikeCount - 1;

        }

        _categoryService.UpdateCategory(p, null, null, null, null);

        return Ok(p.LikeCount);

      }

      if (model.Kind == "music")
      {
        var p = _contentService.FindByIdAsync(model.Id).Result;

        if (model.Action == "like")
        {
          p.LikeCount = p.LikeCount + 1;

        }
        else if (model.Action == "dislike")
        {
          p.LikeCount = p.LikeCount - 1;

        }

        _contentService.UpdateContent(p, null, null, null, null, null, null, null, null, null);

        return Ok(p.LikeCount);
      }


      if (model.Kind == "artist")
      {
        var p = _tagService.FindByIdAsync(model.Id).Result;

        if (model.Action == "like")
        {
          p.LikeCount = p.LikeCount + 1;

        }
        else if (model.Action == "dislike")
        {
          p.LikeCount = p.LikeCount - 1;

        }

        _tagService.UpdateTag(p, null);

        return Ok(p.LikeCount);
      }

      if (model.Kind == "video")
      {
        var p = _contentFileService.FindByIdAsync(model.Id).Result;

        if (model.Action == "like")
        {
          p.LikeCount = p.LikeCount + 1;

        }
        else if (model.Action == "dislike")
        {
          p.LikeCount = p.LikeCount - 1;
        }
        _contentFileService.UpdateContentFile(p, null, null,null,null);

        return Ok(p.LikeCount);
      }


      return Ok();
    }


    [HttpPost]
    public ActionResult GetSettingAndAbout()
    {
      var p = _appSettingService.FindByIdAsync(1).Result;
      var setting = new
      {
        p.Id,
        p.AboutUs,
        p.CafebazarLink,
        p.ChangeLog,
        p.Instagram,
        p.Site,
        p.Soundcloud,
        p.Telegram
      };
      return Ok(setting);
    }


    [HttpPost]
    public async Task<IActionResult> UpdateFirebaseToken([FromBody] LoginViewModel model)
    {

      var device = await _deviceService.FindByDeviceAsync(model.DeviceId);
      device.FirebaseToken = model.FirebaseToken;
      _deviceService.UpdateDevice(device);
      return Ok();
    }




    //[AllowAnonymous]
    //[HttpPost]
    //public async Task<IActionResult> GenerateToken([FromBody] LoginViewModel model)
    //{
    //  // if (ModelState.IsValid)
    //  {
    //   // var user = await _userManager.FindByNameAsync(model.Username);

    //    // if (user != null)
    //    {
    //      //var result = await _signInManager.PasswordSignInAsync(
    //      //  model.Username,
    //      //  model.Password,
    //      //  model.RememberMe,
    //      //  lockoutOnFailure: true);
    //      // if (result.Succeeded)
    //      {

    //        var claims = new[]
    //        {
    //          new Claim(JwtRegisteredClaimNames.Sub, "ali"),
    //          new Claim(JwtRegisteredClaimNames.Email, "ali2"),
    //          new Claim(JwtRegisteredClaimNames.NameId, "ali3"),
    //          new Claim("deviceId", "22"),
    //          new Claim(ClaimTypes.Email, "email"),
    //          new Claim(ClaimTypes.Name, "namesas"),
    //          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //          new Claim(JwtRegisteredClaimNames.Iat,  DateTime.UtcNow.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
    //        };

    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
    //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //        var token = new JwtSecurityToken(_config["Tokens:Issuer"],
    //          _config["Tokens:Issuer"],
    //          claims,
    //       //expires: DateTime.Now.AddMinutes(1),
    //       expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
    //          signingCredentials: creds


    //          );

    //        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    //      }
    //    }
    //  }

    //  // return BadRequest("Could not create token");
    //}




  }
}
