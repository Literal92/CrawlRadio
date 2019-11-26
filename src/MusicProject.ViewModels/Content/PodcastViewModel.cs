using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;
using MusicProject.Entities.Content;


namespace MusicProject.ViewModels.Content
{
  public class PodcastViewModel
  {
    public const string AllowedImages = ".png,.jpg,.jpeg,.gif";
    public const string AllowedVideos = ".png,.jpg,.jpeg,.gif";

    public int Id;

    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "(*)")]
    [StringLength(450)]
    public string Title { get; set; }
    public int TypeId { get; set; }
    public string Pic { get; set; }
    public string Thumbnail { get; set; }

    [Display(Name = "توضیحات")]

    public string Description { get; set; }

    public string RegisterDate { get; set; }
    public string RegisterTime { get; set; }


    [Display(Name = "متن")]
    public string ContentText { get; set; }

    [Display(Name = "انتشار")]
    public bool IsPublic { get; set; }



    [Display(Name = "تصویر")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس تصویر 1000 حرف است.")]
    public string PhotoFileName { set; get; }

    [UploadFileExtensions(AllowedImages,
      ErrorMessage = "لطفا تنها یک تصویر " + AllowedImages + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Photo { get; set; }



    [Display(Name = "ویدیو")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس ویدوی 1000 حرف است.")]
    public string VideoFileName { set; get; }


    [UploadFileExtensions(AllowedImages,
      ErrorMessage = "لطفا تنها یک ویدیو " + AllowedVideos + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Video { get; set; }

    

 

    public string Category { get; set; }
    public string Tags { get; set; }
    public string Auther { get; set; }
    public string Speaker { get; set; }
    public string Editor { get; set; }

    public ICollection<PodcastTag> PodcastTags { get; set; }
    //public ICollection<PodcastTag> AutherTags { get; set; }

  


  }
}
