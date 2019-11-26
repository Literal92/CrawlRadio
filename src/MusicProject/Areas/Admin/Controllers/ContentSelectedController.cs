using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Content;
using MusicProject.Services.Contracts.Content;
using MusicProject.Services.Identity;
using DNTBreadCrumb.Core;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MusicProject.Areas.Admin.Controllers
{

    [Area(AreaConstants.AdminArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]

    [DisplayName("کنترلر مدیریت آهنگ انتخابی در لیست پخش")]

    public class ContentSelectedController : Controller
    {
        private const string RoleNotFound = "رکورد درخواستی یافت نشد.";

        private readonly IContentSelectedService _contentSelectedService;

        private readonly IUnitOfWork _uow;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IContentService _contentService;



        private const int DefaultPageSize = 20;

        public ContentSelectedController(
          IContentService contentService,
          IContentSelectedService contentSelectedService,
          ITagService tagService,
          ICategoryService categoryService,
          IUnitOfWork uow
          )
        {
            _contentSelectedService = contentSelectedService;
            _uow = uow;
            _categoryService = categoryService;
            _contentService = contentService;
            _tagService = tagService;
        }


        [BreadCrumb(Title = "ایندکس", Order = 1)]
        [DisplayName("مشاهده آهنگهای انتخاب شده یک لیست پخش")]

        public async Task<IActionResult> Index(
          string type = "",
          int pageNumber = 1,
          int pageSize = -1,
          string sort = "desc",
          string archive = "",
          string categoryid = "",
          string title = "",
          string from = "",
          string to = "",
          int listId = 0,
          int musicId=0,
          int styleId=0,
          int artistId=0
          )
        {
            //if (type != null)
            //{
            //  var cats = _categoryService.GetIndentedCategory(Convert.ToInt32(type));

            //  ViewBag.CategoryId = new SelectList(cats, "Id", "Title", null);
            //}
            if (categoryid != "0" && categoryid != "")
            {
                ViewBag.CategoryName = _categoryService.FindByIdAsync(Convert.ToInt32(categoryid)).Result.Title;
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
              Convert.ToInt16(categoryid), from, to, title,artistId,styleId,musicId).ConfigureAwait(false);
            model.Type = type;
            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.CategoryId = categoryid;
            model.Title = title;
            model.From = from;
            model.To = to;
            model.ListId = listId;
            model.ArtistId = artistId;
            model.StyleId = styleId;

            if (musicId!=0)
            {
                ViewBag.MusicName = _tagService.FindByIdAsync(musicId).Result.Title;
            }
            if (styleId!=0)
            {
                ViewBag.StyleName = _tagService.FindByIdAsync(styleId).Result.Title;
            }
            if (artistId!=0)
            {
                ViewBag.ArtistName = _tagService.FindByIdAsync(artistId).Result.Title;
            }

            model.SelectedContents = _contentSelectedService.GetAllContentSelected(type, listId).OrderBy(p => p.Order).ToList();

            return View(model);
        }



        // POST: Content/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("انتخاب آهنگ برای لیست پخش")]

        public ActionResult Create(int id, string type, int listId, int order,
          int pageNumber = 1, string title = "", string from = "", string to = "",
          int categoryId = 0
          )
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

                return RedirectToAction(nameof(Index), new { type, listId, pageNumber, title, categoryId, from, to });

            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        // POST: Content/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف آهنگ از لیست پخش")]

        public ActionResult Delete(int id, string type, int listId, int pageNumber)
        {         

                _contentSelectedService.Delete(id);
                _uow.SaveChanges();
                return RedirectToAction(nameof(Index), new { type, listId, pageNumber });

           

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("ویرایش اولویت آهنگ در لیست پخش")]

        public ActionResult Edit(int id, string type, int listId, int pageNumber, int order)
        {

            if (ModelState.IsValid)
            {

                var selected = _contentSelectedService.FindByIdAsync(id).Result;
                selected.Order = order;

                _contentSelectedService.UpdateContentSelected(selected);

                _uow.SaveChanges();

                return RedirectToAction(nameof(Index), new { type, listId, pageNumber });

            }
            else
            {
                return BadRequest(ModelState);
            }

        }



    }
}
