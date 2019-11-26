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
using Newtonsoft.Json;

namespace MusicProject.Areas.Admin.Controllers
{

    [Area(AreaConstants.AdminArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]

    [DisplayName("کنترلر مدیریت اسلایدر")]

    public class SliderController : Controller
    {
        private const string RoleNotFound = "نقش درخواستی یافت نشد.";

        private readonly ISliderService _sliderService;

        private readonly IUnitOfWork _uow;
        private readonly ITagService _tagService;



        private const int DefaultPageSize = 20;

        public SliderController(
          ISliderService sliderService,
          ITagService tagService,
          IUnitOfWork uow

          )
        {
            _sliderService = sliderService;
            _uow = uow;
            _tagService = tagService;


        }


        [DisplayName("مشاهده صفحه ثبت اسلایدر")]
        public ActionResult Create(int type)
        {
            return View();
        }


        // POST: Content/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("ثبت اسلایدر جدید")]
        public ActionResult Create(SliderViewModel model, int type)
        {
            if (ModelState.IsValid)
            {
                var user = new Slider
                {
                    Title = model.Title.Trim(),
                    TypeId = model.TypeId,
                    Link = model.Link,
                    State = model.State,
                    SubTitle = model.SubTitle
                };

                _sliderService.AddNewSlider(user, model.Photo);
                _uow.SaveChanges();
                return RedirectToAction(nameof(Index), new { type = user.TypeId });
            }
            return View(model);
        }

        [DisplayName("مشاهده صفحه ویرایش اسلایدر")]

        public ActionResult Edit(int type, int id)
        {
            var c = _sliderService.FindByIdAsync(id).Result;
            var m = new SliderViewModel
            {
                Title = c.Title,
                TypeId = c.TypeId,
                Thumbnail = c.Thumbnail,
                Pic = c.Pic,
                Id = c.Id,
                State = c.State,
                Link = c.Link,
                PhotoFileName = c.Pic,
                SubTitle = c.SubTitle
            };

            return View(m);
        }

        // POST: Content/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("ویرایش اسلایدر")]

        public async Task<IActionResult> Edit(SliderViewModel model, int id)
        {

            if (ModelState.IsValid)
            {
                var content = await _sliderService.FindByIdAsync(id).ConfigureAwait(false);
                content.Title = model.Title.Trim();
                content.Thumbnail = model.Thumbnail;
                content.Link = model.Link;
                content.State = model.State;
                content.SubTitle = model.SubTitle;
                _sliderService.UpdateSlider(content, model.Photo);
                return RedirectToAction(nameof(Index), new { type = content.TypeId });
            }
            return View();

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف اسلایدر")]

        public IActionResult Delete(int id, string type)
        {
            _sliderService.Delete(id);
            return RedirectToAction("Index", new { type = type });
        }


        [BreadCrumb(Title = "ایندکس", Order = 1)]
        [DisplayName("لیست اسلایدرها")]        
        public async Task<IActionResult> Index(
          int type = 0,
          int pageNumber = 1,
          int pageSize = -1,
          string sort = "desc",
          string state = ""
          )
        {
            var itemsPerPage = 10;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = await _sliderService.GetPagedSliderAsync(
              pageNumber, itemsPerPage, sort == "desc" ? SortOrder.Descending : SortOrder.Ascending, type, state).ConfigureAwait(false);
            model.Type = type;
            model.Paging.CurrentPage = pageNumber;
            model.State = state;
            model.Paging.ItemsPerPage = itemsPerPage;
            return View(model);
        }
    }
}
