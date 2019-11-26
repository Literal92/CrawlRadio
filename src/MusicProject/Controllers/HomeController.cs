using System.IO;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc;
using MusicProject.Services.Contracts.Content;
using Microsoft.AspNetCore.Hosting;
namespace MusicProject.Controllers
{
    [BreadCrumb(Title = "خانه", UseDefaultRouteUrl = true, Order = 0)]
  public class HomeController : Controller
  {
    private readonly ICategoryService _categoryService;
    private readonly ISliderService _sliderService;
   
    private readonly ISessionRequestService _sessionRequestService;
    private readonly IContactUsService _contactUsService;
    private readonly IContentSelectedService _contentSelectedService;

    private readonly IContentService _contentService;
    public HomeController(
      IContentService contentService,
      ICategoryService categoryService,
      ISessionRequestService sessionRequestService,
      IContentSelectedService contentSelectedService,
      IContactUsService contactUsService,   
      ISliderService sliderService    
    )
    {
      _contentService = contentService;
      _contentSelectedService = contentSelectedService;
      _categoryService = categoryService;
      _sessionRequestService = sessionRequestService;
      _contactUsService = contactUsService;     
      _sliderService = sliderService;     
    }
  

    [BreadCrumb(Title = "ایندکس", Order = 1)]
    public  IActionResult Index()
    {
    
      return View();
      //  return View();
    }

    
    [BreadCrumb(Title = "خطا", Order = 1)]
    public IActionResult Error()
    {
      return View();
    }

    [HttpGet("/.well-known/acme-challenge/{id}")]
    public IActionResult LetsEncrypt(string id, [FromServices] IHostingEnvironment env)
    {
      id = Path.GetFileName(id); // security cleaning
      var file = Path.Combine(env.ContentRootPath, ".well-known", "acme-challenge", id);
      return PhysicalFile(file, "text/plain");
    }

  }
}