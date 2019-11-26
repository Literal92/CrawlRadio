namespace MusicProject.ViewModels.Content
{
    public enum ContentItemActiveTab
    {
        UserInfo,
        UserAdmin
    }

    public class ContentItemViewModel
    {
        public Entities.Content.Content Content { set; get; }
        public bool ShowAdminParts { set; get; }
        public ContentItemActiveTab ActiveTab { get; set; }
    }
}