using System.Collections.Generic;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
  public class ContentDetailViewModel
  {
    public int Id;
    

    public string Title { get; set; }


    public int VisitCount { get; set; }

    public string RegisterDate { get; set; }
    public string RegisterTime { get; set; }



    public string Link { get; set; }



    public int Priority { get; set; }

    public CategoryViewModel Category { get; set; }

    public int CategoryId { get; set; }

    public string Pic3 { get; set; }
    public string PhotoFileName { set; get; }
    


    public string SvgStr { get; set; }


    public int TypeId { get; set; }

    public string HeadLine { get; set; }

    public string SubTitle { get; set; }
    public string Lead { get; set; }
    public string ContentText { get; set; }

    

    public string SeoTitle { get; set; }
    public string SeoDescription { get; set; }
    public string SeoKeyboard { get; set; }
    public string SeoUrl { get; set; }


    public string Pic { get; set; }

    public string Video { get; set; }

    public string Thumbnail { get; set; }
    public string Thumbnail3 { get; set; }
    public string MediumPic { get; set; }

    public string Tags { get; set; }
    public ICollection<ContentTag> ContentTags { get; set; }
    public ICollection<ContentFile> ContentFiles { get; set; }
  }
}