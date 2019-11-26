using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;
using MusicProject.Entities.Content;


namespace MusicProject.ViewModels.Content
{
  public class StyleViewModel
  {
    public int Id;
    public string Title { get; set; }
    public string Thumbnail { get; set; }
    public string Description { get; set; }
  }
}
