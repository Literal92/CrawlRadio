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
using Microsoft.IdentityModel.Tokens;
using MimeKit.Text;
using Newtonsoft.Json;

namespace MusicProject.Areas.Admin.Controllers
{

  [Area(AreaConstants.AdminArea)]
  [Authorize(Policy = ConstantPolicies.DynamicPermission)]
  [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]

  [DisplayName("کنترلر مدیریت نوتیفیکیشن ها")]

  public class NotificationController : Controller
  {
    private const string RoleNotFound = " درخواستی یافت نشد.";

    private readonly ICategoryService _categoryService;

    private readonly IUnitOfWork _uow;
    private readonly ITagService _tagService;



    private const int DefaultPageSize = 20;

    public NotificationController(
      ICategoryService categoryService,
      ITagService tagService,
      IUnitOfWork uow

      )
    {
      _categoryService = categoryService;
      _uow = uow;
      _tagService = tagService;


    }

    //public ActionResult resize()
    //{
    //  var cats = _categoryService.GetAllCategories(1).Where(p=>p.Description.ToLower()!= "discography");

    //  foreach (var category in cats)
    //  {
    //    _categoryService.ResizeCategory(category);
    //  }

    //  return View();
    //}
    /*
    public object ret(ICollection<Category> a)
    {
      var b = a.Select(p => new
      {
        id = p.Id,
        text = p.Title,
        children = p.InverseParent != null ?
            ret(p.InverseParent)
            : null
      });


      return b;
    }
    // GET: Content/Create
    public ActionResult Create(int type)
    {

      if (type == 1)
      {
        ViewBag.BreadCrump = "ثبت آلبوم ";
      }

      ViewBag.CurrentDate = DateTime.Now.ToShortPersianDateString();


      var c = new CategoryViewModel();


      return View(c);
    }


    // POST: Content/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(CategoryViewModel model, int type)
    {

      if (ModelState.IsValid)
      {
        var user = new Category
        {
          Title = model.Title.Trim(),
          TypeId = model.TypeId,
          IsPublish = model.IsPublic,
          IsSelected = model.IsSelected,
          ContentText = model.ContentText != null ? model.ContentText.Replace("../../content/files/editor/", "/content/files/editor/")
            .Replace("../content/files/editor/", "/content/files/editor/") : "",
          ParentId = model.ParentId,
          Description = model.Description,
          SeoDescription = model.SeoDescription
        };

        if (model.RegisterTime.ToLower().Contains("pm"))
        {
          var m = model.RegisterTime.Split(":");
          model.RegisterTime = (Convert.ToInt32(m[0]) == 12 ? 0 : Convert.ToInt32(m[0])) + 12 + ":" + m[1].ToLower().Replace("pm", "").Trim();

        }

        user.PublishDateTime = (model.RegisterDate + " , " + model.RegisterTime.ToLower().Replace("am", "").Trim()).ToGregorianDateTime();


        var c = new List<CategoryTag>();

        if (model.Tags != null)
        {
          var tags = model.Tags.Split(",");
          foreach (var t in tags)
          {
            c.Add(new CategoryTag
            {
              TagId = Convert.ToInt32(t),
              Type = "tag",
            });
          }
        }

        if (model.Styles != null)
        {
          var styles = model.Styles.Split(",");
          // var c = new List<CategoryTag>();
          foreach (var t in styles)
          {
            c.Add(new CategoryTag
            {
              TagId = Convert.ToInt32(t),
              Type = "style",
            });
          }

        }
        if (model.Artists != null)
        {
          var artist = model.Artists.Split(",");
          //   var c = new List<CategoryTag>();
          foreach (var t in artist)
          {
            c.Add(new CategoryTag
            {
              TagId = Convert.ToInt32(t),
              Type = "artist",
            });
          }
        }

        if (model.Musics != null)
        {
          var music = model.Musics.Split(",");
          //   var c = new List<CategoryTag>();
          foreach (var t in music)
          {
            c.Add(new CategoryTag
            {
              TagId = Convert.ToInt32(t),
              Type = "music",
            });
          }
        }

        if (c.Count > 0)
        {
          user.CategoryTags = c;
        }

        _categoryService.AddNewCategory(user, model.Photo,model.ZipMp364,model.ZipMp3128,model.ZipMp3320);
        _uow.SaveChanges();

        return RedirectToAction(nameof(Index), new { type = user.TypeId });
      }

      return View(model);


    }


    public ActionResult Edit(int type, int id, int tagId, int pageNumber)
    {


      var c = _categoryService.FindByIdAsync(id).Result;
      if (c.TypeId == 1)
      {
        ViewBag.BreadCrump = "ویرایش آلبوم";
      }
      var m = new CategoryViewModel
      {
        Title = c.Title,
        TypeId = c.TypeId,
        Thumbnail = c.Thumbnail,
        IsSelected =c.IsSelected != null && (bool) c.IsSelected,
        Pic = c.Pic,
        Id = c.Id,
        IsPublic = c.IsPublish,
        ParentId = c.ParentId,
        Description = c.Description,
        SeoDescription = c.SeoDescription,

        Tags = string.Join(",", c.CategoryTags.Where(d => d.Type == "tag").Select(d => d.TagId)),

        Styles = string.Join(",", c.CategoryTags.Where(d => d.Type == "style").Select(d => d.TagId)),

        Artists = string.Join(",", c.CategoryTags.Where(d => d.Type == "artist").Select(d => d.TagId)),

        Musics = string.Join(",", c.CategoryTags.Where(d => d.Type == "music").Select(d => d.TagId)),





        RegisterDate = c.PublishDateTime.ToShortPersianDateString(),
        CategoryTags = c.CategoryTags,
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
    public async Task<IActionResult> Edit(CategoryViewModel model, int id,string pageNumber,string tagId)
    {
      try
      {

        var content = await _categoryService.FindByIdAsync(id).ConfigureAwait(false);

        content.Title = model.Title.Trim();
        content.Thumbnail = model.Thumbnail;
        content.ContentText = model.ContentText != null ? model.ContentText
          .Replace("../../content/files/editor/", "/content/files/editor/")
          .Replace("../content/files/editor/", "/content/files/editor/")
          .Replace("..//content/files/editor/", "/content/files/editor/") : "";
        content.Pic = model.Pic;
        content.ParentId = model.ParentId;
        content.Description = model.Description;
        content.IsSelected = model.IsSelected;
        content.IsPublish = model.IsPublic;
        content.SeoDescription = model.SeoDescription;

        if (model.RegisterTime != null && model.RegisterTime.ToLower().Contains("pm"))
        {
          var m = model.RegisterTime.Split(":");
          model.RegisterTime = (Convert.ToInt32(m[0]) == 12 ? 0 : Convert.ToInt32(m[0])) + 12 + ":" + m[1].ToLower().Replace("pm", "").Trim();

        }

        if (model.RegisterTime != null)
        {
          content.PublishDateTime = (model.RegisterDate + " , " + model.RegisterTime.ToLower().Replace("am", "").Trim()).ToGregorianDateTime();
        }

        var c = new List<CategoryTag>();


        if (model.Tags != null)
        {
          var tags = model.Tags.Split(",").Where(s => !string.IsNullOrWhiteSpace(s));
          foreach (var t in tags)
          {
            c.Add(new CategoryTag
            {
              TagId = Convert.ToInt32(t),
              Type = "tag"
            });
          }
        }


        if (model.Artists != null)
        {
          var tags = model.Artists.Split(",").Where(s => !string.IsNullOrWhiteSpace(s));
          foreach (var t in tags)
          {
            c.Add(new CategoryTag
            {
              TagId = Convert.ToInt32(t),
              Type = "artist"
            });
          }
        }


        if (model.Musics != null)
        {
          var tags = model.Musics.Split(",").Where(s => !string.IsNullOrWhiteSpace(s));
          foreach (var t in tags)
          {
            c.Add(new CategoryTag
            {
              TagId = Convert.ToInt32(t),
              Type = "music"
            });
          }
        }

        if (model.Styles != null)
        {
          var tags = model.Styles.Split(",").Where(s => !string.IsNullOrWhiteSpace(s));
          foreach (var t in tags)
          {
            c.Add(new CategoryTag
            {
              TagId = Convert.ToInt32(t),
              Type = "style"
            });
          }
        }

        content.CategoryTags.Clear();
        content.CategoryTags = c;

        _categoryService.UpdateCategory(content, model.Photo);

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
      _categoryService.Delete(id);

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
        ViewBag.BreadCrump = "لیست آلبوم ها";
      }
      var itemsPerPage = 10;
      if (pageSize > 0)
      {
        itemsPerPage = pageSize;
      }

      bool? ispub = null;
      if (isPublic!="-1")
        ispub = Convert.ToBoolean(isPublic) ;
      
      var model = await _categoryService.GetPagedCategoryAsync(
        pageNumber, itemsPerPage, sort == "desc" ? SortOrder.Descending : SortOrder.Ascending, type, ispub, title, tagId).ConfigureAwait(false);
      model.Type = type;
      model.Paging.CurrentPage = pageNumber;
      model.Title = title;
      model.Paging.ItemsPerPage = itemsPerPage;
      model.TagId = tagId.ToString();
      model.IsPublic = isPublic;
      return View(model);
    }



    public ActionResult GetTags(string term, string type)
    {
      term = term.Split(',').Last().Trim();

      var list = _tagService.GetStartBy(term, 7).Select(p => new
      {
        label = p.Title,
        value = p.Title,
        id = p.Id,

      }).ToList();



      return Json(list);

    }

    // [HttpPost]
    public ActionResult GetStartBy(string searchTerm, int pageSize, int pageNum, string countyId)
    {

      var itemList = _categoryService.GetTopByType("1", pageNum, pageSize, searchTerm,null);

      var result = new
      {
        Total = _categoryService.GetCount("1", searchTerm),
        Results = itemList.Select(p => new Select2Item()
        {
          Id = p.Id,
          Title = p.Title


        })
      };

      return new JsonResult(result);


    }
    public List<Select2Item> GetItems()
    {


      var itemList = new List<Select2Item>();
      for (int i = 0; i < 1000; i++)
      {
        itemList.Add(
          new Select2Item()
          {
            Id = i,
            Title = "First " + i.ToString(),

          }
        );
      }

      return itemList;
    }
    */

  }
}
