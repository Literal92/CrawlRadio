using MusicProject.Common.GuardToolkit;
using MusicProject.Common.IdentityToolkit;
using MusicProject.Entities.Identity;
using MusicProject.Services.Contracts.Identity;
using MusicProject.ViewModels.Identity;
using DNTBreadCrumb.Core;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using MusicProject.ViewModels.Identity.Emails;
using MusicProject.ViewModels.Identity.Settings;
using DNTPersianUtils.Core;
using DNTCommon.Web.Core;
using System.Collections.Generic;

namespace MusicProject.Areas.Identity.Controllers
{
  [Area(AreaConstants.IdentityArea)]
  [AllowAnonymous]
  [BreadCrumb(Title = "ثبت نام", UseDefaultRouteUrl = true, Order = 0)]
  public class RegisterController : Controller
  {
    private readonly IEmailSender _emailSender;
    private readonly ILogger<RegisterController> _logger;
    private readonly IApplicationUserManager _userManager;
    private readonly IPasswordValidator<User> _passwordValidator;
    private readonly IUserValidator<User> _userValidator;
    private readonly IOptionsSnapshot<SiteSettings> _siteOptions;

    public RegisterController(
        IApplicationUserManager userManager,
        IPasswordValidator<User> passwordValidator,
        IUserValidator<User> userValidator,
        IEmailSender emailSender,
        IOptionsSnapshot<SiteSettings> siteOptions,
        ILogger<RegisterController> logger
      )
    {
      _userManager = userManager;
      _userManager.CheckArgumentIsNull(nameof(_userManager));

      _passwordValidator = passwordValidator;
      _passwordValidator.CheckArgumentIsNull(nameof(_passwordValidator));

      _userValidator = userValidator;
      _userValidator.CheckArgumentIsNull(nameof(_userValidator));

      _emailSender = emailSender;
      _emailSender.CheckArgumentIsNull(nameof(_emailSender));

      _logger = logger;
      _logger.CheckArgumentIsNull(nameof(_logger));

      _siteOptions = siteOptions;
      _siteOptions.CheckArgumentIsNull(nameof(_siteOptions));
    }

    /// <summary>
    /// For [Remote] validation
    /// </summary>
    [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> ValidateUsername(string username)
    {
      var result = await _userValidator.ValidateAsync(
          (UserManager<User>)_userManager, new User { UserName = username, Email = username + "@mousigha.com" });
      return Json(result.Succeeded ? "true" : result.DumpErrors(useHtmlNewLine: true));
    }


    /// <summary>
    /// For [Remote] validation
    /// </summary>
    [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> ValidatePhone(string phoneNumber)
    {

      List<IdentityError> errors = new List<IdentityError>();

      if (!phoneNumber.IsValidIranianMobileNumber())
      {
        errors.Add(new IdentityError
        {
          Code = "PhoneIsNotCorrect",
          Description = "لطفا شماره تلفن معتبری را وارد کنید."
        });



        //var result = await _userValidator.ValidateAsync(
        //    (UserManager<User>)_userManager, new User { UserName = username, Email =username+"@mousigha.com" });
        return Json("لطفا شماره تلفن معتبری را وارد کنید");
      }
      var user = await _userManager.FindByPhoneAsync(phoneNumber);
      if (user != null)
      {
        return Json("با این شماره کاربری قبلا ثبت نام کرده است");
      }

      return Json("true");
    }


    /// <summary>
    /// For [Remote] validation
    /// </summary>
    [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> ValidatePassword(string password, string username)
    {
      var result = await _passwordValidator.ValidateAsync(
          (UserManager<User>)_userManager, new User { UserName = username }, password);
      return Json(result.Succeeded ? "true" : result.DumpErrors(useHtmlNewLine: true));
    }

    [BreadCrumb(Title = "تائید ایمیل", Order = 1)]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
      if (userId == null || code == null)
      {
        return View("Error");
      }

      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return View("NotFound");
      }

      var result = await _userManager.ConfirmEmailAsync(user, code);
      return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
    }

    [BreadCrumb(Title = "ایندکس", Order = 1)]
    public IActionResult Index()
    {
      //var user1 = new User
      //{
      //  UserName = "jjj",
      //  Email = "javad.mjt@gmail.com",
      //  FirstName = "jaba",
      //  LastName = "aaa"
      //};
      // var aa= _emailSender.SendEmailAsync(
      //  email: "javad.mjt@gmail.com",
      //  subject: "لطفا اکانت خود را تائید کنید",
      //  viewNameOrPath: "~/Areas/Identity/Views/EmailTemplates/_RegisterEmailConfirmation.cshtml",
      //  model: new RegisterEmailConfirmationViewModel
      //  {
      //    User = user1,
      //    EmailConfirmationToken = "jjjj",
      //    EmailSignature = _siteOptions.Value.Smtp.FromName,
      //    MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
      //  });

      return View();
    }

    [BreadCrumb(Title = "تائیدیه ایمیل", Order = 1)]
    public IActionResult ConfirmedRegisteration()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ValidateDNTCaptcha(CaptchaGeneratorLanguage = DNTCaptcha.Core.Providers.Language.Persian)]
    public async Task<IActionResult> Index(RegisterViewModel model)
    {

      if (ModelState.IsValid)
      {

        var random = new Random();
        var code = random.Next(1000, 9999);

        var user = new User
        {
          UserName = model.Username,
          Email = model.Username + "@mousigha.com",
          FirstName = model.FirstName,
          LastName = model.FirstName,
          PhoneNumber = model.PhoneNumber,
          PhoneNumberConfirmed = false,
          IsActive = false,
          EmailConfirmed = false,
          
          ActivationCode = code.ToString()
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          await _userManager.AddToRoleAsync(user, "site-user");
          _logger.LogInformation(3, $"{user.UserName} created a new account with password.");

          if (_siteOptions.Value.EnableEmailConfirmation)
          {
            var code1 = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //ControllerExtensions.ShortControllerName<RegisterController>(), //todo: use everywhere .................

            await _emailSender.SendEmailAsync(
               email: user.Email,
               subject: "لطفا اکانت خود را تائید کنید",
               viewNameOrPath: "~/Areas/Identity/Views/EmailTemplates/_RegisterEmailConfirmation.cshtml",
               model: new RegisterEmailConfirmationViewModel
               {
                 User = user,
                 EmailConfirmationToken = code1,
                 EmailSignature = _siteOptions.Value.Smtp.FromName,
                 MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
               });

            return RedirectToAction(nameof(ConfirmYourEmail));
          }

          if (_siteOptions.Value.EnableMobileConfirmation)
          {
            var api = new Kavenegar.KavenegarApi("7768417353496E56316A5A6464754E516149704C753251465473515177587458");
            var res = api.VerifyLookup(model.PhoneNumber, code.ToString(), "activation");

            return Json(new
            {
              id=user.Id,
              result=true,
              phone=user.PhoneNumber,
              username=user.UserName
            });
            //   return RedirectToAction(nameof(ConfirmYourMobile), new { id = user.Id });

          }
          return RedirectToAction(nameof(ConfirmedRegisteration));
        }

        foreach (var error in result.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
      }

      return Content("Captcha");
    }

    [BreadCrumb(Title = "ایمیل خود را تائید کنید", Order = 1)]
    public IActionResult ConfirmYourEmail()
    {
      return View();
    }

    public IActionResult ConfirmYourMobile(int id)
    {

      ViewBag.id = id;
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
 //   [ValidateDNTCaptcha(CaptchaGeneratorLanguage = DNTCaptcha.Core.Providers.Language.Persian)]
    public async Task<IActionResult> ConfirmYourMobile(string activationCode,string registerUserId)
    {
      if (ModelState.IsValid)
      {
        var user = await _userManager.FindByIdAsync(registerUserId);
        if (activationCode == user.ActivationCode)
        {
          user.PhoneNumberConfirmed = true;
          user.IsActive = true;
          user.EmailConfirmed = true;
          user.PhoneNumberConfirmed = true;
           await _userManager.UpdateAsync(user);
          return Content("true");

        }
      }

      return Content("false");
    }

  }
}