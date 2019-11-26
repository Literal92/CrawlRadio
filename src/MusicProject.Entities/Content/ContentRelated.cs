using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
    public class ContentRelated : IAuditableEntity
  {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public int Priority { get; set; }
        public int RelatedId { get; set; }

        public Content Content { get; set; }
        public Content Related { get; set; }
    }
}
