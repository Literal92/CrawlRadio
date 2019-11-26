using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicProject.ViewModels.Content
{
  public class ContactViewModel
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "لطفا نام خود را وارد نمایید")]
    [StringLength(50,MinimumLength = 3,ErrorMessage = "لطفا برای نام حد اقل سه حرف را وارد نمایید")]
    public string Name { get; set; }
    public string Mobile { get; set; }
    [Required(ErrorMessage = "لطفا ایمیل خود را وارد نمایید")]

    [EmailAddress(ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید.")]
    public string Email { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }
  }
}
