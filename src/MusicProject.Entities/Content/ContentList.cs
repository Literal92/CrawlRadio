using System;
using System.Collections.Generic;
using MusicProject.Entities.AuditableEntity;
using MusicProject.Entities.Identity;

namespace MusicProject.Entities.Content
{
    public class ContentList : IAuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? PublishDateTime { get; set; }
        public string DatePublished { get; set; }
        public string Type { get; set; }
        public string Pic { get; set; }
        public string Remix { get; set; }
        public bool IsPublish { get; set; }
        public string Thumbnail { get; set; }
        public string Content { get; set; }
        public int VisitCount { get; set; }
        public int LikeCount { get; set; }
        public string ZipMp3128 { get; set; }
        public string ZipMp364 { get; set; }
        public string ZipMp3320 { get; set; }
        public string ZipMp364Size { get; set; }
        public string ZipMp3128Size { get; set; }
        public string ZipMp3320Size { get; set; }
        public ICollection<ContentSelected> ContentSelecteds { get; set; }
        public ICollection<ContentListTag> ContentListTags { get; set; }
        public ICollection<ContentListComment> ContentListComments { get; set; }
        public ICollection<ContentListFile> ContentListFiles { get; set; }

    }
}
