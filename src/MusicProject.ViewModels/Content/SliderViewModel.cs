using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
  public class SliderViewModel
  {
    public const string AllowedImages = ".png,.jpg,.jpeg,.gif";

    public int Id { get; set; }
    public int TypeId { get; set; }
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string Pic { get; set; }
    public string Thumbnail { get; set; }
    public string Link { get; set; }
    public string State { get; set; }
    public bool IsAlbum { get; set; }
    public List<Entities.Content.Content> Musics { get; set; }

    [Display(Name = "تصویر")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس تصویر 1000 حرف است.")]
    public string PhotoFileName { set; get; }

    [UploadFileExtensions(AllowedImages,
      ErrorMessage = "لطفا تنها یک تصویر " + AllowedImages + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Photo { get; set; }
    public Category Category { get; set; }
  }
}
