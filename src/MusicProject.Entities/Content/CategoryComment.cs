using MusicProject.Entities.AuditableEntity;

namespace MusicProject.Entities.Content
{
    public class CategoryComment : IAuditableEntity
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int CommentId { get; set; }
        public Comment.Comment Comment { get; set; }

    }
}
