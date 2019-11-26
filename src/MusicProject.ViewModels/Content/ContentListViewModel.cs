using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;
using MusicProject.Entities.Content;

namespace MusicProject.ViewModels.Content
{
    public class ContentListViewModel
    {
        public const string AllowedImages = ".png,.jpg,.jpeg,.gif";

        public int Id { get; set; }
        [Display(Name = "عنوان")]

        public string Title { get; set; }
        [Display(Name ="توضیحات")]
        public string Description { get; set; }
        public string RegisterDate { get; set; }
        public string RegisterTime { get; set; }
        [Display(Name = "انتشار")]
        public bool IsPublish { get; set; }
        public string Type { get; set; }
        public string ArtistPic { get; set; }
        public string Content { get; set; }
        public string Pic { get; set; }
        public string Tags { get; set; }
        public string Artists { get; set; }
        public string Styles { get; set; }
        public string Musics { get; set; }
        public string Cats { get; set; }
        public bool IsExist { get; set; }
        public string ZipMp364 { get; set; }
        public string ZipMp3128 { get; set; }
        public string ZipMp3320 { get; set; }
        public string Thumbnail { get; set; }
        public int VisitCount { get; set; }
        public int LikeCount { get; set; }
        public string TotalItems { get; set; }
        [Display(Name = "ارسال نوتیفیکیشن")]
        public bool SendNotification { get; set; }

        [Display(Name = "تصویر")]
        [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر طول آدرس تصویر 1000 حرف است.")]
        public string PhotoFileName { set; get; }
        [UploadFileExtensions(AllowedImages,
          ErrorMessage = "لطفا تنها یک تصویر " + AllowedImages + " را ارسال نمائید.")]
        [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }
        public IList<Tag> StylesPL { get; set; }
        public IList<Tag> MusicsPL { get; set; }
        public List<Tag> TagsPL { get; set; }
        public IList<Tag> CatPL { get; set; }
        public IList<Tag> ArtistsPL { get; set; }
        public ICollection<ContentListTag> ContentListTags { get; set; }
        public List<Music> Contents { get; set; }
        public IList<ContentList> Suggested { get; set; }
        public IList<ContentListFile> ContentFile { get; set; }
        public IEnumerable<PagedCommentViewModel> Comments { get; set; }
        public List<Music> MusicsFiles { get; set; }
        public class Music
        {
            public int Id { get; set; }
            public string  Title { get; set; }
            public int Like { get; set; }
            public int Visit { get; set; }
            public string Mp364 { get; set; }
            public string Mp3128 { get; set; }
            public string Mp3320 { get; set; }
            public int Category { get; set; }
            public int Type { get; set; }
            public string Thumbnail { get; set; }
            public int Priority { get; set; }
        }
    }
}
