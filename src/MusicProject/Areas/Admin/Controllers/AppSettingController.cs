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

  [DisplayName("کنترلر مدیریت تظیمات اپ")]

  public class AppSettingController : Controller
  {
    private const string RoleNotFound = "اپ درخواستی یافت نشد.";

    private readonly IAppSettingService _appSettingService;
    
    private const int DefaultPageSize = 20;

    public AppSettingController(
      IAppSettingService appSettingService
      )
    {
      _appSettingService = appSettingService;
    }


        [DisplayName("مشاهده صفحه ثبت تنظیمات")]

        public ActionResult Create(int type)
    {
      return View();
    }

        [DisplayName("  ثبت تنظیمات")]

        [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(AppSettingViewModel model, int type)
    {

      if (ModelState.IsValid)
      {
        var user = new AppSetting
        {
          TypeId = model.TypeId,
          AboutUs = model.AboutUs,
          CafebazarLink = model.CafebazarLink,
          ChangeLog= model.ChangeLog,
          Instagram = model.Instagram,
          Site = model.Site,
          Soundcloud = model.Soundcloud,
          Telegram = model.Telegram
        };
        _appSettingService.AddNewAppSetting(user);
        return RedirectToAction(nameof(Index), new { type = user.TypeId });
      }
      return View(model);
    }

        [DisplayName("مشاهده صفحه ویرایش تنظیمات")]

        public ActionResult Edit(int type, int id)
    {
      var c = _appSettingService.FindByIdAsync(id).Result;
      var m = new AppSettingViewModel
      {
         TypeId = c.TypeId,
          AboutUs = c.AboutUs,
          CafebazarLink = c.CafebazarLink,
          ChangeLog= c.ChangeLog,
          Instagram = c.Instagram,
          Site = c.Site,
          Soundcloud = c.Soundcloud,
          Telegram = c.Telegram
      };

      return View(m);
    }

    // POST: Content/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
        [DisplayName("ویرایش تنظیمات")]

        public async Task<IActionResult> Edit(AppSettingViewModel model, int id)
    {

      if (ModelState.IsValid)
      {
        var content = await _appSettingService.FindByIdAsync(id).ConfigureAwait(false);
        content.AboutUs = model.AboutUs.Trim();
        content.CafebazarLink = model.CafebazarLink;
        content.ChangeLog = model.ChangeLog;
        content.Instagram= model.Instagram;
        content.Site= model.Site;
        content.Soundcloud= model.Soundcloud;
        content.Telegram= model.Telegram;
        content.TypeId= model.TypeId;
        _appSettingService.UpdateAppSetting(content);
        return RedirectToAction(nameof(Index), new { type = content.TypeId });

      }
      return View();
     
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
        [DisplayName("حذف تنظیمات")]

        public IActionResult Delete(int id, string type)
    {
      _appSettingService.Delete(id);
      return RedirectToAction("Index", new { type = type });
    }


    [BreadCrumb(Title = "ایندکس", Order = 1)]
        [DisplayName(" لیست تنظیمات")]

        public async Task<IActionResult> Index(
      int type = 0,
      int pageNumber = 1,
      int pageSize = -1,
      string sort = "desc"
      )
    {
      var itemsPerPage = 10;
      if (pageSize > 0)
        itemsPerPage = pageSize;

      var model = await _appSettingService.GetPagedAppSettingAsync(
        pageNumber, itemsPerPage, sort == "desc" ? SortOrder.Descending : SortOrder.Ascending, type).ConfigureAwait(false);
      model.Type = type;
      model.Paging.CurrentPage = pageNumber;
      model.Paging.ItemsPerPage = itemsPerPage;
      return View(model);

    }
  }
}
