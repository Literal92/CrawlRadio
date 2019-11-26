using System;
using System.Collections.Generic;
using MusicProject.Entities.AuditableEntity;
using MusicProject.Entities.Identity;

namespace MusicProject.Entities.Content
{
  public class Tag : IAuditableEntity
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string EnglishTitle { get; set; }
    public string SeoDescription { get; set; }
    public string SeoKeyword { get; set; }


    public string Type { get; set; }
    public string Link { get; set; }
    public string Content { get; set; }
    public string Pic { get; set; }
    public string Thumbnail { get; set; }
    public bool IsSelected { get; set; }
    public bool IsPublish { get; set; }
    public int VisitCount { get; set; }
    public int LikeCount { get; set; }
    public string ChildCount { get; set; }
    public string Website { get; set; }
    public string Facebook { get; set; }
    public string Instagram { get; set; }
    public string Twitter { get; set; }
    public string Soundcloud { get; set; }
    public string Spotify { get; set; }
    public string Itunes { get; set; }
 
    public ICollection<ContentTag> ContentTags { get; set; }
    public ICollection<ContentListTag> ContentListTags { get; set; }
    public ICollection<CategoryTag> CategoryTags { get; set; }
    public ICollection<PodcastTag> PodcastTags { get; set; }
  }
}
