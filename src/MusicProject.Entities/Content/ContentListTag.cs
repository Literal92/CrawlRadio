using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
    public class ContentListTag : IAuditableEntity
  {
        public int Id { get; set; }
        public int ContentListId { get; set; }
        public int TagId { get; set; }
        public string Type { get; set; }
        public ContentList ContentList { get; set; }
        public Tag Tag { get; set; }
    }
}
