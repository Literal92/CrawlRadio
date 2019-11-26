using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;

namespace MusicProject.ViewModels.Content
{
    public class ContentListFileViewModel
    {     
         public int Id { get; set; }
      
        [Display(Name ="فایل کیفیت 64 ")]
        public string Mp364 { get; set; }
        [Display(Name = "فایل کیفیت 128 ")]
        public string Mp3128 { get; set; }
        [Display(Name = "فایل کیفیت 320 ")]
        public string Mp3320 { get; set; }
        [UploadFileExtensions(".mp3",
                ErrorMessage = "لطفا تنها یک فایل صوتی را ارسال نمائید.")]
        [DataType(DataType.Upload)]
        public IFormFile File1 { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile File2 { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile File3 { get; set; }
        [Display(Name ="عنوان")]
        public string Title { get; set; }
        [Display(Name ="اولویت")]
        public int Order { get; set; }
        public int LikeCount { get; set; }
        public int VisitCount { get; set; }
        public string Type { get; set; }

        [Display(Name = "لیست پخش")]
        [Required(ErrorMessage = "لطفا لیست پخش را انتخاب  نمایید")]

        public int ContentListId { get; set; }
    }
}
