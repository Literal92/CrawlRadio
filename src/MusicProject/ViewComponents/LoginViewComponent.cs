using Microsoft.AspNetCore.Mvc;
using MusicProject.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicProject.ViewComponents
{
  public class LoginViewComponent : ViewComponent
  {
    public LoginViewComponent()
    {

    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
      LoginViewModel model =new LoginViewModel();
      return View(model);
    }
  }
}
