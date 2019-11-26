using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
    public class ContentTag : IAuditableEntity
  {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public int TagId { get; set; }
        public string Type { get; set; }
        public Content Content { get; set; }
        public Tag Tag { get; set; }
    }
}
