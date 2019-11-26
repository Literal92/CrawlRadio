using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;

namespace MusicProject.ViewModels.Content
{
  public class AppSettingViewModel
  {
    public int Id { get; set; }
    [Display(Name = "درباره ما")]

    public string AboutUs { get; set; }
    [Display(Name = "لینک کافه بازار")]

    public string CafebazarLink { get; set; }
    [Display(Name = "لیست تغییرات")]

    public string ChangeLog { get; set; }
    [Display(Name = "ادرس سایت")]

    public string Site { get; set; }
    [Display(Name = "اینستاگرام")]

    public string Instagram { get; set; }
    [Display(Name = "تلگرام")]

    public string Telegram { get; set; }
    [Display(Name = "سوندکلود")]

    public string Soundcloud { get; set; }
    public int TypeId { get; set; }
  }
}
