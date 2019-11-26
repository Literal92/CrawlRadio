using MusicProject.Entities.AuditableEntity;
namespace MusicProject.Entities.Content
{
    public class ContentListFile : IAuditableEntity
    {
        public int Id { get; set; }
        public int ContentListId { get; set; }
        public string Mp364 { get; set; }
        public string Mp3128 { get; set; }
        public string Mp3320 { get; set; }
        public string Title { get; set; }
        public int LikeCount { get; set; }
        public int VisitCount { get; set; }
        public string Type { get; set; }
        public string Ext { get; set; }
        public long FileSize { get; set; }
        public int Order { get; set; }
        public ContentList ContentList { get; set; }
    }
}