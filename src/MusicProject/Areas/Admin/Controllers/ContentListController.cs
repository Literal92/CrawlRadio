using DNTBreadCrumb.Core;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts.Content;
using MusicProject.Services.Identity;
using MusicProject.ViewModels.Content;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MusicProject.Areas.Admin.Controllers
{

    [Area(AreaConstants.AdminArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]

    [DisplayName("کنترلر مدیریت لیست های پخش")]

    public class ContentListController : Controller
    {
        private const string RoleNotFound = "رکورد درخواستی یافت نشد.";

        private readonly IContentListService _contentListService;

        private readonly IUnitOfWork _uow;




        private const int DefaultPageSize = 20;

        public ContentListController(
          // IContentService contentService,

          IContentListService contentListService,
          ITagService tagService,
          // ICategoryService categoryService,
          IUnitOfWork uow
          )
        {
            _contentListService = contentListService;
            _uow = uow;
            // _categoryService = categoryService;
            //  _contentService = contentService;
        }


        [BreadCrumb(Title = "ایندکس", Order = 1)]
        [DisplayName("مشاهده لیست های پخش")]

        public async Task<IActionResult> Index(

           string type = "",
          int pageNumber = 1,
          int pageSize = -1,
          string sort = "desc",
          string title = "",
          int tagId = 0
          )
        {
            var itemsPerPage = 10;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = await _contentListService.GetPagedContentListAsync(
              pageNumber,
              itemsPerPage,
              sort == "desc" ? SortOrder.Descending : SortOrder.Ascending,
              type, null, title,0,tagId ).ConfigureAwait(false);
            model.Type = type;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Title = title;
            return View(model);
        }

        // GET: Content/Create
        [DisplayName("مشاهده صفحه لیست پخش")]

        public ActionResult Create(string type)
        {
            ViewBag.CurrentDate = DateTime.Now.ToShortPersianDateString();
            return View();
        }


        // POST: Content/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("ثبت لیست پخش")]

        public ActionResult Create(ContentListViewModel model)
        {

            if (ModelState.IsValid)
            {
                var contentList = new ContentList
                {
                    Title = model.Title.Trim(),
                    Description = model.Description ?? "",
                    Type = model.Type,
                    Content = model.Content != null
                    ? model.Content.Replace("../../content/files/editor/", "/content/files/editor/")
                      .Replace("../content/files/editor/", "/content/files/editor/")
                    : "",
                    IsPublish = model.IsPublish
                };

                var contentListTags = new List<ContentListTag>();
                if (model.RegisterTime.ToLower().Contains("pm"))
                {
                    var m = model.RegisterTime.Split(":");
                    model.RegisterTime = (Convert.ToInt32(m[0]) == 12 ? 0 : Convert.ToInt32(m[0])) + 12 + ":" + m[1].ToLower().Replace("pm", "").Trim();
                }

                contentList.PublishDateTime = (model.RegisterDate + " , " + model.RegisterTime.ToLower().Replace("am", "").Trim()).ToGregorianDateTime();
                if (model.Tags != null)
                {
                    var tags = model.Tags.Split(",");
                    foreach (var t in tags)
                    {
                        contentListTags.Add(new ContentListTag
                        {
                            TagId = Convert.ToInt32(t),
                            Type = "cat",
                        });
                    }
                }

                if (model.Artists != null)
                {
                    var tags = model.Artists.Split(",");
                    foreach (var t in tags)
                    {
                        contentListTags.Add(new ContentListTag
                        {
                            TagId = Convert.ToInt32(t),
                            Type = "artist",
                        });
                    }
                }

                if (model.Styles != null)
                {
                    var tags = model.Styles.Split(",");
                    foreach (var t in tags)
                    {
                        contentListTags.Add(new ContentListTag
                        {
                            TagId = Convert.ToInt32(t),
                            Type = "style",
                        });
                    }
                }

                if (model.Musics != null)
                {
                    var tags = model.Musics.Split(",");
                    foreach (var t in tags)
                    {
                        contentListTags.Add(new ContentListTag
                        {
                            TagId = Convert.ToInt32(t),
                            Type = "music",
                        });
                    }
                }

                if (contentListTags.Any())
                {
                    contentList.ContentListTags = contentListTags;
                }
                var id = _contentListService.AddNewContentList(contentList, model.Photo);
                _uow.SaveChanges();

                if (model.SendNotification)
                {
                    SendNotification(
                    "/topics/all", "لیست پخش " + model.Title + " منتشر شد",
                    "لیست پخش " + model.Title + " منتشر شد"
                    , id.ToString());
                }

                return RedirectToAction(nameof(Index), new { type = contentList.Type });
            }
            return View(model);
        }

        private string serverKey = "AAAAX2BnHD4:APA91bG5D-DvWqIeOfFZlKMS6Uur6TzHcazbo2tcGy9dyAeu9LXVTWujepLKgaW3hm3-TL8pgnLNi0q9-x773NZqINdL7xcfklsLpGoaMQLl72KzNY8devpKLvTw8r5x6tBW9z960t9Aqpv6jFM5C76xGlld1thp1g";
        private string senderId = "409639263294";
        private string webAddr = "https://fcm.googleapis.com/fcm/send";
        [HttpPost]
        [DisplayName("ارسال نوتیفیکیشن")]

        public string SendNotification(string DeviceToken, string title, string msg, string id)
        {
            DeviceToken = "/topics/all";
            title = "لیست پخش  منتشر شد";
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

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        [DisplayName("مشاهده صفحه ویرایش لیست پخش")]

        public async Task<IActionResult> Edit(int id, string type)
        {
            var contentList = await _contentListService.FindByIdAsync(id).ConfigureAwait(false);

            var m = new ContentListViewModel
            {
                Title = contentList.Title,

                Id = contentList.Id,
                Pic = contentList.Pic,
                Thumbnail = contentList.Thumbnail,
                Description = contentList.Description,
                IsPublish = contentList.IsPublish,
                RegisterDate = contentList.PublishDateTime.ToShortPersianDateString(),
                RegisterTime = contentList.PublishDateTime != null ? contentList.PublishDateTime.Value.ToString("h:mm:tt") : "",
                Content = contentList.Content != null
                ? contentList.Content.Replace("../../content/files/editor/", "/content/files/editor/")
                  .Replace("../content/files/editor/", "/content/files/editor/")
                  .Replace("..//content/files/editor/", "/content/files/editor/")
                : "",
                Type = contentList.Type,
                Tags = contentList.ContentListTags != null ? string.Join(",", contentList.ContentListTags?.Where(d => d.Type == "cat").Select(d => d.TagId)) : null,
                Styles = contentList.ContentListTags != null ? string.Join(",", contentList.ContentListTags?.Where(d => d.Type == "style").Select(d => d.TagId)) : null,
                Artists = contentList.ContentListTags != null ? string.Join(",", contentList.ContentListTags?.Where(d => d.Type == "artist").Select(d => d.TagId)) : null,
                Musics = contentList.ContentListTags != null ? string.Join(",", contentList.ContentListTags?.Where(d => d.Type == "music").Select(d => d.TagId)) : null,
                ContentListTags = contentList.ContentListTags
            };
            return View(viewName: nameof(Edit), model: m);

        }

        // POST: Content/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("ویرایش لیست پخش")]

        public async Task<IActionResult> Edit(ContentListViewModel model, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var content = await _contentListService.FindByIdAsync(id).ConfigureAwait(false);
                content.Title = model.Title.Trim();
                content.Description = model.Description.Trim();
                content.IsPublish = model.IsPublish;
                content.Thumbnail = model.Thumbnail;
                content.Pic = model.Pic;

                content.Content = model.Content != null ? model.Content
                  .Replace("../../content/files/editor/", "/content/files/editor/")
                  .Replace("../content/files/editor/", "/content/files/editor/")
                  .Replace("..//content/files/editor/", "/content/files/editor/") : "";

                var c = new List<ContentListTag>();

                if (model.Tags != null)
                {
                    var tags = model.Tags.Split(",").Where(s => !string.IsNullOrWhiteSpace(s));
                    foreach (var t in tags)
                    {
                        c.Add(new ContentListTag
                        {
                            TagId = Convert.ToInt32(t),
                            Type = "cat"
                        });
                    }
                }
                if (model.Artists != null)
                {
                    var tags = model.Artists.Split(",").Where(s => !string.IsNullOrWhiteSpace(s));
                    foreach (var t in tags)
                    {
                        c.Add(new ContentListTag
                        {
                            TagId = Convert.ToInt32(t),
                            Type = "artist"
                        });
                    }
                }
                if (model.Styles != null)
                {
                    var tags = model.Styles.Split(",").Where(s => !string.IsNullOrWhiteSpace(s));
                    foreach (var t in tags)
                    {
                        c.Add(new ContentListTag
                        {
                            TagId = Convert.ToInt32(t),
                            Type = "style"
                        });
                    }
                }
                if (model.Musics != null)
                {
                    var tags = model.Musics.Split(",").Where(s => !string.IsNullOrWhiteSpace(s));
                    foreach (var t in tags)
                    {
                        c.Add(new ContentListTag
                        {
                            TagId = Convert.ToInt32(t),
                            Type = "music"
                        });
                    }
                }


                content.ContentListTags.Clear();
                content.ContentListTags = c;
                _contentListService.UpdateContentList(content, model.Photo);

                if (model.SendNotification)
                {
                    SendNotification(
                      "/topics/all", "لیست پخش " + content.Title + " منتشر شد",
                      "لیست پخش " + content.Title + " منتشر شد"
                      , content.Id.ToString());

                }

                return RedirectToAction(nameof(Index), new { type = content.Type });
            }
            catch (Exception e)
            {
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف لیست پخش")]

        public IActionResult Delete(int id, string type)
        {
            _contentListService.Delete(id);
            return RedirectToAction("Index", new { type = type });
        }

        [DisplayName("نمایش لیست پخش در منوهای کشویی")]

        public ActionResult GetStartBy(string searchTerm, int pageSize, int pageNum, string countyId)
        {

            var itemList = _contentListService.GetTopByType("1", pageNum, pageSize, searchTerm);

            var result = new
            {
                Total = _contentListService.GetCount("1", searchTerm),
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
