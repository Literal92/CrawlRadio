using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicProject.Services.Contracts.Content;
using MusicProject.ViewModels.Api;

namespace MusicProject.API.Controllers.Api
{

  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class MusicController : ControllerBase
  {
    private readonly IContentService _contentService;

    public MusicController(
      IContentService contentService
    )
    {
      _contentService = contentService;
    }

    [HttpPost]
    public ActionResult Visit([FromBody] PageLimitViewModel model)
    {
      var result = _contentService.UpdateVisit(model.Id).VisitCount;
      return Ok(result);
    }
  }
}