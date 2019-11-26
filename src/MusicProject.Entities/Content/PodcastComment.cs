using System;
using System.Collections.Generic;
using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
    public class PodcastComment : IAuditableEntity
    {
        public int Id { get; set; }
        public int PodcastId { get; set; }
        public Podcast Podcast { get; set; }
        public int CommentId { get; set; }
        public Comment.Comment Comment { get; set; }
    }
}