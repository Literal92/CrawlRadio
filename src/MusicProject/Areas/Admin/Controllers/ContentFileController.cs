using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts.Content;
using MusicProject.Services.Identity;
using MusicProject.ViewModels.Content;

namespace MusicProject.Areas.Admin.Controllers
{
    [Area(AreaConstants.AdminArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]

    [DisplayName("کنترلر مدیریت ویدیو ها")]
    public class ContentFileController : Controller
    {        
        private readonly IContentService _contentService;
        private readonly IContentFileService _contentFileService;

        public ContentFileController(
          IContentFileService contentFileService,
          IContentService contentService
          )
        {
            _contentFileService = contentFileService;
            _contentService = contentService;
        }

        // GET: Content/Create
        [DisplayName("مشاهده صفحه ثبت ویدیو")]
        public ActionResult Create(int contentId, string type)
        {
            if (contentId != 0)
            {
                ViewBag.ContentId = contentId;
                ViewBag.ContentName = _contentService.FindByIdAsync(contentId).Result.Title;
            }
            return View();
        }

        // POST: Content/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("ثبت ویدیو")]

        public ActionResult Create(ContentFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var contentFile = new ContentFile
                {
                    ContentId = model.ContentId,
                    Description = model.Description,
                    VisitCount = 0,
                    LikeCount = 0,
                    FileSize = 0,
                    Thumbnail = "",
                    Title = model.Title,
                    IsSelected = model.IsSelected,
                    IsPublish = model.IsPublic,
                    ContentText = model.ContentText != null ? model.ContentText.Replace("../../content/files/editor/", "/content/files/editor/")
                    .Replace("../content/files/editor/", "/content/files/editor/") : "",
                    Pic = "",
                    Type = model.Type
                };
                _contentFileService.AddNewContentFile(contentFile, model.Photo, model.Video, model.Video2, model.Video3);

                return RedirectToAction(nameof(Index), new { type = contentFile.Type });
            }
            return View(model);

        }


        [BreadCrumb(Title = "ایندکس", Order = 1)]
        [DisplayName("لیست ویدیو")]
        public async Task<IActionResult> Index(
          string type = "",
          int pageNumber = 1,
          int pageSize = -1,
          string sort = "desc",
          string title = "",
          int contentId = 0
        )
        {
            var itemsPerPage = 10;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = await _contentFileService.GetPagedContentFileAsync(pageNumber, itemsPerPage, sort == "desc" ? SortOrder.Descending : SortOrder.Ascending, type, title, contentId).ConfigureAwait(false);
            model.Type = type;
            model.Paging.CurrentPage = pageNumber;
            model.Title = title;
            model.Paging.ItemsPerPage = itemsPerPage;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف ویدیو")]
        public IActionResult Delete(int id, string type)
        {
            _contentFileService.Delete(id);
            return RedirectToAction("Index", new { type = type });
        }

        [DisplayName("مشاهده صفحه ویدیو")]
        public ActionResult Edit(string type, int id)
        {
            var c = _contentFileService.FindByIdAsync(id).Result;
            var m = new ContentFileViewModel
            {
                Title = c.Title,
                Type = c.Type,
                Thumbnail = c.Thumbnail,
                Pic = c.Pic,
                Id = c.Id,
                Description = c.Description,
                ContentText = c.ContentText != null ?
                c.ContentText.Replace("../../content/files/editor/", "/content/files/editor/")
                .Replace("../content/files/editor/", "/content/files/editor/")
                .Replace("..//content/files/editor/", "/content/files/editor/") : "",
                ContentId = c.ContentId,
                Ext = c.Ext,
                FileName = c.FileName,
                FileName2 = c.FileName2,
                FileName3 = c.FileName3,
                LikeCount = c.LikeCount,
                PhotoFileName = c.Pic,
                VideoFileName = c.FileName,
                VideoFileName2 = c.FileName2,
                VideoFileName3 = c.FileName3,

                VisitCount = c.VisitCount,
                IsSelected = Convert.ToBoolean(c.IsSelected),
                IsPublic = Convert.ToBoolean(c.IsPublish)
            };
            ViewBag.ContentName = c.Content.Title;
            return View(m);
        }

        // POST: Content/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("ویرایش ویدیو")]
        public async Task<IActionResult> Edit(ContentFileViewModel model, int id)
        {
            var content = await _contentFileService.FindByIdAsync(id).ConfigureAwait(false);
            content.Title = model.Title.Trim();
            content.Thumbnail = model.Thumbnail;
            content.ContentText = model.ContentText != null ? model.ContentText
              .Replace("../../content/files/editor/", "/content/files/editor/")
              .Replace("../content/files/editor/", "/content/files/editor/")
              .Replace("..//content/files/editor/", "/content/files/editor/") : "";
            content.Pic = model.Pic;
            content.FileName = model.FileName;
            content.FileName2 = model.FileName2;
            content.FileName3 = model.FileName3;
            content.Type = model.Type;
            content.ContentId = model.ContentId;
            content.Description = model.Description;
            content.Ext = model.Ext;
            content.FileSize = model.FileSize;
            content.FileSize2 = model.FileSize2;
            content.FileSize3 = model.FileSize3;
            content.IsSelected = model.IsSelected;
            content.IsPublish = model.IsPublic;


            _contentFileService.UpdateContentFile(content, model.Photo, model.Video, model.Video2, model.Video3);

            return RedirectToAction(nameof(Index), new { type = content.Type });

        }
    }
}