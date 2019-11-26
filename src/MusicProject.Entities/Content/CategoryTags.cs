using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
    public class CategoryTag : IAuditableEntity
  {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int TagId { get; set; }
        public string Type { get; set; }
        public Category Category { get; set; }
        public Tag Tag { get; set; }
    }
}
