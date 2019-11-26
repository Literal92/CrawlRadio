using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Services.Content;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;
using MusicProject.Services.Identity;
using MusicProject.ViewModels.Content;

namespace MusicProject.Areas.Admin.Controllers
{

    [Area(AreaConstants.AdminArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]

    [DisplayName("کنترلر مدیریت هنرمند،تگ،سبک و ساز")]

    public class TagController : Controller
    {
        private const string RoleNotFound = "رکورد درخواستی یافت نشد.";


        private readonly ITagService _tagService;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<TagController> _logger;
        private readonly IUploadService _uploadService;

        private const int DefaultPageSize = 7;

        public TagController(
          IContentService contentService,
          ITagService tagService,
          IUploadService uploadService,
          IUnitOfWork uow,
          ILogger<TagController> logger
          )
        {
            _tagService = tagService;
            _uow = uow;
            _logger = logger;
            _logger.CheckArgumentIsNull(nameof(_logger));
            _uploadService = uploadService;
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        [DisplayName("مشاهده لیست")]

        public async Task<IActionResult> Index(
          string type = "",
          int pageNumber = 1,
          int pageSize = -1,
          string sort = "desc",
          string title = "",
          string orderBy = "last",
          bool? isPublish = null,
          bool? isSelected = null
          )
        {

            var itemsPerPage = 10;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = await _tagService.GetPagedTagsAsync(
              pageNumber,
              itemsPerPage,
              sort == "desc" ? SortOrder.Descending : SortOrder.Ascending,
              type, title, orderBy, isPublish, isSelected).ConfigureAwait(false);
            model.Type = type;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Title = title;
            model.IsPublish = isPublish;
            model.IsSelected = isSelected;
            model.OrderBy = orderBy;
            return View(model);
        }

        // GET: Content/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [DisplayName("مشاهده صفحه ثبت")]

        public ActionResult Create(string type)
        {
            return View();
        }


        // POST: Content/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("ثبت")]

        public ActionResult Create(TagViewModel model)
        {


            if (ModelState.IsValid)
            {
                var tag = new Tag
                {
                    Title = model.Title.Trim(),
                    Type = model.Type,
                    EnglishTitle = model.EnglishTitle,
                    Content = model.Content != null ? model.Content.Replace("../../content/files/editor/", "/content/files/editor/")
                      .Replace("../content/files/editor/", "/content/files/editor/") : "",
                    IsPublish = model.IsPublish,
                    IsSelected = model.IsSelected,
                    Instagram = model.Instagram,
                    Twitter = model.Twitter,
                    Facebook = model.Facebook,
                    Spotify = model.Spotify,
                    Website = model.Website,
                    Soundcloud = model.Soundcloud,
                    Itunes = model.Itunes,
                    SeoDescription = model.SeoDescription,
                    SeoKeyword = model.SeoKeyword


                };

                _tagService.AddNewTag(tag, model.Photo);
                _uow.SaveChanges();

                return RedirectToAction(nameof(Index), new { type = tag.Type });

            }
            else
            {
                return View(model);
            }

        }

        [DisplayName("بارگذاری تصویر در ایدتور متنی")]

        public ActionResult UploadImage(IFormFile file)
        {
            string fileName = "";
            long fileSize = 0;
            var size = new int[0];
            var resized = new string[0];
            fileName = _uploadService.UploadPicResize(file, "/content/files/editor", 1200, ref fileSize, size, EnumC.Dimensions.Width, ref resized);
            return Json(new { location = "/content/files/editor/" + fileName });
        }


        [BreadCrumb(Title = "ایندکس", Order = 1)]
        [DisplayName("مشاهده صفحه ویرایش")]

        public async Task<IActionResult> Edit(int id, string type)
        {
            var content = await _tagService.FindByIdAsync(id).ConfigureAwait(false);
            var m = new TagViewModel
            {
                Title = content.Title,
                EnglishTitle = content.EnglishTitle,
                PhotoFileName = content.Pic,
                Id = content.Id,
                Pic = content.Pic,
                Thumbnail = content.Thumbnail,
                IsPublish = content.IsPublish,
                IsSelected = content.IsSelected,
                Instagram = content.Instagram,
                Twitter = content.Twitter,
                Facebook = content.Facebook,
                Spotify = content.Spotify,
                Website = content.Website,
                Soundcloud = content.Soundcloud,
                Itunes = content.Itunes,
                SeoDescription = content.SeoDescription,
                SeoKeyword = content.SeoKeyword,

                Content = content.Content != null
                ? content.Content.Replace("../../content/files/editor/", "/content/files/editor/")
                  .Replace("../content/files/editor/", "/content/files/editor/")
                  .Replace("..//content/files/editor/", "/content/files/editor/")
                : ""
            };
            return View(viewName: nameof(Edit), model: m);

        }

        // POST: Content/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("ویرایش")]

        public async Task<IActionResult> Edit(TagViewModel model, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var content = await _tagService.FindByIdAsync(id).ConfigureAwait(false);
                content.Title = model.Title.Trim();
                content.EnglishTitle = model.EnglishTitle;
                content.Thumbnail = model.Thumbnail;
                content.Pic = model.Pic;

                content.IsPublish = model.IsPublish;
                content.IsSelected = model.IsSelected;


                content.Instagram = model.Instagram;
                content.Twitter = model.Twitter;
                content.Facebook = model.Facebook;
                content.Spotify = model.Spotify;
                content.Website = model.Website;
                content.Soundcloud = model.Soundcloud;
                content.Itunes = model.Itunes;
                content.SeoDescription = model.SeoDescription;
                content.SeoKeyword = model.SeoKeyword;

                content.Content = model.Content != null ? model.Content
                  .Replace("../../content/files/editor/", "/content/files/editor/")
                  .Replace("../content/files/editor/", "/content/files/editor/")
                  .Replace("..//content/files/editor/", "/content/files/editor/") : "";


                _tagService.UpdateTag(content, model.Photo);

                return RedirectToAction(nameof(Index), new { type = content.Type });
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف")]

        public IActionResult Delete(int id, string type)
        {
            _tagService.Delete(id);
            return RedirectToAction("Index", new { type = type });
        }

        [DisplayName("نمایش در لیست های کشویی")]

        public ActionResult GetTags(string term, string type)
        {
            term = term.Split(',').Last().Trim();
            var list = _tagService.GetStartBy(term, 7, type).Select(p => new
            {
                label = p.Title,
                value = p.Title,
                id = p.Id,
            }).ToList();
            return Json(list);
        }
        [DisplayName("نمایش در لیست های کشویی")]

        public ActionResult GetStartBy(string searchTerm, int pageSize, int pageNum, string type)
        {
            var itemList = _tagService.GetStartBy(pageNum, pageSize, type, searchTerm);
            var result = new
            {
                Total = _tagService.GetCount("1", searchTerm),
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
