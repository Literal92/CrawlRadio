using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MusicProject.Entities.Content;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;

namespace MusicProject.ViewModels.Content
{
  public class ContentCreateViewModel
  {
    public int Id;
    public const string AllowedVideo = ".mp4";
    public const string AllowedImages = ".png,.jpg,.jpeg,.gif,.pdf";
    public const string AllowedMp3 = ".mp3";
    public const string AllowedFiles = ".png,.jpg,.jpeg,.gif,.svg,.mp3,.mp4";
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "لطفا عنوان را وارد نمایید")]
    [StringLength(450)]

    public string Title { get; set; }

    [Display(Name = "توضیحات")]

    //[StringLength(450)]

    public string Description { get; set; }
    public string RegisterDate { get; set; }
    public string RegisterTime { get; set; }

    [Display(Name = "آرشیو")]
    public bool IsArchive { get; set; }


    [Display(Name = "لینک به")]
    public string Link { get; set; }


    [Display(Name = "اولویت")]
    public int Priority { get; set; }

    public CategoryViewModel Category { get; set; }

    [Display(Name = "دسته بندی")]
    //[Required(ErrorMessage = "لطفا دسته بندی را انتخاب  نمایید")]

    public int? CategoryId { get; set; }

    [Display(Name = "تصویر بزرگ صفحه اول")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس تصویر 1000 حرف است.")]
    public string PhotoFileName2 { set; get; }

    [UploadFileExtensions(AllowedImages,
      ErrorMessage = "لطفا تنها یک فایل  " + AllowedImages + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Photo2 { get; set; }

    [Display(Name = "تصویر دسته بندی")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس تصویر 1000 حرف است.")]
    public string PhotoFileName3 { set; get; }

    [UploadFileExtensions(AllowedImages,
      ErrorMessage = "لطفا تنها یک تصویر " + AllowedImages + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Photo3 { get; set; }

    [Display(Name = "تصویر   ")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس تصویر 1000 حرف است.")]
    public string PhotoFileName { set; get; }

    [UploadFileExtensions(AllowedImages,
      ErrorMessage = "لطفا تنها یک تصویر " + AllowedImages + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Photo { get; set; }

    [Display(Name = "ویدیو")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس فایل 1000 حرف است.")]
    public string VideoFileName { set; get; }

    [UploadFileExtensions(AllowedVideo,
      ErrorMessage = "لطفا تنها یک ویدیو " + AllowedVideo + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Video { get; set; }



    [Display(Name = "پی دی اف ")]
    public string PdfFileName { set; get; }
    [UploadFileExtensions(AllowedMp3,
      ErrorMessage = "لطفا تنها یک فایل صوتی " + AllowedMp3 + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Pdf { get; set; }


    [Display(Name = "فایل 128 ")]
    public string Mp3128Name { set; get; }
    [UploadFileExtensions(AllowedMp3,
      ErrorMessage = "لطفا تنها یک فایل صوتی " + AllowedMp3 + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Mp3128 { get; set; }


    [Display(Name = "فایل 64 ")]
    public string Mp364Name { set; get; }
    [UploadFileExtensions(AllowedMp3,
      ErrorMessage = "لطفا تنها یک فایل صوتی " + AllowedMp3 + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Mp364 { get; set; }


    [Display(Name = "فایل 320 ")]
    public string Mp3320Name { set; get; }
    [UploadFileExtensions(AllowedMp3,
      ErrorMessage = "لطفا تنها یک فایل صوتی " + AllowedMp3 + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Mp3320 { get; set; }




    [UploadFileExtensions(AllowedFiles,
      ErrorMessage = "لطفا تنها یک تصویر " + AllowedFiles + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile[] Files { get; set; }



    [UploadFileExtensions(AllowedFiles,
      ErrorMessage = "لطفا تنها یک تصویر " + AllowedFiles + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Svg { get; set; }

    public string SvgStr { get; set; }


    public int TypeId { get; set; }

    [Display(Name = "رو تیتر")]

    public string HeadLine { get; set; }
    [Display(Name = "زیر تیتر")]

    public string SubTitle { get; set; }
    public string Lead { get; set; }
    public string ContentText { get; set; }






    public string SeoTitle { get; set; }
    [Display(Name = "توضیحات سئو")]
    public string SeoDescription { get; set; }
    public string SeoKeyboard { get; set; }
    public string SeoUrl { get; set; }

    public string Mp364Bit { get; set; }
    public string Mp3128Bit { get; set; }
    public string Mp3320Bit { get; set; }

    public string Pic { get; set; }
    public string Thumbnail { get; set; }
    public string MediumPic { get; set; }


    public string Pic2 { get; set; }
    public string Thumbnail2 { get; set; }
    public string MediumPic2 { get; set; }

    public string Pic3 { get; set; }
    public string Thumbnail3 { get; set; }
    public string MediumPic3 { get; set; }

    public string Tags { get; set; }
    public ICollection<ContentTag> ContentTags { get; set; }
    public ICollection<ContentFile> ContentFiles { get; set; }
  }
}