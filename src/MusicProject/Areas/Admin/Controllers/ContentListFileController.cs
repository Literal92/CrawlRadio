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

    [DisplayName("کنترلر مدیریت فایل های لیست پخش")]
    public class ContentListFileController : Controller
    {
        private readonly IContentListService _contentListService;
        private readonly IContentListFileService _contentListFileService;

        public ContentListFileController(
          IContentListFileService contentListFileService,
          IContentListService contentListService
          )
        {
            _contentListFileService = contentListFileService;
            _contentListService = contentListService;
        }
        [DisplayName("مشاهده صفحه اضافه کردن فایل به لیست پخش")]

        public async Task<ActionResult> Create(int id)
        {

            var contentList = await _contentListService.FindByIdAsync(id);
            ViewBag.ContentListName = contentList.Title;
            return View(new ContentListFileViewModel
            {
                ContentListId = id,
                Order = 0
            });
        }

        // POST: Content/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("اضافه کردن فایل به لیست پخش")]

        public ActionResult Create(ContentListFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var contentFile = new ContentListFile
                {
                    VisitCount = 0,
                    LikeCount = 0,
                    FileSize = 0,
                    Title = model.Title,
                    Type = "1",
                    ContentListId = model.ContentListId,
                    Order = model.Order
                };

                _contentListFileService.AddNewContentListFile(contentFile, model.File1, model.File2, model.File3);
                return RedirectToAction(nameof(Index), new { type = contentFile.Type });
            }
            return View(model);
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        [DisplayName("مشاهده لیست فایل های لیست پخش")]

        public async Task<IActionResult> Index(
          string type = "",
          int pageNumber = 1,
          int pageSize = -1,
          string sort = "desc",
          string title = "",
          int contentlistid = 0
        )
        {
            var itemsPerPage = 10;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            var model = await _contentListFileService.GetPagedContentListFileAsync(pageNumber, itemsPerPage, sort == "desc" ? SortOrder.Descending : SortOrder.Ascending, type, title, contentlistid).ConfigureAwait(false);
            model.Type = type;
            model.Paging.CurrentPage = pageNumber;
            model.Title = title;
            model.Paging.ItemsPerPage = itemsPerPage;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف فایل آپلودی از لیست پخش")]

        public IActionResult Delete(int id, string type)
        {
            _contentListFileService.Delete(id);
            return RedirectToAction("Index", new { type = type });
        }

        [DisplayName("مشاهده صفحه ویرایش فایل اپلودی لیست پخش")]

        public ActionResult Edit(string type, int id)
        {
            var c = _contentListFileService.FindByIdAsync(id).Result;
            var m = new ContentListFileViewModel
            {
                Title = c.Title,
                Id = c.Id,
                Mp364 = c.Mp364,
                Mp3128=c.Mp3128,
                Mp3320=c.Mp3320,
                LikeCount = c.LikeCount,
                VisitCount = c.VisitCount,
                Type = c.Type,
                ContentListId = c.ContentListId,
                Order = c.Order
            };
            ViewBag.ContentListName = c.ContentList.Title;
            return View(m);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("ویرایش فایل اپلودی لیست پخش")]

        public async Task<IActionResult> Edit(ContentListFileViewModel model, int id)
        {
            var content = await _contentListFileService.FindByIdAsync(id).ConfigureAwait(false);
            content.Title = model.Title==null ? "":model.Title.Trim();         
            content.Mp364 = model.Mp364;         
            content.Mp3128 = model.Mp3128;         
            content.Mp3320 = model.Mp3320;         
            content.Type = model.Type;
            content.VisitCount = model.VisitCount;
            content.LikeCount = model.LikeCount;
            content.Order = model.Order;
            content.ContentListId = model.ContentListId;
            _contentListFileService.UpdateContentListFile(content, model.File1,model.File2,model.File3);
            return RedirectToAction(nameof(Index), new { type = content.Type });
        }
    }
}