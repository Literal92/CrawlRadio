using System.ComponentModel.DataAnnotations;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;

namespace MusicProject.ViewModels.Content
{
  public class OrderViewModel
  {
    public const string AllowedFiles = ".rar,.zip,.doc,.docx,.pdf";

    public int Id { get; set; }
    public string Subject { get; set; }
    public string ProjectDuration { get; set; }
    public string Level { get; set; }
    public string Budget { get; set; }
    public string Mobile { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Place { get; set; }
    public string Speecher { get; set; }
    public string Senario { get; set; }
    public string SpeechTime { get; set; }
    public string SpeechCount { get; set; }
    public string SpeechType { get; set; }
    public string AnimationTime { get; set; }
    public string AnimationCount { get; set; }
    public string AnimationType { get; set; }
    public string AnimationHasSpeecher { get; set; }
    public string AnimationHasSenario { get; set; }
    public string HasEdit { get; set; }
    public string FrameCount { get; set; }
    public string CameraType { get; set; }
    public string GraphicCount { get; set; }
    public string GraphicTime { get; set; }
    public string GraphicType { get; set; }

    [UploadFileExtensions(AllowedFiles,
      ErrorMessage = "لطفا تنها یک فایل " + AllowedFiles + " را ارسال نمائید.")]
    [DataType(DataType.Upload)]
    public IFormFile Photo { get; set; }
  }
}