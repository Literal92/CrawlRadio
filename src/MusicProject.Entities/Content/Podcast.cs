using System;
using System.Collections.Generic;
using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
    public class Podcast : IAuditableEntity
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Title { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeyboard { get; set; }
        public string Pic { get; set; }
        public string Video { get; set; }
        public string Thumbnail { get; set; }

        public DateTimeOffset? PublishDateTime { get; set; }
        public string ContentText { get; set; }
        public string DatePublished { get; set; }
        public string Description { get; set; }

        public int VisitCount { get; set; }
        public int LikeCount { get; set; }
        public bool IsPublish { get; set; }

        public ICollection<PodcastTag> PodcastTags { get; set; }
        public ICollection<PodcastComment> PodcastComments { get; set; }


    }
}
