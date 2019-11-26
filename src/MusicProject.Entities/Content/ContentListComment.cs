using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
    public class ContentListComment : IAuditableEntity
    {
        public int Id { get; set; }
        public int ContentListId { get; set; }
        public ContentList ContentList { get; set; }
        public int CommentId { get; set; }
        public Comment.Comment Comment { get; set; }
    }
}