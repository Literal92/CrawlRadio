using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;
using MusicProject.Entities.Content;


namespace MusicProject.ViewModels.Content
{
  public class CategoryViewModel
  {
    public const string AllowedImages = ".png,.jpg,.jpeg,.gif";
  public const string AllowedZip = ".zip";
    public int Id;

    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "(*)")]
    [StringLength(450)]
    public string Title { get; set; }
    public int TypeId { get; set; }
    public string Pic { get; set; }
    public string Thumbnail { get; set; }
    [Display(Name = "نام به انگلیسی از این فیلد برای تولید پوشه آلبوم استفاده میشود")]

    public string Description { get; set; }

    public string RegisterDate { get; set; }
    public string RegisterTime { get; set; }
    [Display(Name = "توضیحات سئو")]
    public string SeoDescription { get; set; }


    [Display(Name = "متن")]
    public string ContentText { get; set; }

    [Display(Name = "انتشار")]
    public bool IsPublic { get; set; }

    [Display(Name = "منتخب")]
    public bool IsSelected { get; set; }

    [Display(Name = "ارسال نوتیفیکیشن")]
    public bool SendNotification { get; set; }

    [Display(Name = "تصویر")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس تصویر 1000 حرف است.")]
    public string PhotoFileName { set; get; }

    [UploadFileExtensions(AllowedImages,
      ErrorMessage = "لطفا تنها یک تصویر " + AllowedImages + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Photo { get; set; }




    [Display(Name = "فایل 128")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس  1000 حرف است.")]
    public string ZipMp3128Name { set; get; }

    [Display(Name = "فایل 64")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس  1000 حرف است.")]
    public string ZipMp364Name { set; get; }

 [Display(Name = "فایل 320")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس  1000 حرف است.")]
    public string ZipMp3320Name { set; get; }


    [UploadFileExtensions(AllowedZip,
      ErrorMessage = "لطفا تنها یک فایل زیپ " + AllowedZip + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile ZipMp3128 { get; set; }

    [UploadFileExtensions(AllowedZip,
      ErrorMessage = "لطفا تنها یک فایل زیپ " + AllowedZip + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile ZipMp364 { get; set; }

    [UploadFileExtensions(AllowedZip,
      ErrorMessage = "لطفا تنها یک فایل زیپ " + AllowedZip + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile ZipMp3320 { get; set; }




    public string ZipMp364Src { get; set; }
    public string ZipMp3128Src { get; set; }
    public string ZipMp3320Src { get; set; }



    public string Cats;
    public int? ParentId { get; set; }




    public string Tags { get; set; }
    public string Artists { get; set; }
    public string Styles { get; set; }
    public string Musics { get; set; }

    public ICollection<CategoryTag> CategoryTags { get; set; }
    public ICollection<CategoryTag> ArtistTags { get; set; }

    public ICollection<Entities.Content.Content> Contents { get; set; }


  }
}
