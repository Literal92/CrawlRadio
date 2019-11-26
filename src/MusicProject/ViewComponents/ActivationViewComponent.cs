using Microsoft.AspNetCore.Mvc;
using MusicProject.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicProject.ViewComponents
{
  public class ActivationViewComponent : ViewComponent
  {
    public ActivationViewComponent()
    {

    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
      RegisterViewModel model=new RegisterViewModel();
      return View(model);
    }
  }
}
