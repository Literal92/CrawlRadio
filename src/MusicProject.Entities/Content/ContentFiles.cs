using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{

  public class ContentFile : IAuditableEntity
  {
    public int Id { get; set; }
    public int ContentId { get; set; }
    public string FileName { get; set; }
    public string FileName2 { get; set; }
    public string FileName3 { get; set; }
    public string Title { get; set; }
    public string Pic { get; set; }
    public string Description { get; set; }
    public string ContentText { get; set; }
    public string Thumbnail { get; set; }
    public int LikeCount { get; set; }
    public int VisitCount { get; set; }
    public string Type { get; set; }
    public string Ext { get; set; }
    public long FileSize { get; set; }
    public long FileSize2 { get; set; }
    public long FileSize3 { get; set; }
    public bool? IsPublish { get; set; }
    public bool? IsSelected { get; set; }
    public Content Content { get; set; }
  }
}
