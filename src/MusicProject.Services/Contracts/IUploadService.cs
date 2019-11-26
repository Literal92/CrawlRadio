
using MusicProject.Services.Content;
using Microsoft.AspNetCore.Http;

namespace MusicProject.Services.Contracts
{

  public interface IUploadService
  {
 
    string UploadPicResize(IFormFile file, 
      string path,
      int maxPicWidth,
      ref long serverFileSize,
      int[] size,
      EnumC.Dimensions dimension, ref string[] resized);

    string UploadFile(
      IFormFile file,
      string path,
      ref long serverFileSize,
      string[] allowedExtensions

    );
    string UploadFile(
      IFormFile file,
      string path,
      ref long serverFileSize,
      string[] allowedExtensions,
      string fileName

    );
    string ResizeImage(string source, string destination,string album, int size, EnumC.Dimensions dimension);
  }
}