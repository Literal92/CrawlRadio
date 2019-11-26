using Microsoft.AspNetCore.Mvc;
using MusicProject.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicProject.ViewComponents
{
  public class RegisterViewComponent : ViewComponent
  {
    public RegisterViewComponent()
    {

    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
      RegisterViewModel model=new RegisterViewModel();
      return View(model);
    }
  }
}
