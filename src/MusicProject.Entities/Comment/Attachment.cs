using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mime;
using System.Text;
using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Comment
{
    public class Attachment : IAuditableEntity
    {

        public int Id { get; set; }
        [DisplayName("نام پیوست")]
        public string Name { get; set; }
        [DisplayName("لینک پیوست")]
        public string Link { get; set; }
        [DisplayName("توضیحات پیوست")]
        public string Description { get; set; }
        [DisplayName("تاریخ")]
        public DateTimeOffset? CreatDateTime { get; set; }
        [DisplayName("نوع محتوا پیوست")]
        public string ContentType { get; set; }
        // public ContentType ContentType { get; set; }
        [DisplayName("حجم پیوست")]
        public byte ContentLength { get; set; }
        [DisplayName("معتبر")]
        public bool IsValid { get; set; }
        [DisplayName("نوع فایل پیوست")]
        public AttachmentType Type { get; set; }

    }
}
