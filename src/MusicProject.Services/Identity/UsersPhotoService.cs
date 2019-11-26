using System.IO;
using MusicProject.Common.GuardToolkit;
using MusicProject.Entities.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MusicProject.Common.IdentityToolkit;
using MusicProject.Services.Contracts.Identity;
using MusicProject.ViewModels.Identity.Settings;
using Microsoft.Extensions.Options;

namespace MusicProject.Services.Identity
{
    public class UsersPhotoService : IUsersPhotoService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOptionsSnapshot<SiteSettings> _siteSettings;
        private readonly IApplicationUserManager _userManager;

        public UsersPhotoService(
            IHttpContextAccessor contextAccessor,
            IHostingEnvironment hostingEnvironment,
            IOptionsSnapshot<SiteSettings> siteSettings,
            IApplicationUserManager userManager)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _contextAccessor.CheckArgumentIsNull(nameof(_contextAccessor));

            _hostingEnvironment = hostingEnvironment;
            _hostingEnvironment.CheckArgumentIsNull(nameof(_hostingEnvironment));

            _siteSettings = siteSettings;
            _siteSettings.CheckArgumentIsNull(nameof(_siteSettings));
        }

        public string GetUsersAvatarsFolderPath()
        {
            var usersAvatarsFolder = _siteSettings.Value.UsersAvatarsFolder;
            var uploadsRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, usersAvatarsFolder);
            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }
            return uploadsRootFolder;
        }

        public void SetUserDefaultPhoto(User user)
        {
            if (!string.IsNullOrWhiteSpace(user.PhotoFileName))
            {
                return;
            }

            var avatarPath = Path.Combine(GetUsersAvatarsFolderPath(), user.PhotoFileName ?? string.Empty);
            if (!File.Exists(avatarPath))
            {
                user.PhotoFileName = _siteSettings.Value.UserDefaultPhoto;
            }
        }

        public string GetUserDefaultPhoto(string photoFileName)
        {
            if (string.IsNullOrWhiteSpace(photoFileName))
            {
                return _siteSettings.Value.UserDefaultPhoto;
            }

            var avatarPath = Path.Combine(GetUsersAvatarsFolderPath(), photoFileName ?? string.Empty);
            return !File.Exists(avatarPath) ? _siteSettings.Value.UserDefaultPhoto : photoFileName;
        }

        public string GetUserPhotoUrl(string photoFileName)
        {
            photoFileName = GetUserDefaultPhoto(photoFileName);
            return $"~/{_siteSettings.Value.UsersAvatarsFolder}/{photoFileName}";
        }

        public string GetCurrentUserPhotoUrl()
        {
            var photoFileName = _contextAccessor.HttpContext.User.Identity.GetUserClaimValue(ApplicationClaimsPrincipalFactory.PhotoFileName);
            return GetUserPhotoUrl(photoFileName);
        }

        public string FindPhotoByUserId(int id)
        {
            var UserPic = _userManager.FindById(id).PhotoFileName;
            return GetUserPhotoUrl(UserPic);
        }
    }
}
