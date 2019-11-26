using System.Security.Claims;
using System.Security.Principal;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Identity;
using MusicProject.Services.Content;
using MusicProject.Services.Contracts;
using MusicProject.Services.Contracts.Content;
using MusicProject.Services.Contracts.Identity;
using MusicProject.Services.Identity;
using MusicProject.Services.Identity.Logger;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace MusicProject.IocConfig
{
    public static class AddCustomServicesExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, ApplicationDbContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IPrincipal>(provider =>
                provider.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? ClaimsPrincipal.Current);

            services.AddScoped<ILookupNormalizer, CustomNormalizer>();
            services.AddScoped<UpperInvariantLookupNormalizer, CustomNormalizer>();

            services.AddScoped<ISecurityStampValidator, CustomSecurityStampValidator>();
            services.AddScoped<SecurityStampValidator<User>, CustomSecurityStampValidator>();

            services.AddScoped<IPasswordValidator<User>, CustomPasswordValidator>();
            services.AddScoped<PasswordValidator<User>, CustomPasswordValidator>();

            services.AddScoped<IUserValidator<User>, CustomUserValidator>();
            services.AddScoped<UserValidator<User>, CustomUserValidator>();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, ApplicationClaimsPrincipalFactory>();
            services.AddScoped<UserClaimsPrincipalFactory<User, Role>, ApplicationClaimsPrincipalFactory>();

            services.AddScoped<IdentityErrorDescriber, CustomIdentityErrorDescriber>();

            services.AddScoped<IApplicationUserStore, ApplicationUserStore>();
            services.AddScoped<UserStore<User, Role, ApplicationDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>, ApplicationUserStore>();

            services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
            services.AddScoped<UserManager<User>, ApplicationUserManager>();

            services.AddScoped<IApplicationRoleManager, ApplicationRoleManager>();
            services.AddScoped<RoleManager<Role>, ApplicationRoleManager>();

            services.AddScoped<IApplicationSignInManager, ApplicationSignInManager>();
            services.AddScoped<SignInManager<User>, ApplicationSignInManager>();

            services.AddScoped<IApplicationRoleStore, ApplicationRoleStore>();
            services.AddScoped<RoleStore<Role, ApplicationDbContext, int, UserRole, RoleClaim>, ApplicationRoleStore>();

            services.AddScoped<IEmailSender, AuthMessageSender>();
            services.AddScoped<ISmsSender, AuthMessageSender>();

            // services.AddSingleton<IAntiforgery, NoBrowserCacheAntiforgery>();
            // services.AddSingleton<IHtmlGenerator, NoBrowserCacheHtmlGenerator>();

            services.AddScoped<IIdentityDbInitializer, IdentityDbInitializer>();
            services.AddScoped<IUsedPasswordsService, UsedPasswordsService>();
            services.AddScoped<ISiteStatService, SiteStatService>();
            services.AddScoped<IUsersPhotoService, UsersPhotoService>();
            services.AddScoped<ISecurityTrimmingService, SecurityTrimmingService>();
            services.AddScoped<IAppLogItemsService, AppLogItemsService>();



            services.AddScoped<IContentService, EfContentService>();
            services.AddScoped<IContentListFileService, EfContentListFileService>();

            services.AddScoped<ICategoryService, EfCategoryService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<ITagService, EfTagService>();
            services.AddScoped<IContentFileService, EfContentFileService>();
            services.AddScoped<ISessionRequestService, EfSessionRequestService>();
            services.AddScoped<IContactUsService, EfContactUsService>();
            services.AddScoped<ICommentService, EfCommentService>();
            services.AddScoped<IContentListService, EfContentListService>();
            services.AddScoped<IContentListCommentService, EfContentListCommentService>();

            services.AddScoped<IContentSelectedService, EfContentSelectedService>();
            services.AddScoped<IOrderService, EfOrderService>();
            services.AddScoped<ICategoryTagService, EfCategoryTagService>();
            services.AddScoped<ISliderService, EfSliderService>();

            services.AddScoped<IPodcastService, EfPodcastService>();
            services.AddScoped<IAppSettingService, EfAppSettingService>();
            services.AddScoped<IDeviceService, EfDeviceService>();
            services.AddScoped<ICategoryCommentService, EfCategoryCommentService>();

            services.AddScoped<ITokenStoreService, TokenStoreService>();
            return services;
        }
    }
}