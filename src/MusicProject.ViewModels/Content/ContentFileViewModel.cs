using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;

namespace MusicProject.ViewModels.Content
{
  public class ContentFileViewModel
  {
    public const string AllowedImages = ".png,.jpg,.jpeg,.gif";
    public const string AllowedVideo = ".mp4";

    public int Id { get; set; }

    public string FileName { get; set; }
    public string FileName2 { get; set; }
    public string FileName3 { get; set; }
    [Display(Name = "عنوان")]

    public string Title { get; set; }
    public string Pic { get; set; }
    [Display(Name = "توضیحات")]

    public string Description { get; set; }
    public string ContentText { get; set; }
    public string Thumbnail { get; set; }
    public int LikeCount { get; set; }
    public int VisitCount { get; set; }
    public string Type { get; set; }
    public string Ext { get; set; }
    public long FileSize { get; set; }
    public long FileSize2 { get; set; }
    public long FileSize3 { get; set; }

    [Display(Name = "انتشار")]
    public bool IsPublic { get; set; }

    [Display(Name = "منتخب")]
    public bool IsSelected { get; set; }
    public ContentCreateViewModel Content { get; set; }

    [Display(Name = "دسته بندی")]
    [Required(ErrorMessage = "لطفا دسته بندی را انتخاب  نمایید")]

    public int ContentId { get; set; }

    [Display(Name = "تصویر")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس تصویر 1000 حرف است.")]
    public string PhotoFileName { set; get; }

    [UploadFileExtensions(AllowedImages,
      ErrorMessage = "لطفا تنها یک تصویر " + AllowedImages + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Photo { get; set; }


    [Display(Name = "ویدیو 1080")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس  1000 حرف است.")]
    public string VideoFileName { set; get; }

    [UploadFileExtensions(AllowedVideo,
      ErrorMessage = "لطفا تنها یک ویدیو " + AllowedVideo + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Video { get; set; }



    [Display(Name = " ویدیو 720")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس  1000 حرف است.")]
    public string VideoFileName2 { set; get; }

    [UploadFileExtensions(AllowedVideo,
      ErrorMessage = "لطفا تنها یک ویدیو " + AllowedVideo + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Video2 { get; set; }

    [Display(Name = " ویدیو 480")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس  1000 حرف است.")]
    public string VideoFileName3 { set; get; }

    [UploadFileExtensions(AllowedVideo,
      ErrorMessage = "لطفا تنها یک ویدیو " + AllowedVideo + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Video3 { get; set; }


  }
}
