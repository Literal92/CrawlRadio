using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MusicProject.ViewModels.Api
{
  public class ActivationViewModel
  {
    public string Username { get; set; }
    public string ActivationCode { get; set; }
    public string Phone { get; set; }
    public string DeviceId { get; set; }
    public string Code { get; set; }
    public string Password { get; set; }

  }
  public class ApiRegisterViewModel
  {
    public const string AllowedImages = ".png,.jpg,.jpeg,.gif";

    public string Username { get; set; }
    public string Password { get; set; }
    public bool Gender { get; set; }
    public string DeviceId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    //[UploadFileExtensions(AllowedImages,
    //  ErrorMessage = "لطفا تنها یک تصویر " + AllowedImages + " را ارسال نمائید.")]
    //[DataType(DataType.Upload)]
    public string Photo { get; set; }
  }


  public class RefreshTokenViewModel
  {
    public string Token { get; set; }
  }
}