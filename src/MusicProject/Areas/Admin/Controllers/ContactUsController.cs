using System;
using System.ComponentModel;
using System.Data.SqlClient;
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

namespace MusicProject.Areas.Admin.Controllers
{

  [Area(AreaConstants.AdminArea)]
  [Authorize(Policy = ConstantPolicies.DynamicPermission)]
  [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]

  [DisplayName("کنترلر ارتباط با ما")]

  public class ContactUsController : Controller
  {
    private const string RoleNotFound = "رکورد درخواستی یافت نشد.";


    private readonly IContactUsService _contactUsService;

    private readonly IUnitOfWork _uow;



    private const int DefaultPageSize = 20;

    public ContactUsController(

    
      IContactUsService contactusService,
   



      IUnitOfWork uow

      )
    {
     
      _contactUsService = contactusService;
      _uow = uow;

    }


    [HttpPost]
    [ValidateAntiForgeryToken]
        [DisplayName("حذف ارتباط با ما")]

        public IActionResult Delete(int id, string type)
    {
      _contactUsService.Delete(id);
      return RedirectToAction("Index", new { type = type });
    }

    [BreadCrumb(Title = "ارتباط با ما", Order = 1)]
        [DisplayName("لیست ارتباط با ما")]

        public async Task<IActionResult> Index(
      string type = "",
      int pageNumber = 1,
      int pageSize = -1,
      string sort = "desc")
    {
      var itemsPerPage = 10;
      if (pageSize > 0)
      {
        itemsPerPage = pageSize;
      }

      var model = await _contactUsService.GetPagedContactUsAsync(
        pageNumber, itemsPerPage, sort == "desc" ? SortOrder.Descending : SortOrder.Ascending, type).ConfigureAwait(false);
      model.Type = type;
      model.Paging.CurrentPage = pageNumber;
      model.Paging.ItemsPerPage = itemsPerPage;
      return View(model);
    }
  }
}
