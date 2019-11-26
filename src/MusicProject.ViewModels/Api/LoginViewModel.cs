using System.ComponentModel.DataAnnotations;

namespace MusicProject.ViewModels.Api
{
    public class LoginViewModel
    {
      
        public string Username { get; set; }

        public string DeviceId { get; set; }
        public string FirebaseToken { get; set; }

     
        public string Password { get; set; }

         public string Phone { get; set; }
    }
}