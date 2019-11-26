using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
    public class ContentSelected : IAuditableEntity
  {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public int ContentListId { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public Content Content { get; set; }
        public ContentList ContentList { get; set; }
    }
}
