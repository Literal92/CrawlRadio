using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
  public class PodcastTag : IAuditableEntity
  {
    public int Id { get; set; }
    public int PodcastId { get; set; }
    public int TagId { get; set; }
    public string Type { get; set; }
    public Podcast Podcast { get; set; }
    public Tag Tag { get; set; }
  }
}
