using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicProject.Services.Identity;

namespace MusicProject.API.Controllers.Api
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

  [Route("/api/customers")]
  public class ProtectedController : Controller
  {
    public ProtectedController()
    {

    }

    public IActionResult Get()
    {
      var loggedInUser = User.Identity.Name;
      return Ok(new[] { "One", "Two", "Three" });
    }

  }
}