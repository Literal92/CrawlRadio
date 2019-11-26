using System;
using System.Collections.Generic;
using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
  public class Category : IAuditableEntity
  {
    public int Id { get; set; }
    public int TypeId { get; set; }
    public string Title { get; set; }
    public string SeoDescription { get; set; }
    public string SeoKeyboard { get; set; }
    public string Pic { get; set; }
    public string Thumbnail { get; set; }
    public int? ParentId { get; set; }
    public DateTimeOffset? PublishDateTime { get; set; }
    public string ContentText { get; set; }
    public string DatePublished { get; set; }
    public string Link { get; set; }
    public string Description { get; set; }

    public int VisitCount { get; set; }
    public int LikeCount { get; set; }
    public bool IsPublish { get; set; }
    public bool? IsSelected { get; set; }


    public string ZipMp3128 { get; set; }
    public string ZipMp3128Size { get; set; }
    public string ZipMp364 { get; set; }
    public string ZipMp364Size { get; set; }
    public string ZipMp3320 { get; set; }
    public string ZipMp3320Size { get; set; }

    public Category Parent { get; set; }
    public ICollection<Category> InverseParent { get; set; }
    public ICollection<Content> Contents { get; set; }
    public ICollection<CategoryTag> CategoryTags { get; set; }
    public ICollection<CategoryComment> CategoryComments { get; set; }

  }
}
