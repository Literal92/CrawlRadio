using System.Collections.Generic;
using MusicProject.Entities.Identity;
using cloudscribe.Web.Pagination;

namespace MusicProject.ViewModels.Content
{
  public class ContentIndexViewModel
  {

    public List<Entities.Content.Content> Contents { get; set; }
    public List<Entities.Content.Category> Categories { get; set; }


  }
}