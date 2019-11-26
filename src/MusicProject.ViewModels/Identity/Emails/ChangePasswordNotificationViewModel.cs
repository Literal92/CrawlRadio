using MusicProject.Entities.Identity;

namespace MusicProject.ViewModels.Identity.Emails
{
    public class ChangePasswordNotificationViewModel : EmailsBase
    {
        public User User { set; get; }
    }
}