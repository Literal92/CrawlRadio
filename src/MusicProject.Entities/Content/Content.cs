using System;
using System.Collections.Generic;
using MusicProject.Entities.AuditableEntity;
using MusicProject.Entities.Identity;

namespace MusicProject.Entities.Content
{
    public class Content : IAuditableEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public int Priority { get; set; }

        public string HeadLine { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Lead { get; set; }
        public string ContentText { get; set; }


        public string Link { get; set; }
        public int LikeCount { get; set; }


        public int VisitCount { get; set; }
        public bool IsArchive { get; set; }

        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeyboard { get; set; }
        public string SeoUrl { get; set; }



        public string Video { get; set; }

        public string Pic { get; set; }
        public string Thumbnail { get; set; }
        public string MediumPic { get; set; }

        public string Pic2 { get; set; }
        public string Thumbnail2 { get; set; }
        public string MediumPic2 { get; set; }

        public string Pic3 { get; set; }
        public string Thumbnail3 { get; set; }
        public string MediumPic3 { get; set; }



        public string Svg { get; set; }
        public string Pdf { get; set; }
        public string Mp3128 { get; set; }
        public string Mp364 { get; set; }
        public string Mp3320 { get; set; }

        public byte[] Music { get; set; }



        public DateTimeOffset? CreatedDateTime { get; set; }
        public DateTimeOffset? PublishDateTime { get; set; }


        public Category Category { get; set; }

        public ICollection<ContentTag> ContentTags { get; set; }
        public ICollection<ContentSelected> ContentSelecteds { get; set; }
        public ICollection<ContentComment> ContentComments { get; set; }
        public ICollection<ContentFile> ContentFiles { get; set; }
        public ICollection<ContentRelated> ContentRelatedsContent { get; set; }
        public ICollection<ContentRelated> ContentRelatedsContentRelated { get; set; }

    }
}
