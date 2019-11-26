using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts.Content;
using MusicProject.Services.Identity;
using MusicProject.ViewModels.Content;
using DNTBreadCrumb.Core;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MusicProject.Areas.Admin.Controllers
{

  [Area(AreaConstants.AdminArea)]
 [Authorize(Policy = ConstantPolicies.DynamicPermission)]
  [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]

  [DisplayName("کنترلر مدیریت پادکست ها")]

  public class PodcastController : Controller
  {
   

    private readonly IPodcastService _podcastService;

    private readonly IUnitOfWork _uow;
    private readonly ITagService _tagService;

    private const int DefaultPageSize = 20;

    public PodcastController(
      IPodcastService podcastService,
      ITagService tagService,
      IUnitOfWork uow
      )
    {
      _podcastService = podcastService;
      _uow = uow;
      _tagService = tagService;
    }
    
    // GET: Content/Create
    public ActionResult Create(int type)
    {

      if (type == 1)
      {
        ViewBag.BreadCrump = "ثبت پادکست  ";
      }

      ViewBag.CurrentDate = DateTime.Now.ToShortPersianDateString();

      var c = new PodcastViewModel();


      return View(c);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(PodcastViewModel model, int type)
    {

      if (ModelState.IsValid)
      {
        var user = new Podcast
        {
          Title = model.Title.Trim(),
          TypeId = model.TypeId,
          IsPublish = model.IsPublic,
          ContentText = model.ContentText != null ? model.ContentText.Replace("../../content/files/editor/", "/content/files/editor/")
            .Replace("../content/files/editor/", "/content/files/editor/") : "",
      
          Description = model.Description
        };

        if (model.RegisterTime.ToLower().Contains("pm"))
        {
          var m = model.RegisterTime.Split(":");
          model.RegisterTime = (Convert.ToInt32(m[0]) == 12 ? 0 : Convert.ToInt32(m[0])) + 12 + ":" + m[1].ToLower().Replace("pm", "").Trim();

        }

        user.PublishDateTime = (model.RegisterDate + " , " + model.RegisterTime.ToLower().Replace("am", "").Trim()).ToGregorianDateTime();

        var c = new List<PodcastTag>();


        if (model.Tags != null)
        {
          var styles = model.Tags.Split(",");
         
          foreach (var t in styles)
          {
            c.Add(new PodcastTag
            {
              TagId = Convert.ToInt32(t),
              Type = "tag",
            });
          }
        }

        if (model.Category != null)
        {
          var artist = model.Category.Split(",");
          //   var c = new List<PodcastTag>();
          foreach (var t in artist)
          {
            c.Add(new PodcastTag
            {
              TagId = Convert.ToInt32(t),
              Type = "cat",
            });
          }
        }

        if (model.Auther != null)
        {
          var music = model.Auther.Split(",");
          //   var c = new List<PodcastTag>();
          foreach (var t in music)
          {
            c.Add(new PodcastTag
            {
              TagId = Convert.ToInt32(t),
              Type = "auther",
            });
          }
        }

        if (model.Speaker != null)
        {
          var music = model.Speaker.Split(",");
          //   var c = new List<PodcastTag>();
          foreach (var t in music)
          {
            c.Add(new PodcastTag
            {
              TagId = Convert.ToInt32(t),
              Type = "speaker",
            });
          }
        }

        if (model.Editor != null)
        {
          var music = model.Editor.Split(",");
          //   var c = new List<PodcastTag>();
          foreach (var t in music)
          {
            c.Add(new PodcastTag
            {
              TagId = Convert.ToInt32(t),
              Type = "editor",
            });
          }
        }

        if (c.Count > 0)
        {
          user.PodcastTags = c;
        }

        _podcastService.AddNewPodcast(user, model.Photo, model.Photo);
        _uow.SaveChanges();

        return RedirectToAction(nameof(Index), new { type = user.TypeId });}

      return View(model);

    }


    public ActionResult Edit(int type, int id, int tagId, int pageNumber)
    {


      var c = _podcastService.FindByIdAsync(id).Result;
      if (c.TypeId == 1)
      {
        ViewBag.BreadCrump = "ویرایش آلبوم";
      }
      var m = new PodcastViewModel
      {
        Title = c.Title,
        TypeId = c.TypeId,
        Thumbnail = c.Thumbnail,
        Pic = c.Pic,
        Id = c.Id,
        IsPublic = c.IsPublish,
        Description = c.Description,

        Tags = string.Join(",", c.PodcastTags.Where(d => d.Type == "tag").Select(d => d.TagId)),

        Category = string.Join(",", c.PodcastTags.Where(d => d.Type == "podcastcat").Select(d => d.TagId)),

        Auther = string.Join(",", c.PodcastTags.Where(d => d.Type == "auther").Select(d => d.TagId)),

        Editor = string.Join(",", c.PodcastTags.Where(d => d.Type == "editor").Select(d => d.TagId)),

        Speaker = string.Join(",", c.PodcastTags.Where(d => d.Type == "speaker").Select(d => d.TagId)),
        
        RegisterDate = c.PublishDateTime.ToShortPersianDateString(),
        PodcastTags = c.PodcastTags,
        RegisterTime = c.PublishDateTime != null ? c.PublishDateTime.Value.ToString("h:mm:tt") : "",
        ContentText = c.ContentText != null ? c.ContentText.Replace("../../content/files/editor/", "/content/files/editor/")
          .Replace("../content/files/editor/", "/content/files/editor/")
          .Replace("..//content/files/editor/", "/content/files/editor/") : ""
      };

      return View(m);
    }

    // POST: Content/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PodcastViewModel model, int id,string pageNumber,string tagId)
    {
      try
      {

        var content = await _podcastService.FindByIdAsync(id).ConfigureAwait(false);

        content.Title = model.Title.Trim();
        content.Thumbnail = model.Thumbnail;
        content.ContentText = model.ContentText != null ? model.ContentText
          .Replace("../../content/files/editor/", "/content/files/editor/")
          .Replace("../content/files/editor/", "/content/files/editor/")
          .Replace("..//content/files/editor/", "/content/files/editor/") : "";
        content.Pic = model.Pic;
    
        content.Description = model.Description;

        content.IsPublish = model.IsPublic;

        if (model.RegisterTime != null && model.RegisterTime.ToLower().Contains("pm"))
        {
          var m = model.RegisterTime.Split(":");
          model.RegisterTime = (Convert.ToInt32(m[0]) == 12 ? 0 : Convert.ToInt32(m[0])) + 12 + ":" + m[1].ToLower().Replace("pm", "").Trim();

        }

        if (model.RegisterTime != null)
        {
          content.PublishDateTime = (model.RegisterDate + " , " + model.RegisterTime.ToLower().Replace("am", "").Trim()).ToGregorianDateTime();
        }

        var c = new List<PodcastTag>();
   

        if (model.Tags != null)
        {
          var tag = model.Tags.Split(",").Where(s => !string.IsNullOrWhiteSpace(s));

          foreach (var t in tag)
          {
            c.Add(new PodcastTag
            {
              TagId = Convert.ToInt32(t),
              Type = "tag",
            });
          }

        }
        if (model.Category != null)
        {
          var podcastcat = model.Category.Split(",").Where(s => !string.IsNullOrWhiteSpace(s));
          //   var c = new List<PodcastTag>();
          foreach (var t in podcastcat)
          {
            c.Add(new PodcastTag
            {
              TagId = Convert.ToInt32(t),
              Type = "podcastcat",
            });
          }
        }

        if (model.Auther != null)
        {
          var auther = model.Auther.Split(",").Where(s => !string.IsNullOrWhiteSpace(s));

          foreach (var t in auther)
          {
            c.Add(new PodcastTag
            {
              TagId = Convert.ToInt32(t),
              Type = "auther",
            });
          }
        }

        if (model.Speaker != null)
        {
          var speaker = model.Speaker.Split(",").Where(s => !string.IsNullOrWhiteSpace(s));
          foreach (var t in speaker)
          {
            c.Add(new PodcastTag
            {
              TagId = Convert.ToInt32(t),
              Type = "speaker",
            });
          }
        }

        if (model.Editor != null)
        {
          var editor = model.Editor.Split(",").Where(s => !string.IsNullOrWhiteSpace(s));

          foreach (var t in editor)
          {
            c.Add(new PodcastTag
            {
              TagId = Convert.ToInt32(t),
              Type = "editor",
            });
          }
        }

        content.PodcastTags.Clear();
        content.PodcastTags = c;

        _podcastService.UpdatePodcast(content, model.Photo, model.Video);

        return RedirectToAction(nameof(Index), new { type = content.TypeId, tagId , pageNumber  });

      }
      catch (Exception e)
      {
        return View();
      }
    }
    

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id, string type, int tagId, int pageNumber)
    {
      _podcastService.Delete(id);

      return RedirectToAction("Index", new { type = type, tagId, pageNumber = pageNumber == 0 ? 1 : pageNumber });
    }

    [BreadCrumb(Title = "ایندکس", Order = 1)]
    public async Task<IActionResult> Index(
      string type = "",
      int pageNumber = 1,
      int pageSize = -1,
      string sort = "desc",
      string title = "",
      int tagId = 0,
      string isPublic="true"
      )
    {
      if (pageNumber <= 0)
        pageNumber = 1;

      if (type == "1")
      {
        ViewBag.BreadCrump = "لیست پادکست ها";
      }
      var itemsPerPage = 10;
      if (pageSize > 0)
      {
        itemsPerPage = pageSize;
      }

      bool? ispub = null;
      if (isPublic!="-1")
        ispub = Convert.ToBoolean(isPublic) ;
      
      var model = await _podcastService.GetPagedPodcastAsync(
        pageNumber, itemsPerPage, sort == "desc" ? SortOrder.Descending : SortOrder.Ascending, type, ispub, title, tagId).ConfigureAwait(false);
      model.Type = type;
      model.Paging.CurrentPage = pageNumber;
      model.Title = title;
      model.Paging.ItemsPerPage = itemsPerPage;
      model.TagId = tagId.ToString();
      model.IsPublic = isPublic;
      return View(model);
    }
  }
}
