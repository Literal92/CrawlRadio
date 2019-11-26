using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MusicProject.Common.GuardToolkit;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Services.Content;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;
using MusicProject.Services.Identity;
using MusicProject.ViewModels.Content;
using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MusicProject.Common;
using MusicProject.ViewModels.Identity;
using AutoMapper;
using AutoMapper.Mappers;

namespace MusicProject.Areas.Admin.Controllers
{

    [Area(AreaConstants.AdminArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]

    [DisplayName("کنترلر مدیریت آهنگ ها")]
    public class ContentController : Controller
    {
        private const string RoleNotFound = "رکورد درخواستی یافت نشد.";
        private readonly IContentService _contentService;
        private readonly IContentListService _contentListService;
        private readonly IContentFileService _contentFileService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<ContentController> _logger;
        private readonly IUploadService _uploadService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IContentSelectedService _contentSelectedService;
        private readonly IMapper _mapper;

        private const int DefaultPageSize = 7;

        public ContentController(
            IContentService contentService,
            IContentFileService contentFileService,
            ICategoryService categoryService,
            ITagService tagService,
            IContentListService contentListService,
            IUploadService uploadService,
            IHostingEnvironment hostingEnvironment,
            IContentSelectedService contentSelectedService,
            IUnitOfWork uow,
            ILogger<ContentController> logger,
            IMapper mapper
        )
        {
            _contentListService = contentListService;
            _contentService = contentService;
            _contentFileService = contentFileService;
            _categoryService = categoryService;
            _tagService = tagService;
            _uow = uow;
            _logger = logger;
            _logger.CheckArgumentIsNull(nameof(_logger));
            _uploadService = uploadService;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
            _contentSelectedService = contentSelectedService;

        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        [DisplayName("مشاهده لیست آهنگ ها")]

        public async Task<IActionResult> Index(
            string type = "",
            int pageNumber = 1,
            int pageSize = -1,
            string sort = "desc",
            string archive = "",
            string categoryid = "",
            string title = "",
            string from = "",
            string to = ""

        )
        {

            if (type != null)
            {

                if (categoryid != "" && categoryid != "0")
                {
                    ViewBag.CategoryName = _categoryService.FindByIdAsync(Convert.ToInt32(categoryid)).Result.Title;
                    ViewBag.CategoryId = categoryid;
                }
                else
                {
                    ViewBag.CategoryName = "تمام آلبوم ها";
                    ViewBag.CategoryId = 0;
                }
                //var cats = _categoryService.GetIndentedCategory(Convert.ToInt32(type));

                //ViewBag.CategoryId = new SelectList(cats, "Id", "Title", null);
            }

            ViewBag.CurrentDate = DateTime.Now.ToShortPersianDateString();




            var itemsPerPage = 10;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            bool? isArchive = null;
            if (archive == "1")
                isArchive = true;
            else if (archive == "0")
                isArchive = false;
            if (!categoryid.IsNumber())
            {
                categoryid = "0";
            }

            var model = await _contentService.GetPagedContentsAsync(
                pageNumber,
                itemsPerPage,
                sort == "desc" ? SortOrder.Descending : SortOrder.Ascending,
                type, isArchive,
                Convert.ToInt16(categoryid), from, to, title).ConfigureAwait(false);
            model.Type = type;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.CategoryId = categoryid;
            model.Title = title;
            model.From = from;
            model.To = to;
            return View(model);
        }

        // GET: Content/Details/5
        //public ActionResult Details(int id)
        //{
        //    var content = _contentService.GetTopByType("1", 1, 10000, "").Where(p => string.IsNullOrEmpty(p.Mp364))
        //        .ToList();

        //    foreach (var c in content)
        //    {

        //        var directoryName = c.Category.Description.SafeFileName();

        //        if (!Directory.Exists(_hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName +
        //                              "/64"))
        //            Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "/content/files/album/" +
        //                                      directoryName + "/64");

        //        if (!string.IsNullOrEmpty(c.Mp3128))
        //        {
        //            c.Svg = "1";
        //            if (!c.Mp3128.StartsWith(directoryName))
        //            {
        //                var filename = directoryName + "/64/" + c.Mp3128;
        //                c.Mp364 = filename;
        //                c.Mp3128 = directoryName + "/" + c.Mp3128;
        //            }
        //            else
        //            {
        //                var filename = directoryName + "/64/" + c.Mp3128.Replace(directoryName + "/", "");
        //                c.Mp364 = filename;
        //            }

        //            if (string.IsNullOrEmpty(c.Mp3320) && System.IO.File.Exists(
        //                    _hostingEnvironment.WebRootPath + "/content/files/album/" + directoryName + "/320/" +
        //                    c.Mp3128.Replace(directoryName + "/", "")))
        //            {
        //                c.Mp3320 = directoryName + "/320/" + c.Mp3128.Replace(directoryName + "/", "");
        //            }

        //            _contentService.UpdateContent(c, null, null, null, null, null, null, null, null, null);
        //        }


        //    }

        //    return View();
        //}

        [DisplayName("مشاهده صفحه ثبت آهنگ")]

        public ActionResult Create(int type,
            int categoryId
        )
        {
            ViewBag.CurrentDate = DateTime.Now.ToShortPersianDateString();

            //   var cats = _categoryService.GetTopByType("1",1,20,"");


            //  int? selected = null;
            //  selected = categoryId;

            if (type == 20 || type == 110 || type == 120 || type == 130 || type == 140 || type == 150)
            {
                // selected = cats.FirstOrDefault().Id;
            }

            if (categoryId != 0)
            {
                ViewBag.CategoryId = categoryId;
                ViewBag.CategoryName = _categoryService.FindByIdAsync(categoryId).Result.Title;
            }

            return View();
        }


        // POST: Content/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisableRequestSizeLimit]
        [DisplayName(" ثبت آهنگ")]

        public ActionResult Create(ContentCreateViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = string.Empty;

                if (model.Svg != null)
                {
                    using (var reader = new StreamReader(model.Svg.OpenReadStream()))
                    {
                        result = reader.ReadToEnd().Replace("\n", " ").Replace("\r", " ").Replace("\t", " ")
                            .Replace(
                                "<!-- Generator: Adobe Illustrator 16.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->",
                                "");
                    }
                }

                var user = new Content
                {
                    Title = model.Title.Trim(),
                    Lead = model.Description,
                    Priority = model.Priority,
                    CategoryId = (int)model.CategoryId,
                    TypeId = model.TypeId,
                    Svg = result,
                    Link = model.Link,
                    IsArchive = model.IsArchive,
                    HeadLine = model.HeadLine,
                    SubTitle = model.SubTitle,
                    SeoDescription = model.SeoDescription,
                    SeoTitle = model.SeoTitle,
                    ContentText = model.ContentText != null
                        ? model.ContentText.Replace("../../content/files/editor/", "/content/files/editor/")
                            .Replace("../content/files/editor/", "/content/files/editor/")
                        : ""
                };


                if (model.RegisterTime.ToLower().Contains("pm"))
                {
                    var m = model.RegisterTime.Split(":");
                    model.RegisterTime = (Convert.ToInt32(m[0]) == 12 ? 0 : Convert.ToInt32(m[0])) + 12 + ":" +
                                         m[1].ToLower().Replace("pm", "").Trim();
                }

                user.PublishDateTime =
                    (model.RegisterDate + " , " + model.RegisterTime.ToLower().Replace("am", "").Trim())
                    .ToGregorianDateTime();

                if (model.Tags != null)
                {
                    var tags = model.Tags.Split(",");
                    List<ContentTag> c = new List<ContentTag>();
                    foreach (var t in tags)
                    {
                        c.Add(new ContentTag
                        {
                            TagId = Convert.ToInt32(t),
                        });
                    }

                    user.ContentTags = c;
                }

                _contentService.AddNewContent(user, model.Photo, model.Photo2, model.Photo3, model.Files, model.Video,
                    model.Pdf, model.Mp364, model.Mp3128, model.Mp3320);

                _uow.SaveChanges();

                return RedirectToAction(nameof(Index), new { type = user.TypeId, categoryid = user.CategoryId });

            }

            ViewBag.CurrentDate = DateTime.Now.ToShortPersianDateString();

            var cats = _categoryService.GetIndentedCategory(model.TypeId);


            //int? selected = null;
            //selected = model.CategoryId;

            //ViewBag.CategoryId = new SelectList(cats, "Id", "Title", selected);
            return View(model);

        }

        [DisplayName("بارگذاری تصویر در ادیتور متن")]

        public ActionResult UploadImage(IFormFile file)
        {
            string fileName = "";
            long fileSize = 0;
            var size = new int[0];
            var resized = new string[0];
            fileName = _uploadService.UploadPicResize(file, "/content/files/editor", 1200, ref fileSize, size,
                EnumC.Dimensions.Width, ref resized);
            return Json(new { location = "/content/files/editor/" + fileName });
        }


        [BreadCrumb(Title = "ایندکس", Order = 1)]
        [DisplayName("مشاهده صفحه ویرایش آهنگ")]

        public async Task<IActionResult> Edit(int id, int type)
        {
            var content = await _contentService.FindByIdAsync(id).ConfigureAwait(false);

            var m = new ContentCreateViewModel();
            m.CategoryId = content.CategoryId;
            m.Description = content.Lead;
            m.SeoDescription = content.SeoDescription;
            m.Title = content.Title;
            m.PhotoFileName = content.Pic;
            m.Id = content.Id;
            m.VideoFileName = content.Video;
            m.Priority = content.Priority;

            m.Pic2 = content.Pic2;
            m.Thumbnail2 = content.Thumbnail2;
            m.MediumPic2 = content.MediumPic2;

            m.Pic = content.Pic;
            m.Thumbnail = content.Thumbnail;
            m.MediumPic = content.MediumPic;

            m.Pic3 = content.Pic3;
            m.Thumbnail3 = content.Thumbnail3;
            m.MediumPic3 = content.MediumPic3;

            m.Link = content.Link;
            m.Mp364Bit = content.Mp364;
            m.Mp3128Bit = content.Mp3128;
            m.Mp3320Bit = content.Mp3320;
            m.SvgStr = content.Svg;
            m.HeadLine = content.HeadLine;
            m.SubTitle = content.SubTitle;
            m.SeoTitle = content.SeoTitle;
            m.IsArchive = content.IsArchive;
            m.Tags = content.ContentTags != null
                ? (string.Join(",", content.ContentTags.Select(x => x.Id).ToArray()))
                : "";
            m.ContentTags = content.ContentTags;
            m.ContentFiles = content.ContentFiles;
            m.RegisterDate = content.PublishDateTime.ToShortPersianDateString();
            m.RegisterTime = content.PublishDateTime != null ? content.PublishDateTime.Value.ToString("h:mm:tt") : "";
            m.ContentText = content.ContentText != null
                ? content.ContentText.Replace("../../content/files/editor/", "/content/files/editor/")
                    .Replace("../content/files/editor/", "/content/files/editor/")
                    .Replace("..//content/files/editor/", "/content/files/editor/")
                : "";


            //var cats = _categoryService.FindByIdAsync(content.CategoryId).Result;
            //List<Category> dd=new List<Category>();
            //dd.Add(cats);
            //ViewBag.CategoryId = new SelectList(dd, "Id", "Title", null);
            ViewBag.CategoryName = content.Category.Title;
            return View(viewName: nameof(Edit), model: m);
        }

        // POST: Content/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisableRequestSizeLimit]
        [DisplayName("ویرایش آهنگ")]

        public async Task<IActionResult> Edit(ContentCreateViewModel model, int id)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    ViewBag.CurrentDate = DateTime.Now.ToShortPersianDateString();

                    var cats = _categoryService.GetIndentedCategory(model.TypeId);


                    int? selected = null;
                    selected = model.CategoryId;

                    ViewBag.CategoryId = new SelectList(cats, "Id", "Title", selected);
                    return View(model);
                }






                var content = await _contentService.FindByIdAsync(id).ConfigureAwait(false);
                content.Lead = model.Description;
                content.CategoryId = (int)model.CategoryId;
                content.Title = model.Title.Trim();
                content.Thumbnail = model.Thumbnail;
                content.MediumPic = model.MediumPic;
                content.Pic = model.Pic;
                content.Priority = model.Priority;
                content.Link = model.Link;
                content.HeadLine = model.HeadLine;
                content.SeoTitle = model.SeoTitle;
                content.Video = model.VideoFileName;
                content.SeoDescription = model.SeoDescription;
                content.Pdf = model.PdfFileName;
                // content.ContentFiles = model.ContentFiles;

                content.SubTitle = model.SubTitle;
                content.ContentText = model.ContentText != null
                    ? model.ContentText
                        .Replace("../../content/files/editor/", "/content/files/editor/")
                        .Replace("../content/files/editor/", "/content/files/editor/")
                        .Replace("..//content/files/editor/", "/content/files/editor/")
                    : "";

                if (model.RegisterTime.ToLower().Contains("pm"))
                {
                    var m = model.RegisterTime.Split(":");
                    model.RegisterTime = (Convert.ToInt32(m[0]) == 12 ? 0 : Convert.ToInt32(m[0])) + 12 + ":" +
                                         m[1].ToLower().Replace("pm", "").Trim();

                }

                content.PublishDateTime =
                    (model.RegisterDate + " , " + model.RegisterTime.ToLower().Replace("am", "").Trim())
                    .ToGregorianDateTime();
                if (model.Tags != null)
                {
                    var tags = model.Tags.Split(",");
                    List<ContentTag> c = new List<ContentTag>();
                    foreach (var t in tags)
                    {
                        c.Add(new ContentTag
                        {
                            TagId = Convert.ToInt32(t),
                        });

                    }

                    content.ContentTags.Clear();
                    content.ContentTags = c;
                }
                else
                {
                    content.ContentTags.Clear();
                }



                if (model.Svg != null)

                    using (var reader = new StreamReader(model.Svg.OpenReadStream()))
                    {
                        content.Svg = reader.ReadToEnd().Replace("\n", " ").Replace("\r", " ").Replace("\t", " ")
                            .Replace(
                                "<!-- Generator: Adobe Illustrator 16.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->",
                                "");
                    }


                else

                    content.Svg = model.SvgStr;


                _contentService.UpdateContent(content, model.Photo, model.Photo2, model.Photo3, model.Files,
                    model.Video, model.Pdf, model.Mp364, model.Mp3128, model.Mp3320);

                return RedirectToAction(nameof(Index), new { type = content.TypeId });
            }
            catch (Exception e)
            {
                return View();
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف آهنگ")]

        public IActionResult Delete(int id, string type, string categoryId)
        {
            _contentService.Delete(id);
            return RedirectToAction("Index", new { type = type, categoryId });
        }

        //[AjaxOnly]
        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //[DisplayName("حذف تص آهنگ")]

        //public IActionResult DelPic(int id)
        //{

        //    _contentFileService.Delete(id);
        //    return Json(true);

        //}




        //public ActionResult GetTags(string term)
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
        [DisplayName("مشاهده لیست آهنگ ها در منوهای کشویی")]

        public ActionResult GetStartBy(string searchTerm, int pageSize, int pageNum, string countyId)
        {
            var itemList = _contentService.GetTopByType("1", pageNum, pageSize, searchTerm);
            var result = new
            {
                Total = _contentService.GetCount(1, searchTerm),
                Results = itemList.Select(p => new Select2Item()
                {
                    Id = p.Id,
                    Title = p.Title
                })
            };
            return new JsonResult(result);
        }

        ////[AjaxOnly]
        //public IActionResult RenderContentListModal([FromBody]ModelIdViewModel model)
        //{
        //    //if (string.IsNullOrWhiteSpace(model?.Id))
        //    //{
        //    //    return PartialView("_ContentList");
        //    //}

        //    //            var result = _mapper.Map<TypeIWantToMapTo>(originalObject);

        //    var content = _contentListService.GetAllContentList("1").Select(p => new ContentListViewModel
        //    {
        //        Id = p.Id,
        //        Title = p.Title
        //    });
        //    return PartialView("_ContentList", model: content);
        //}
        [DisplayName("مشاهده لیست پخش های یک آهنگ در صفحه لیست آهنگ ها")]

        public async Task<IActionResult> ContentListIndex(int id, string title, int? page = 1, string field = "Id",
            SortOrder order = SortOrder.Ascending)
        {
            var model = await _contentListService.GetPagedContentListAsync(
                pageNumber: page.Value,
                pageSize: 5,
                sortOrder: order,
                type: "1",
                archive:true,
                title: title,
                id: id,
                tagId:0
                );

            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = 5;
            model.Paging.ShowFirstLast = true;
            model.Title = title;
            model.Id = id;

            if (HttpContext.Request.IsAjaxRequest())
            {
                return PartialView("_ContentList", model);
            }
            return View("Index", model);
        }

        [HttpPost]
        //  [ValidateAntiForgeryToken]
        [DisplayName("اضافه آهنگ به یک لیست پخش در لیست آهنگ ها")]

        public ActionResult AddToContentList(int id, string type, int listId, int order)
        {

            if (ModelState.IsValid)
            {
                _contentSelectedService.AddNewContentSelected(new ContentSelected
                {
                    ContentId = id,
                    ContentListId = listId,
                    Order = order,
                    Type = type,
                });
                _uow.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: Content/Create
        [HttpPost]
        //        [ValidateAntiForgeryToken]
        [DisplayName("حذف آهنگ از یک لیست پخش در لیست آهنگ ها")]

        public ActionResult DeleteFromContentList(int id, string type, int listId)
        {
            if (ModelState.IsValid)
            {
                _contentSelectedService.Delete(id, listId);
                _uow.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //public ActionResult CheckExistInContentList(int id, int listId)
        //{
        //    return View("Index");
        //}
    }
}
