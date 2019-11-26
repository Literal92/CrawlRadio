using MusicProject.Entities.AuditableEntity;


namespace MusicProject.Entities.Content
{
  public class Notification : IAuditableEntity
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string Type { get; set; }

  }
}