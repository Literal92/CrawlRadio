using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.Mime;
using MusicProject.Services.Contracts;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MusicProject.Common;

namespace MusicProject.Services.Content
{
    /// <summary>
    /// Summary description for Upload
    /// </summary>
    /// 
    /// 



    public class UploadService : IUploadService
    {

        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

        }
        private bool IsExtensionAllowed(IList<string> allowedExtensions, string fileExtension)
        {
            int length = allowedExtensions.Count;
            if (length == 0)
                return true;
            for (int i = 0; i < length; i++)
            {
                if (String.CompareOrdinal(allowedExtensions[i], fileExtension) == 0)
                {
                    return true;
                }
            }
            return false;
        }



        public string UploadPicResize(
                                      IFormFile file,
                                      string path,
                                      int maxPicWidth,
                                      ref long serverFileSize,
                                      int[] size,
                                      EnumC.Dimensions dimension,
                                      ref string[] resized
                                      )
        {
            var image = Image.FromStream(file.OpenReadStream(), true, true);
            var allowedMimeType = new[] { "image/png", "image/x-png", "image/pjpeg", "image/jpeg", "image/gif" };
            var serverfileName = "";
            var webroot = _hostingEnvironment.WebRootPath;
            path = webroot + path;

            if (!Directory.Exists(path))

                Directory.CreateDirectory(path);


            int result = 0; //Failor Flag

            if (file != null && file.Length != 0)
            {
                var a = file.OpenReadStream();
                var filePath = "";
                var userFileName = file.FileName;
                if (IsExtensionAllowed(allowedMimeType, file.ContentType))
                {

                    var r = new Random();
                    if (File.Exists(Path.Combine(path, userFileName)))
                    {
                        string newstr;
                        do
                        {
                            int randomnumber = r.Next(1, 10);
                            newstr = string.Concat(DateTime.Now.ToShortPersianDateString().Replace("/", ""), randomnumber,
                              userFileName.Replace(" ", ""));
                        } while (File.Exists(path + newstr));

                        filePath = Path.Combine(path, newstr);

                        if (image.Width > maxPicWidth)
                        {


                            var resizedImg = ResizeImage(image, maxPicWidth, EnumC.Dimensions.Width, filePath);
                            // resizedImg.Save(filePath);

                        }
                        else
                        {
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }
                        }

                        serverfileName = newstr;
                        serverFileSize = file.Length / 1024; //Save server file size in KB
                    }
                    else
                    {
                        filePath = Path.Combine(path, userFileName.Replace(" ", ""));

                        if (image.Width > maxPicWidth)
                        {
                            var resizedImg = ResizeImage(image, maxPicWidth, EnumC.Dimensions.Width, filePath);
                            //    resizedImg.Save(filePath);
                        }
                        else
                        {
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }
                        }
                        serverfileName = userFileName.Replace(" ", "");
                        serverFileSize = file.Length / 1024; //Save server file size in KB
                    }

                    for (int i = 0; i < size.Length; i++)
                    {
                        resized[i] = ResizeImage(image, serverfileName, path, size[i], EnumC.Dimensions.Width);
                    }



                    return serverfileName;

                }
                throw new Exception("unexpected extension in upload Service");
            }
            throw new Exception("No File were selected in upload Service");
        }




        public string UploadFile(
                                      IFormFile file,
                                      string path,
                                      ref long serverFileSize,
                                      string[] allowedExtensions

                                      )
        {


            var serverfileName = "";
            var webroot = _hostingEnvironment.WebRootPath;
            path = webroot + path;

            if (!Directory.Exists(path))

                Directory.CreateDirectory(path);


            int result = 0; //Failor Flag

            if (file != null && file.Length != 0)
            {

                var userFileName = file.FileName;
                if (IsExtensionAllowed(allowedExtensions, file.ContentType))
                {

                    var r = new Random();
                    if (File.Exists(Path.Combine(path, userFileName)))
                    {
                        string newstr;
                        do
                        {
                            int randomnumber = r.Next(1, 10);
                            newstr = string.Concat(DateTime.Now.ToShortPersianDateString().Replace("/", ""), randomnumber,
                              userFileName.Replace(" ", ""));
                        } while (File.Exists(path + newstr));

                        var filePath = Path.Combine(path, newstr);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        serverfileName = newstr;
                        serverFileSize = file.Length / 1024; //Save server file size in KB
                    }
                    else
                    {
                        var filePath = Path.Combine(path, userFileName.Replace(" ", ""));

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        serverfileName = userFileName.Replace(" ", "");
                        serverFileSize = file.Length / 1024; //Save server file size in KB
                    }

                    return serverfileName;

                }
                throw new Exception("unexpected extension in upload Service");
            }
            throw new Exception("No File were selected in upload Service");
        }

        public string UploadFileRadio(
                                      byte[] file,
                                      string path,
                                      ref long serverFileSize,
                                      string[] allowedExtensions

                                      )
        {


            var serverfileName = "";
            //var webroot = _hostingEnvironment.WebRootPath;
            //path = webroot + path;

            if (!Directory.Exists(path))

                Directory.CreateDirectory(path);


            int result = 0; //Failor Flag

            if (file != null && file.Length != 0)
            {

                //var userFileName = file.FileName;
                //if (IsExtensionAllowed(allowedExtensions, file.ContentType))
                //{

                var r = new Random();
                if (File.Exists(Path.Combine(path)))
                {
                    string newstr;
                    do
                    {
                        int randomnumber = r.Next(1, 10);
                        newstr = string.Concat(DateTime.Now.ToShortPersianDateString().Replace("/", ""), randomnumber);
                        //userFileName.Replace(" ", ""));
                    } while (File.Exists(path + newstr));

                    var filePath = Path.Combine(path, newstr);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        fileStream.Write(file, 0, file.Length);
                    }

                    serverfileName = newstr;
                    serverFileSize = file.Length / 1024; //Save server file size in KB
                }
                else
                {
                    var filePath = Path.Combine(path);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        fileStream.Write(file, 0, file.Length);

                    }
                    //serverfileName = userFileName.Replace(" ", "");
                    serverFileSize = file.Length / 1024; //Save server file size in KB
                }

                return serverfileName;

                //}
                throw new Exception("unexpected extension in upload Service");
            }
            throw new Exception("No File were selected in upload Service");
        }




        public string UploadFile(
                                      IFormFile file,
                                      string path,
                                      ref long serverFileSize,
                                      string[] allowedExtensions,
                                      string fileName

                                      )
        {


            var serverfileName = "";
            var webroot = _hostingEnvironment.WebRootPath;
            path = webroot + path;

            if (!Directory.Exists(path))

                Directory.CreateDirectory(path);


            int result = 0; //Failor Flag

            if (file != null && file.Length != 0)
            {

                var userFileName =
                  fileName;
                if (IsExtensionAllowed(allowedExtensions, file.ContentType))
                {

                    var r = new Random();
                    if (File.Exists(Path.Combine(path, userFileName)))
                    {
                        string newstr;
                        do
                        {
                            int randomnumber = r.Next(1, 10);
                            newstr = string.Concat(DateTime.Now.ToShortPersianDateString().Replace("/", ""), randomnumber,
                              userFileName.Replace(" ", ""));
                        } while (File.Exists(path + newstr));

                        var filePath = Path.Combine(path, newstr);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        serverfileName = newstr;
                        serverFileSize = file.Length / 1024; //Save server file size in KB
                    }
                    else
                    {
                        var filePath = Path.Combine(path, userFileName.Replace(" ", ""));

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        serverfileName = userFileName.Replace(" ", "");
                        serverFileSize = file.Length / 1024; //Save server file size in KB
                    }

                    return serverfileName;

                }
                throw new Exception("unexpected extension in upload Service" + file.ContentType + userFileName);
            }
            throw new Exception("No File were selected in upload Service");
        }



        #region Resize

        public Image ResizeImage(Image imgToResize, int size, EnumC.Dimensions dimension, string dest)
        {



            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            switch (dimension)
            {
                case EnumC.Dimensions.Width:
                    nPercent = (size / (float)sourceWidth);
                    break;
                default:
                    nPercent = ((float)size / sourceHeight);
                    break;
            }


            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);

            var b = new Bitmap(destWidth, destHeight);
            var g = Graphics.FromImage(b);
            //g.SmoothingMode = SmoothingMode.HighQuality;
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            //g.Dispose();

            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            b.Save(dest, imgToResize.RawFormat);
            g.Dispose();
            b.Dispose();
            //   imgToResize.Dispose();

            return b;
        }

        /// <summary>
        /// This method used to resize images and like resizefile method, but this has most quality
        /// </summary>
        /// <param name="path"></param>
        /// <param name="size">determinde a int for width/height of image and set its height/width proportion to width/height</param>
        /// <param name="dimension">dimension determine size for width or height</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string ResizeImage(string fileName, string path, string album, int size, EnumC.Dimensions dimension)
        {
            var strAddress = _hostingEnvironment.WebRootPath + "/" + path + "/" + fileName;
            if (!File.Exists(strAddress))
                return "no Exist"; // not exist source file

            string dest = _hostingEnvironment.WebRootPath + "/" + path + "/" + album + "/" + size;
            if (!Directory.Exists(dest))
                Directory.CreateDirectory(dest);

            Stream st = new FileStream(strAddress, FileMode.Open);
            var imgToResize = Image.FromStream(st);
            st.Close();
            st.Dispose();
            var sourceWidth = imgToResize.Width;
            var sourceHeight = imgToResize.Height;
            float nPercent = 0;
            switch (dimension)
            {
                case EnumC.Dimensions.Width:
                    nPercent = ((float)size / sourceWidth);
                    break;
                case EnumC.Dimensions.Height:
                    break;
                default:
                    nPercent = (size / (float)sourceHeight);
                    break;
            }
            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);
            var b = new Bitmap(destWidth, destHeight);
            var g = Graphics.FromImage(b);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            b.Save(dest + "/" + album + ".jpg", imgToResize.RawFormat);
            g.Dispose();
            b.Dispose();
            imgToResize.Dispose();
            return album + ".jpg";

        }


        public string ResizeImage(Image imgToResize, string fileName, string path, int size, EnumC.Dimensions dimension)
        {

            string dest = path + "/" + size;
            if (!Directory.Exists(dest))
                Directory.CreateDirectory(dest);




            var sourceWidth = imgToResize.Width;
            var sourceHeight = imgToResize.Height;
            float nPercent = 0;
            switch (dimension)
            {
                case EnumC.Dimensions.Width:
                    nPercent = ((float)size / sourceWidth);
                    break;
                case EnumC.Dimensions.Height:
                    break;
                default:
                    nPercent = (size / (float)sourceHeight);
                    break;
            }
            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);
            var b = new Bitmap(destWidth, destHeight);
            var g = Graphics.FromImage(b);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            b.Save(dest + "/" + fileName, imgToResize.RawFormat);
            g.Dispose();
            b.Dispose();
            imgToResize.Dispose();
            return fileName;

        }


        #endregion




    }
}