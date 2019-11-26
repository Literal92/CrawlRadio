using System.ComponentModel.DataAnnotations;

namespace MusicProject.ViewModels.Identity
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        public string DeviceId { get; set; }
        public string FirebaseToken { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "کلمه‌ی عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "به‌خاطر سپاری کلمه‌ی عبور؟")]
        public bool RememberMe { get; set; }
    }
}