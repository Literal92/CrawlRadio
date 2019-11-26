using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
    public class ContentComment : IAuditableEntity
    {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public Content Content { get; set; }
        public Comment.Comment Comment { get; set; }

    }
}
