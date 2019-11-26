using MusicProject.Entities.Identity;

namespace MusicProject.ViewModels.Identity.Emails
{
    public class UserProfileUpdateNotificationViewModel : EmailsBase
    {
        public User User { set; get; }
    }
}