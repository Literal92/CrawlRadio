using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
  public class TagViewModel
  {
    public const string AllowedImages = ".png,.jpg,.jpeg,.gif";

    public int Id { get; set; }
    [Display(Name = "عنوان")]

    public string Title { get; set; }
    [Display(Name = "عنوان انگلیسی")]

    public string EnglishTitle { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
    public string Pic { get; set; }
    public string Thumbnail { get; set; }
    public bool IsPublish { get; set; }
    [Display(Name = "توضیحات سئو")]

    public string SeoDescription { get; set; }
    [Display(Name = "توضیحات کلمات کلیدی")]

    public string SeoKeyword { get; set; }
    public string Website { get; set; }
    public string Facebook { get; set; }
    public string Instagram { get; set; }
    public string Twitter { get; set; }
    public string Soundcloud { get; set; }
    public string Spotify { get; set; }
    public string Itunes { get; set; }
    public int VisitCount { get; set; }
    public int LikeCount { get; set; }
    public bool IsSelected { get; set; }
    [Display(Name = "تصویر")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس تصویر 1000 حرف است.")]
    public string PhotoFileName { set; get; }
    [UploadFileExtensions(AllowedImages,
      ErrorMessage = "لطفا تنها یک تصویر " + AllowedImages + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Photo { get; set; }
    public ICollection<ContentTag> ContentTags { get; set; }
    public ICollection<CategoryTag> CategoryTags { get; set; }
        public ICollection<ContentList> ContentLists { get; set; }
    }
}
