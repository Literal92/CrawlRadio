using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
    public class TagComment : IAuditableEntity
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public int CommentId { get; set; }
        public Comment.Comment Comment { get; set; }

    }
}