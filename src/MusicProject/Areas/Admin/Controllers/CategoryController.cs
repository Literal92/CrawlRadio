using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
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
using Newtonsoft.Json;

namespace MusicProject.Areas.Admin.Controllers
{

    [Area(AreaConstants.AdminArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]

    [DisplayName("کنترلر مدیریت آلبوم")]

    public class CategoryController : Controller
    {
        private const string RoleNotFound = "نقش درخواستی یافت نشد.";

        private readonly ICategoryService _categoryService;

        private readonly IUnitOfWork _uow;
        private readonly ITagService _tagService;



        private const int DefaultPageSize = 20;

        public CategoryController(
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

        //public object ret(ICollection<Category> a)
        //{
        //    var b = a.Select(p => new
        //    {
        //        id = p.Id,
        //        text = p.Title,
        //        children = p.InverseParent != null ?
        //          ret(p.InverseParent)
        //          : null
        //    });
        //    return b;
        //}

        [DisplayName("مشاهده صفحه ثبت آلبوم جدید")]

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
        [DisplayName(" ثبت آلبوم جدید")]

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
                    Description = model.Description.Trim(),
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

                var id = _categoryService.AddNewCategory(user, model.Photo, model.ZipMp364, model.ZipMp3128, model.ZipMp3320);
                _uow.SaveChanges();
                if (model.SendNotification)
                {

                    SendNotification(
                      "/topics/all", "آلبوم " + model.Title + " منتشر شد",
                      "آلبوم " + model.Title + " منتشر شد"
                      , id.ToString());
                }

                return RedirectToAction(nameof(Index), new { type = user.TypeId, IsPublic = -1 });
            }

            return View(model);


        }

        private string serverKey = "AAAAX2BnHD4:APA91bG5D-DvWqIeOfFZlKMS6Uur6TzHcazbo2tcGy9dyAeu9LXVTWujepLKgaW3hm3-TL8pgnLNi0q9-x773NZqINdL7xcfklsLpGoaMQLl72KzNY8devpKLvTw8r5x6tBW9z960t9Aqpv6jFM5C76xGlld1thp1g";
        private string senderId = "409639263294";
        private string webAddr = "https://fcm.googleapis.com/fcm/send";
        [DisplayName(" ارسال نوتیف به موبایل")]

        public string SendNotification(string DeviceToken, string title, string msg, string id)
        {
            var result = "-1";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
            httpWebRequest.Method = "POST";
            var payload = new
            {
                to = DeviceToken,
                priority = "high",
                content_available = true,
                data = new
                {
                    body = msg,
                    title = title,
                    id = id,
                    kind = "mousigha"
                },
            };
            //  var serializer = new JavaScriptSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json = JsonConvert.SerializeObject(payload);
                streamWriter.Write(json);
                streamWriter.Flush();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        // POST: Content/Edit/5
        [HttpPost]

        [ValidateAntiForgeryToken]
        [DisplayName("ویرایش آلبوم")]

        public async Task<IActionResult> Edit(CategoryViewModel model, int id, string pageNumber, string tagId)
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
            content.Description = model.Description.Trim();
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

            _categoryService.UpdateCategory(content, model.Photo, model.ZipMp364, model.ZipMp3128, model.ZipMp3320);




            if (model.SendNotification)
            {
                SendNotification(
                  "/topics/all", "آلبوم " + content.Title + " منتشر شد",
                  "آلبوم " + content.Title + " منتشر شد"
                  , content.Id.ToString());

            }

            return RedirectToAction(nameof(Index), new { type = content.TypeId, tagId, pageNumber, IsPublic = -1 });

            //}
            //catch (Exception e)
            //{
            //  return View();
            //}
        }

        [DisplayName("مشاهده صفحه ویرایش آلبوم ")]

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
                IsSelected = c.IsSelected != null && (bool)c.IsSelected,
                Pic = c.Pic,
                Id = c.Id,
                IsPublic = c.IsPublish,
                ParentId = c.ParentId,
                Description = c.Description.Trim(),
                SeoDescription = c.SeoDescription,
                ZipMp364Src = c.ZipMp364,
                ZipMp3128Src = c.ZipMp3128,
                ZipMp3320Src = c.ZipMp3320,
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف آلبوم ")]

        public IActionResult Delete(int id, string type, int tagId, int pageNumber)
        {
            _categoryService.Delete(id);

            return RedirectToAction("Index", new { type = type, tagId, pageNumber = pageNumber == 0 ? 1 : pageNumber });
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        [DisplayName("مشاهده لیست آلبوم ها")]

        public async Task<IActionResult> Index(
          string type = "",
          int pageNumber = 1,
          int pageSize = -1,
          string sort = "desc",
          string title = "",
          int tagId = 0,
          string isPublic = "true"
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
            if (isPublic != "-1")
                ispub = Convert.ToBoolean(isPublic);

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



        //public ActionResult GetTags(string term, string type)
        //{
        //    term = term.Split(',').Last().Trim();

        //    var list = _tagService.GetStartBy(term, 7).Select(p => new
        //    {
        //        label = p.Title,
        //        value = p.Title,
        //        id = p.Id,

        //    }).ToList();



        //    return Json(list);

        //}

        // [HttpPost]
        [DisplayName("مشاهده لیست آلبوم ها در منوهای کشویی ")]
        public ActionResult GetStartBy(string searchTerm, int pageSize, int pageNum, string countyId)
        {

            var itemList = _categoryService.GetTopByType("1", pageNum, pageSize, searchTerm, null);

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
    }
}
